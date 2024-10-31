using DWPipelineWebService;
using iPipelinePStepWebService.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static iPipelinePStepWebService.API.ENUM.APIENUM;

namespace iPipelinePStepWebService
{
    public sealed class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);                       
                    }

                  
                    /*****************************************
                     * Read Policy Data 
                     * Convert Data to JSON 
                     *************************************/
                    using (var eFContext = new EFContext())
                    {
                        eFContext.Database.EnsureCreated();

                        ReadConfig readConfig = new ReadConfig();
                        readConfig.GetSettings();

                        List<TPOL> policies = eFContext.Policies.Where(x => x.POL_CREATE_STAT_CD != null && x.POL_CREATE_STAT_CD.ToUpper() == "R").ToList();
                        API.API api = new API.API();

                        List<SP_POLICY_RESULTS> pendingPolicies = getPendingPolcies(eFContext);

                        foreach (TPOL pol in policies)
                        {
                            try
                            {
                              
                                List<TERROR> policyErrors = eFContext.Errors.Where(x => x.POL_ID != null && x.POL_ID.ToLower() == pol.POL_ID.ToLower()).ToList();

                                bool doesPolicyContainErrors = false;
                                if (policyErrors != null && policyErrors.Count > 0) doesPolicyContainErrors = true;

                                // Verify a pending policy exists
                                // If true = proceed, If false = do not attempt to create an already existing policy
                                bool isPolicyPending = pendingPolicies.Where(x => x.POL_ID.ToLower() == pol.POL_ID.ToLower()) != null ? true : false;

                                if (isPolicyPending && !doesPolicyContainErrors)
                                {                                   
                                    List<TCVG> coverages = eFContext.Coverages.Where(x => x.POL_ID == pol.POL_ID).ToList();
                                    List<TCLI> clients = eFContext.Clients.Where(x => x.POL_ID == pol.POL_ID).ToList();
                                    List<TCLIA> addresses = eFContext.Addresses.Where(x => x.POL_ID == pol.POL_ID).ToList();

                                    List<TCDOC> documents = eFContext.Documents.Where(x => x.POL_ID == pol.POL_ID).ToList();
                                    List<TBENE> beneficiaries = eFContext.Beneficiaries.Where(x => x.POL_ID == pol.POL_ID).ToList();

                                    // Check if the required records are in the system before processing the policy
                                    if (coverages.Count > 0 && clients.Count > 0 && addresses.Count > 0)
                                    {
                                        PathFinderObjectModel pathFinderObjectModel = api.CreatePolicyJSONAsync(pol, coverages, clients, addresses, documents, beneficiaries); // Create policy in Ingenium using the PathFinder web service

                                        // Update TPOL status to P = PathFinder process was called 
                                        pol.POL_CREATE_STAT_CD = "P";
                                        eFContext.SaveChanges();

                          
                                        // Verify Policy exists in Ingenium after policy creating process
                                        // Policy may not exists in Ingenium due to network failures or other possible reasons, etc.
                                        List<SP_POLICY_RESULTS> verifiedPolicies = eFContext.VerifyPolicy(pol.POL_ID).ToList();
                                        if (verifiedPolicies != null && verifiedPolicies.Count > 0)
                                        {
                                            // Update TPOL status to C = Complete
                                            pol.POL_CREATE_STAT_CD = "C";
                                            eFContext.SaveChanges();
                                        }
                                        else
                                        {
                                            // Log Error if LISR RETURN CODE IS AN ERROR
                                            bool isError = isPolicyAnError(readConfig, pathFinderObjectModel);

                                            if (isError)
                                            {
                                                pol.POL_CREATE_STAT_CD = "E";
                                                eFContext.SaveChanges();
                                                string msg = string.Format("Policy Number: {0}\r\n {1}", pol.POL_ID, pathFinderObjectModel.SERVER_RESPONSE_MESSAGE);
                                                _logger.LogError(pathFinderObjectModel.SERVER_RESPONSE_MESSAGE);
                                                saveErrorData(pathFinderObjectModel.SERVER_RESPONSE_MESSAGE, pol.POL_ID, "WS");
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string errorMessage = getErrorExceptionMessage(ex);
                                _logger.LogError(ex, "{Message}", ex.Message);
                                saveErrorData(errorMessage, pol.POL_ID, "IG");                               
                            }
                        }
                    }

                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = getErrorExceptionMessage(ex);
                saveErrorData(errorMessage, "", "IG");
                Environment.Exit(1); // In order for Windows Service Management to leverage configured recovery options,
                                     // we need to have the process terminated with a non-zero code
            }
        }

        #region Helper Functions
        private bool isPolicyAnError(ReadConfig readConfig, PathFinderObjectModel pathFinderResponseModel)
        {
            bool isError = false;
            int errorCode = -1;
            foreach(string s in readConfig.ErrorCodes)
            {
                int.TryParse(s, out errorCode);

                if (errorCode == pathFinderResponseModel.LISR_RETURN_CODE) isError = true;
            }

            return isError;
        }

        private string getErrorExceptionMessage(Exception ex)
        {
            string innerException = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
            string stackTrace = ex.StackTrace != null ? ex.StackTrace : string.Empty;
            string errorMessage = string.Format("{0}\r\n\r\n{1}\\r\n\r\n{2}", ex.Message, ex.InnerException, stackTrace);
            return errorMessage;
        }
        private void saveErrorData(string errorMessage, string policyID, string appID)
        {
            using (var eFContext = new EFContext())
            {
                eFContext.Database.EnsureCreated();
                TERROR errorRecord = new TERROR(appID, policyID, errorMessage, DateTime.Now);
                eFContext.Add(errorRecord);
                eFContext.SaveChanges();
            }
        }

        private List<SP_POLICY_RESULTS> getPendingPolcies(EFContext context)
        {
           return context.Get_Pending_Policies(30).ToList();
        }

        #endregion
    }
}
