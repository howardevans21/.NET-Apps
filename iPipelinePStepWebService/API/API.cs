
using DWPipelineWebService;
using iPipelinePStepWebService.API.ENUM;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Security;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static iPipelinePStepWebService.API.ENUM.APIENUM;
using Newtonsoft.Json;

namespace iPipelinePStepWebService.API
{
    internal partial class API
    {
     

        public PathFinderObjectModel CreatePolicyJSONAsync(TPOL policy, List<TCVG> coverages, List<TCLI> clients, List<TCLIA> addresses, List<TCDOC> documents, List<TBENE> beneficiaries)
        {
            PathFinderObjectModel pathFinderObjectModel = new PathFinderObjectModel();

            Util util = new Util();
            ReadConfig readConfig = new ReadConfig();
            string? val = string.Empty;
            string jsonFormatted = string.Empty;
            string jsonCombined = string.Empty;
            int counter = 1;

            readConfig.GetSettings();

            /************************************
             * Process Class ID
             ***********************************/
            val = readConfig.ProcessClassID;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.ProcessClassID, val);
            jsonCombined = jsonFormatted;

            /************************************
             * MIR-USER-SESN-CTRY-CD
             ***********************************/
            val = "BS";
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_USER_SESN_CTRY_CD, val);
            jsonCombined += jsonFormatted;

            /************************************
             * MIR-CO-ID
             ***********************************/
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_CO_ID, "CL");
            jsonCombined += jsonFormatted;

            /************************************
            * MIR-POL_ID
           ***********************************/
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_POL_ID, policy.POL_ID);
            jsonCombined += jsonFormatted;


            /************************************
               * MIR-POL-INS-PURP-CD
            ***********************************/
            val = policy.POL_INS_PURP_CD != null ? policy.POL_INS_PURP_CD : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_POL_INS_PURP_CD, val);
            jsonCombined += jsonFormatted;

            /************************************
             * MIR-CLI-INSRD-CD 
            ***********************************/
            val = policy.POL_CLI_INSRD_CD != null ? policy.POL_CLI_INSRD_CD : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_POL_CLI_INSRD_CD, val);
            jsonCombined += jsonFormatted;

           
            /************************************
           * MIR-POL-CLI-COUNTRY-CD
           ***********************************/
            val = policy.POL_CTRY_CD != null ? policy.POL_CTRY_CD : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_POL_COUNTRY_CD, val);
            jsonCombined += jsonFormatted;

            /************************************
               * MIR-POL-APP-RECV-DT
           ***********************************/
            val = policy.POL_APP_RECV_DT != null ? policy.POL_APP_RECV_DT?.ToString(util.DateFormat) : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_POL_APP_RECV_DT, val);
            jsonCombined += jsonFormatted;

            /************************************
            * MIR-POL-ISS-EFF-DT
            ***********************************/
            val = policy.POL_ISS_EFF_DT != null ? policy.POL_ISS_EFF_DT?.ToString(util.DateFormat) : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_POL_ISS_EFF_DT, val);
            jsonCombined += jsonFormatted;

            /************************************
           * MIR-POL-ISS-LOC-CD
           ***********************************/
            val = policy.POL_ISS_LOC_CD != null ? policy.POL_ISS_LOC_CD : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_POL_ISS_LOC_CD, val);
            jsonCombined += jsonFormatted;

            /************************************
            * MIR-POL-BILL-MODE-CD
            ***********************************/
            val = policy.POL_BILL_MODE_CD != null ? policy.POL_BILL_MODE_CD : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_POL_BILL_MODE_CD, val);
            jsonCombined += jsonFormatted;

            /************************************
           * MIR-POL-BILL-TYP-CD
           ***********************************/
            val = policy.POL_BILL_TYP_CD != null ? policy.POL_BILL_TYP_CD : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_POL_BILL_TYP_CD, val);
            jsonCombined += jsonFormatted;

            /************************************
            * MIR-POL-DIVOPT-CD
            ***********************************/
            val = policy.POL_DIV_OPT_CD != null ? policy.POL_DIV_OPT_CD : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_POL_DIVOPT_CD, val);
            jsonCombined += jsonFormatted;

            /************************************
            * MIR-SERV-AGT-ID
            ***********************************/
            val = policy.SERV_AGT_ID != null ? policy.SERV_AGT_ID : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.SERV.MIR_SERV_AGT_ID, val);
            jsonCombined += jsonFormatted;


            /************************************
            * MIR_AGT_SHRE_PCT
            ***********************************/
            val = policy.AGT_SHRT_PCT_1 != null ? policy.AGT_SHRT_PCT_1.ToString() : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.AGT.MIR_AGT_SHRE_PCT1, val);
            jsonCombined += jsonFormatted;

            /************************************
            * MIR-AGT2_ID
            ***********************************/
            val = policy.AGT_ID_2 != null ? policy.AGT_ID_2 : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.AGT.MIR_AG2_ID, val);
            jsonCombined += jsonFormatted;

            /************************************
             * MIR-AGT2-SHRE-PCT
             ***********************************/
            val = policy.AGT_SHRT_PCT_2 != null ? policy.AGT_SHRT_PCT_2.ToString() : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.AGT.MIR_AGT2_SHRE_PCT, val);
            jsonCombined += jsonFormatted;

            /************************************
            * MIR-POL-CONTINGENT-GIV-NM
            ***********************************/
            val = policy.POL_CONTINGENT_GIV_NM != null ? policy.POL_CONTINGENT_GIV_NM : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_POL_CONTINGENT_GIV_NM, val);
            jsonCombined += jsonFormatted;

            /************************************
            * MIR-POL-CONTINGENT-SUR-NM
            ***********************************/
            val = policy.POL_CONTINGENT_SUR_NM != null ? policy.POL_CONTINGENT_SUR_NM : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_POL_CONTINGENT_SUR_NM, val);
            jsonCombined += jsonFormatted;

            /************************************
            * MIR-POL-CONTINGENT-BTH-DT
            ***********************************/
            val = policy.POL_CONTINGENT_BTH_DT != null ? policy.POL_CONTINGENT_BTH_DT?.ToString(util.DateFormat) : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_POL_CONTINGENT_BTH_DT, val);
            jsonCombined += jsonFormatted;

            /************************************
             * MIR-POL-CONTIN-CLI-INSRD-CD
            ***********************************/
            val = policy.POL_CONTINGENT_CLI_INSRD_CD != null ? policy.POL_CONTINGENT_CLI_INSRD_CD : string.Empty;
            jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL.MIR_POL_CONTIN_CLI_INSRD_CD, val);
            jsonCombined += jsonFormatted;

            /************************************
             * Coverages 
            ***********************************/
            counter = 1;
            foreach (TCVG coverage in coverages)
            {
                val = coverage.ClI_TYP_CD != null ? coverage.ClI_TYP_CD : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CVG.MIR_CVG_TYP_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                val = coverage.CVG_PLAN_ID != null ? coverage.CVG_PLAN_ID : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CVG.MIR_CVG_PLAN_ID_T, val, counter);
                jsonCombined += jsonFormatted;

                val = coverage.CVG_FACE_AMT != null ? coverage.CVG_FACE_AMT.ToString() : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CVG.MIR_CVG_FACE_AMT_T, val, counter);
                jsonCombined += jsonFormatted;

                counter++;
            }

            /************************************
            * Clients
           ***********************************/
            counter = 1;
            foreach (TCLI client in clients)
            {

                val = client.CLI_TYP_CD != null ? client.CLI_TYP_CD : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_TYP_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_INDV_GIV_NM != null ? client.CLI_INDV_GIV_NM : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_INDV_GIV_NM_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_INDV_MID_NM != null ? client.CLI_INDV_MID_NM : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_INDV_MID_NM_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_INDV_SUR_NM != null ? client.CLI_INDV_SUR_NM : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_INDV_SUR_NM_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_INDV_TITL_TXT != null ? client.CLI_INDV_TITL_TXT : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_INDV_TITL_TXT_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_INDV_SFX_NM != null ? client.CLI_INDV_SFX_NM : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_INDV_SFX_NM_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_COMPANY_NM != null ? client.CLI_COMPANY_NM : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_COMPANY_NM_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_BTH_DT != null ? client.CLI_BTH_DT?.ToString(util.DateFormat) : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_BTH_DT_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_SEX_CD != null ? client.CLI_SEX_CD : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_SEX_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_SMKR_CD != null ? client.CLI_SMKR_CD : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_SMKR_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_TAX_ID != null ? client.CLI_TAX_ID : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_TAX_ID_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_BTH_LOC_CD != null ? client.CLI_BTH_LOC_CD : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_BTH_LOC_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_EI_IND != null ? client.CLI_EI_IND : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_EI_IND_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_SHR_IND != null ? client.CLI_SHR_IND : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_SHR_IND_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_EMPL_YR_DUR != null ? client.CLI_EMPL_YR_DUR.ToString() : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_EMPL_YR_DUR_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_EMPL_NM != null ? client.CLI_EMPL_NM : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_EMPL_NM_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_SSN_ID != null ? client.CLI_SSN_ID : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_SSN_ID_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_TIN_ID != null ? client.CLI_TIN_ID : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_TIN_ID_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_NATLTY_CNTRY_CD != null ? client.CLI_NATLTY_CNTRY_CD : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_NATLTY_CNTRY_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_PRM_RES_CNTRY != null ? client.CLI_PRM_RES_CNTRY : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_PREM_RES_CNTRY_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_COMNT_TXT != null ? client.CLI_COMNT_TXT : string.Empty;

                val = val.Replace(",", " ");
                val = val.Replace("\r\n", "");
                val = Regex.Replace(val, @"[^\u0000-\u007F]+", string.Empty);

                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_COMNT_TXT_T, val, counter);
                jsonCombined += jsonFormatted;

                val = client.CLI_INCM_EARN_AMT != null ? client.CLI_INCM_EARN_AMT.ToString() : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_INCM_EARN_AMT_T, val, counter);
                jsonCombined += jsonFormatted;

                counter++;
            }

            /************************************
             * Addresses 
            ***********************************/
            counter = 1;
            foreach (TCLIA address in addresses)
            {

                val = address.CLI_TYP_CD != null ? address.CLI_TYP_CD : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLIA_TYP_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                val = address.CLI_ADDR_TYP_CD != null ? address.CLI_ADDR_TYP_CD : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_ADDR_TYP_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                val = address.CLI_ADDR_LN_1_TXT != null ? address.CLI_ADDR_LN_1_TXT : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_ADDR_LN_1_TXT_T, val, counter);
                jsonCombined += jsonFormatted;

                val = address.CLI_ADDR_LN_2_TXT != null ? address.CLI_ADDR_LN_2_TXT : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_ADDR_LN_2_TXT_T, val, counter);
                jsonCombined += jsonFormatted;

                val = address.CLI_ADDR_LN_3_TXT != null ? address.CLI_ADDR_LN_3_TXT : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_ADDR_LN_3_TXT_T, val, counter);
                jsonCombined += jsonFormatted;

                val = address.CLI_ADDR_ADDL_TXT != null ? address.CLI_ADDR_ADDL_TXT : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_ADDR_ADDL_TXT_T, val, counter);
                jsonCombined += jsonFormatted;

                val = address.CLI_ADDR_MUN_CD != null ? address.CLI_ADDR_MUN_CD : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_ADDR_MUN_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                val = address.CLI_CITY_NM_TXT != null ? address.CLI_CITY_NM_TXT : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_CITY_NM_TXT_T, val, counter);
                jsonCombined += jsonFormatted;

                val = address.CLI_CTRY_CD != null ? address.CLI_CTRY_CD : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_COUNTRY_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                val = address.CLI_PSTL_CD != null ? address.CLI_PSTL_CD : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_PSTL_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                val = address.CLI_CRNT_LOC_CD != null ? address.CLI_CRNT_LOC_CD : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_CRNT_LOC_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                val = address.CLI_ADDR_YR_DUR != null ? address.CLI_ADDR_YR_DUR.ToString() : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_ADDR_YR_DUR_T, val, counter);
                jsonCombined += jsonFormatted;

                /************************************
                 * Contacts  
                ***********************************/
                val = address.CLI_CNCT_EMAIL != null ? address.CLI_CNCT_EMAIL : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLIC.MIR_CLIC_CNTCT_EMAIL_T, val, counter);
                jsonCombined += jsonFormatted;

                val = address.CLI_CNCT_HOME_PH != null ? address.CLI_CNCT_HOME_PH : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLIC.MIR_CLIC_CNTCT_HOME_PH_T, val, counter);
                jsonCombined += jsonFormatted;

                val = address.CLI_CNCT_CELL_PH != null ? address.CLI_CNCT_CELL_PH : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLIC.MIR_CLIC_CNTCT_CELL_PH_T, val, counter);
                jsonCombined += jsonFormatted;

                val = address.CLI_CNCT_WORK_PH != null ? address.CLI_CNCT_WORK_PH : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLIC.MIR_CLIC_CNTCT_WORK_PH_T, val, counter);
                jsonCombined += jsonFormatted;

                counter++;
            }

            /************************************
             * Documents 
            ***********************************/
            counter = 1; // Reset
            if (documents.Count > 0)
            {
                foreach (TCDOC document in documents)
                {
                    val = document.CLI_TYP_CD != null ? document.CLI_TYP_CD : string.Empty;
                    jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_DOC_TYP_CD_T, val, counter);
                    jsonCombined += jsonFormatted;

                    val = document.CLI_DOC_ID != null ? document.CLI_DOC_ID : string.Empty;
                    jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_DOC_ID_T, val, counter);
                    jsonCombined += jsonFormatted;

                    val = document.CLI_DOC_CNTRY_CD != null ? document.CLI_DOC_CNTRY_CD : string.Empty;
                    jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_DOC_CNTRY_CD_T, val, counter);
                    jsonCombined += jsonFormatted;

                    val = document.CLI_DOC_XPRY_DT != null ? document.CLI_DOC_XPRY_DT?.ToString(util.DateFormat) : string.Empty;
                    jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_DOC_XPRY_DT_T, val, counter);
                    jsonCombined += jsonFormatted;

                    counter++;
                }
            }
            else
            {
                val = string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_DOC_TYP_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_DOC_ID_T, val, counter);
                jsonCombined += jsonFormatted;

                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_DOC_CNTRY_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI.MIR_CLI_DOC_XPRY_DT_T, val, counter);
                jsonCombined += jsonFormatted;
            }

            /************************************
            * Beneficiaries 
           ***********************************/
            counter = 1;
            int maxCount = beneficiaries.Count;
            foreach (TBENE beneficiary in beneficiaries)
            {
                val = beneficiary.CLI_TYP_CD != null ? beneficiary.CLI_TYP_CD : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.BNFY.MIR_BNFY_CLI_TYP_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                val = beneficiary.BNFY_NM != null ? beneficiary.BNFY_NM : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.BNFY.MIR_BNFY_NM_T, val, counter);
                jsonCombined += jsonFormatted;

                val = beneficiary.BNFY_TYP_CD != null ? beneficiary.BNFY_TYP_CD : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.BNFY.MIR_BNFY_TYP_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                val = beneficiary.BNFY_REL_INSRD_CD != null ? beneficiary.BNFY_REL_INSRD_CD : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.BNFY.MIR_BNFY_REL_INSRD_CD_T, val, counter);
                jsonCombined += jsonFormatted;

                decimal percentage = 0;
                percentage = beneficiary.BNFY_PRCDS_PCT * 100;
                int wholePercentage = (int)percentage;
                val = wholePercentage.ToString();
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.BNFY.MIR_BNFY_PRCDS_PCT_T, val, counter);
                jsonCombined += jsonFormatted;

                val = beneficiary.BNFY_TRUSTEE_NM != null ? beneficiary.BNFY_TRUSTEE_NM : string.Empty;
                jsonFormatted = util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.BNFY.MIR_BNFY_TRUSTEE_NM_T, val, counter);
                jsonCombined += jsonFormatted;

              
                val = beneficiary.BNFY_TRUSTEE_REL_CD != null ? beneficiary.BNFY_TRUSTEE_REL_CD : string.Empty;
                if (counter == beneficiaries.Count)
                {
                    jsonFormatted = maxCount == counter ? jsonFormatted = util.Format_JSON_BODY_VariableFormatWithNoComma(APIENUM.BNFY.MIR_BNFT_TRUSTEE_REL_CD_T, val, counter)
                      : util.Format_JSON_BODY_VariableFormatWithNoComma(APIENUM.BNFY.MIR_BNFT_TRUSTEE_REL_CD_T, val, counter);
                } else
                {
                    jsonFormatted = maxCount == counter ? jsonFormatted = util.Format_JSON_BODY_VariableFormatWithNoComma(APIENUM.BNFY.MIR_BNFT_TRUSTEE_REL_CD_T, val, counter)
                      : util.Format_JSON_BODY_VariableFormatWithComma(APIENUM.BNFY.MIR_BNFT_TRUSTEE_REL_CD_T, val, counter);

                }
                jsonCombined += jsonFormatted;

                counter++;
            }

            string completeJSON = "{ \r\n\r\n" + jsonCombined + "\r\n}";

            /************************************
           * Web Service Call  
          ***********************************/

            // Debug!
           // completeJSON = "{ \r\n\"ProcessClassID\": \"BF9720-P\\",\r\n\"MIR-USER-SESN-CTRY-CD\": \"BS\",\r\n\"MIR-CO-ID\": \"CL\",\r\n\"MIR-POL-ID\": \"1234\",\r\n\"MIR-POL-CLI-INSRD-CD\": \"\",\r\n\"MIR-POL-COUNTRY-CD\": \"\",\r\n\"MIR-POL-APP-RECV-DT\": \"2024-08-13\",\r\n\"MIR-POL-ISS-EFF-DT\": \"2024-08-13\",\r\n\"MIR-POL-ISS-LOC-CD\": \"\",\r\n\"MIR-POL-BILL-MODE-CD\": \"\",\r\n\"MIR-POL-BILL-TYP-CD\": \"\",\r\n\"MIR-POL-DIVOPT-CD\": \"4\",\r\n\"MIR-SERV-AGT-ID\": \"98989D\",\r\n\"MIR-AGT-SHRE-PCT1\": \"60\",\r\n\"MIR-AG2-ID\": \"99605\",\r\n\"MIR-AGT2-SHRE-PCT\": \"0.40\",\r\n\"MIR-POL-CONTINGENT-GIV-NM\": \"Jonh\",\r\n\"MIR-POL-CONTINGENT-SUR-NM\": \"Congingent\",\r\n\"MIR-POL-CONTINGENT-BTH-DT\": \"2024-08-13\",\r\n\"MIR-CVG-TYP-CD-T[1]\": \"\",\r\n\"MIR-CVG-PLAN-ID-T[1]\": \"\",\r\n\"MIR-CVG-FACE-AMT-T[1]\": \"100000.00\",\r\n\"MIR-CLI-INDV-GIV-NM-T[1]\": \"Joanne\",\r\n\"MIR-CLI-INDV-MID-NM-T[1]\": \"Mid\",\r\n\"MIR-CLI-INDV-SUR-NM-T[1]\": \"NEWTEST\",\r\n\"MIR-CLI-INDV-TITL-TXT-T[1]\": \"MS\",\r\n\"MIR-CLI-INDV-SFX-NM-T[1]\": \"DVD\",\r\n\"MIR-CLI-COMPANY-NM-T[1]\": \"\",\r\n\"MIR-CLI-BTH-DT-T[1]\": \"2024-08-13\",\r\n\"MIR-CLI-SEX-CD-T[1]\": \"F\",\r\n\"MIR-CLI-SMKR-CD-T[1]\": \"S\",\r\n\"MIR-CLI-TAX-ID-T[1]\": \"\",\r\n\"MIR-CLI-BTH-LOC-CD-T[1]\": \"\",\r\n\"MIR-CLI-EI-IND-T[1]\": \"Y\",\r\n\"MIR-CLI-SHR-IND-T[1]\": \"Y\",\r\n\"MIR-CLI-EMPL-YR-DUR-T[1]\": \"11\",\r\n\"MIR-CLI-EMPL-NM-T[1]\": \"GoodFood Restaurant\",\r\n\"MIR-CLI-SSN-ID-T[1]\": \"222222221\",\r\n\"MIR-CLI-TIN-ID-T[1]\": \"\",\r\n\"MIR-CLI-NATLTY-CNTRY-CD-T[1]\": \"US\",\r\n\"MIR-CLI-PREM-RES-CNTRY-CD-T[1]\": \"BS\",\r\n\"MIR-CLI-COMNT-TXT-T[1]\": \"0,Cook\",\r\n\"MIR-CLI-INCM-EARN-AMT-T[1]\": \"500000.00\",\r\n\"MIR-CLI-INDV-GIV-NM-T[2]\": \"Spencer\",\r\n\"MIR-CLI-INDV-MID-NM-T[2]\": \"Mid\",\r\n\"MIR-CLI-INDV-SUR-NM-T[2]\": \"Strand\",\r\n\"MIR-CLI-INDV-TITL-TXT-T[2]\": \"MR\",\r\n\"MIR-CLI-INDV-SFX-NM-T[2]\": \"\",\r\n\"MIR-CLI-COMPANY-NM-T[2]\": \"\",\r\n\"MIR-CLI-BTH-DT-T[2]\": \"2024-10-08\",\r\n\"MIR-CLI-SEX-CD-T[2]\": \"M\",\r\n\"MIR-CLI-SMKR-CD-T[2]\": \"N\",\r\n\"MIR-CLI-TAX-ID-T[2]\": \"\",\r\n\"MIR-CLI-BTH-LOC-CD-T[2]\": \"\",\r\n\"MIR-CLI-EI-IND-T[2]\": \"Y\",\r\n\"MIR-CLI-SHR-IND-T[2]\": \"Y\",\r\n\"MIR-CLI-EMPL-YR-DUR-T[2]\": \"0\",\r\n\"MIR-CLI-EMPL-NM-T[2]\": \"\",\r\n\"MIR-CLI-SSN-ID-T[2]\": \"113998011\",\r\n\"MIR-CLI-TIN-ID-T[2]\": \"\",\r\n\"MIR-CLI-NATLTY-CNTRY-CD-T[2]\": \"US\",\r\n\"MIR-CLI-PREM-RES-CNTRY-CD-T[2]\": \"BS\",\r\n\"MIR-CLI-COMNT-TXT-T[2]\": \"50000,Disabled\",\r\n\"MIR-CLI-INCM-EARN-AMT-T[2]\": \"0.00\",\r\n\"MIR-CLI-INDV-GIV-NM-T[3]\": \"Frank\",\r\n\"MIR-CLI-INDV-MID-NM-T[3]\": \"Mid\",\r\n\"MIR-CLI-INDV-SUR-NM-T[3]\": \"Strand\",\r\n\"MIR-CLI-INDV-TITL-TXT-T[3]\": \"DR\",\r\n\"MIR-CLI-INDV-SFX-NM-T[3]\": \"\",\r\n\"MIR-CLI-COMPANY-NM-T[3]\": \"\",\r\n\"MIR-CLI-BTH-DT-T[3]\": \"2024-08-13\",\r\n\"MIR-CLI-SEX-CD-T[3]\": \"M\",\r\n\"MIR-CLI-SMKR-CD-T[3]\": \"\",\r\n\"MIR-CLI-TAX-ID-T[3]\": \"\",\r\n\"MIR-CLI-BTH-LOC-CD-T[3]\": \"AF\",\r\n\"MIR-CLI-EI-IND-T[3]\": \"Y\",\r\n\"MIR-CLI-SHR-IND-T[3]\": \"Y\",\r\n\"MIR-CLI-EMPL-YR-DUR-T[3]\": \"10\",\r\n\"MIR-CLI-EMPL-NM-T[3]\": \"GoodFood Restaurant\",\r\n\"MIR-CLI-SSN-ID-T[3]\": \"133333333\",\r\n\"MIR-CLI-TIN-ID-T[3]\": \"\",\r\n\"MIR-CLI-NATLTY-CNTRY-CD-T[3]\": \"US\",\r\n\"MIR-CLI-PREM-RES-CNTRY-CD-T[3]\": \"BS\",\r\n\"MIR-CLI-COMNT-TXT-T[3]\": \"0,Business owner\",\r\n\"MIR-CLI-INCM-EARN-AMT-T[3]\": \"1000000.00\",\r\n\"MIR-CLIA-TYP-CD-T[1]\": \"P1\",\r\n\"MIR-CLI-ADDR-TYP-CD-T[1]\": \"PR\",\r\n\"MIR-CLI-ADDR-LN-1-TXT-T[1]\": \"123 Street\",\r\n\"MIR-CLI-ADDR-LN-2-TXT-T[1]\": \"\",\r\n\"MIR-CLI-ADDR-LN-3-TXT-T[1]\": \"\",\r\n\"MIR-CLI-ADDR-MUN-CD-T[1]\": \"\",\r\n\"MIR-CLI-CITY-NM-TXT-T[1]\": \"\",\r\n\"MIR-CLI-COUNTRY-CD-T[1]\": \"BS\",\r\n\"MIR-CLI-PSTL-CD-T[1]\": \"2789\",\r\n\"MIR-CLI-ADDR-YR-DUR-T[1]\": \"10\",\r\n\"MIR-CLIC-CNCT-EMAIL-T[1]\": \"jorob@x.com\",\r\n\"MIR-CLIC-CNCT-HOME-PH-T[1]\": \"290-512-2222\",\r\n\"MIR-CLIC-CNCT-CELL-PH-T[1]\": \"242-113-8313\",\r\n\"MIR-CLIC-CNCT-WORK-PH-T[1]\": \"778-585-8953\",\r\n\"MIR-CLIA-TYP-CD-T[2]\": \"P1\",\r\n\"MIR-CLI-ADDR-TYP-CD-T[2]\": \"FM\",\r\n\"MIR-CLI-ADDR-LN-1-TXT-T[2]\": \"123 US street\",\r\n\"MIR-CLI-ADDR-LN-2-TXT-T[2]\": \"\",\r\n\"MIR-CLI-ADDR-LN-3-TXT-T[2]\": \"\",\r\n\"MIR-CLI-ADDR-MUN-CD-T[2]\": \"\",\r\n\"MIR-CLI-CITY-NM-TXT-T[2]\": \"\",\r\n\"MIR-CLI-COUNTRY-CD-T[2]\": \"US\",\r\n\"MIR-CLI-PSTL-CD-T[2]\": \"999999999\",\r\n\"MIR-CLI-ADDR-YR-DUR-T[2]\": \"5\",\r\n\"MIR-CLIC-CNCT-EMAIL-T[2]\": \"jorob@x.com\",\r\n\"MIR-CLIC-CNCT-HOME-PH-T[2]\": \"290-512-2222\",\r\n\"MIR-CLIC-CNCT-CELL-PH-T[2]\": \"242-113-8313\",\r\n\"MIR-CLIC-CNCT-WORK-PH-T[2]\": \"778-585-8953\",\r\n\"MIR-CLIA-TYP-CD-T[3]\": \"P2\",\r\n\"MIR-CLI-ADDR-TYP-CD-T[3]\": \"PR\",\r\n\"MIR-CLI-ADDR-LN-1-TXT-T[3]\": \"123 Turks Street\",\r\n\"MIR-CLI-ADDR-LN-2-TXT-T[3]\": \"\",\r\n\"MIR-CLI-ADDR-LN-3-TXT-T[3]\": \"\",\r\n\"MIR-CLI-ADDR-MUN-CD-T[3]\": \"\",\r\n\"MIR-CLI-CITY-NM-TXT-T[3]\": \"\",\r\n\"MIR-CLI-COUNTRY-CD-T[3]\": \"\",\r\n\"MIR-CLI-PSTL-CD-T[3]\": \"\",\r\n\"MIR-CLI-ADDR-YR-DUR-T[3]\": \"10\",\r\n\"MIR-CLIC-CNCT-EMAIL-T[3]\": \"\",\r\n\"MIR-CLIC-CNCT-HOME-PH-T[3]\": \"242-143-1111\",\r\n\"MIR-CLIC-CNCT-CELL-PH-T[3]\": \"242-133-9999\",\r\n\"MIR-CLIC-CNCT-WORK-PH-T[3]\": \"\",\r\n\"MIR-CLIA-TYP-CD-T[4]\": \"OP\",\r\n\"MIR-CLI-ADDR-TYP-CD-T[4]\": \"PR\",\r\n\"MIR-CLI-ADDR-LN-1-TXT-T[4]\": \"123 Street\",\r\n\"MIR-CLI-ADDR-LN-2-TXT-T[4]\": \"\",\r\n\"MIR-CLI-ADDR-LN-3-TXT-T[4]\": \"\",\r\n\"MIR-CLI-ADDR-MUN-CD-T[4]\": \"\",\r\n\"MIR-CLI-CITY-NM-TXT-T[4]\": \"\",\r\n\"MIR-CLI-COUNTRY-CD-T[4]\": \"BS\",\r\n\"MIR-CLI-PSTL-CD-T[4]\": \"2789\",\r\n\"MIR-CLI-ADDR-YR-DUR-T[4]\": \"5\",\r\n\"MIR-CLIC-CNCT-EMAIL-T[4]\": \"frank@mid.com\",\r\n\"MIR-CLIC-CNCT-HOME-PH-T[4]\": \"242-111-1111\",\r\n\"MIR-CLIC-CNCT-CELL-PH-T[4]\": \"998-223-4233\",\r\n\"MIR-CLIC-CNCT-WORK-PH-T[4]\": \"242-111-1111\",\r\n\"MIR-CLI-TYP-CD-T[1]\": \"P1\",\r\n\"MIR-CLI-DOC-ID-T[1]\": \"BH12345\",\r\n\"MIR-CLI-DOC-CNTRY-CD-T[1]\": \"BS\",\r\n\"MIR-CLI-DOC-XPRY-DT-T[1]\": \"2024-08-13\",\r\n\"MIR-CLI-TYP-CD-T[2]\": \"P2\",\r\n\"MIR-CLI-DOC-ID-T[2]\": \"US123456\",\r\n\"MIR-CLI-DOC-CNTRY-CD-T[2]\": \"US\",\r\n\"MIR-CLI-DOC-XPRY-DT-T[2]\": \"2024-08-13\",\r\n\"MIR-BNFY-NM-T[1]\": \"Sisi Strand\",\r\n\"MIR-BNFY-CLI-TYP-CD[1]\": \"I\",\r\n\"MIR-BNFY-REL-INSRD-CD-T[1]\": \"DAUGH\",\r\n\"MIR-BNFY-PRCDS-PCT-T[1]\": \"0\",\r\n\"MIR-BNFY-TRUSTEE-NM-T[1]\": \"Brother to Sisi\",\r\n\"MIR-BNFT-TRUSTEE-REL-CD-T[1]\": \"SIBLG\",\r\n\"MIR-BNFY-NM-T[2]\": \"Sean strand\",\r\n\"MIR-BNFY-CLI-TYP-CD[2]\": \"I\",\r\n\"MIR-BNFY-REL-INSRD-CD-T[2]\": \"SON\",\r\n\"MIR-BNFY-PRCDS-PCT-T[2]\": \"0\",\r\n\"MIR-BNFY-TRUSTEE-NM-T[2]\": \"\",\r\n\"MIR-BNFT-TRUSTEE-REL-CD-T[2]\": \"\",\r\n\"MIR-BNFY-NM-T[3]\": \"Sisi strand\",\r\n\"MIR-BNFY-CLI-TYP-CD[3]\": \"R\",\r\n\"MIR-BNFY-REL-INSRD-CD-T[3]\": \"DAUGH\",\r\n\"MIR-BNFY-PRCDS-PCT-T[3]\": \"100\",\r\n\"MIR-BNFY-TRUSTEE-NM-T[3]\": \"Brother to Sisi\",\r\n\"MIR-BNFT-TRUSTEE-REL-CD-T[3]\": \"SIBLG\"\r\n}";

            string url = string.Empty;
            string resource = string.Empty;
            if (readConfig.HTTP_RESOURCE != null && readConfig.HTTP_RESOURCE.Substring(0, 1) != "/")
                resource = "/" + readConfig.HTTP_RESOURCE;
            else resource = readConfig.HTTP_RESOURCE;

            url = readConfig.HTTP_PROTOCOL + "://" + readConfig.HTTP_DOMAIN;

            if (readConfig.HTTP_PORT_NUMBER != null)
                url = url + ":" + readConfig.HTTP_PORT_NUMBER;

            url = url + resource;

            string userID = readConfig.Service_Account_UserID + "@" + readConfig.Service_Account_Domain;

            
            NetworkCredential networkCredential = new NetworkCredential(userID, readConfig.Service_Account_Password);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            RequestCachePolicy cachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            WebRequest.DefaultCachePolicy = cachePolicy;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            request.Credentials = networkCredential;
            request.PreAuthenticate = true;

            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(completeJSON);

   
            request.ContentType = "application/json; charset=UTF-8";
            request.Accept = "application/json";
            request.ContentLength = postBytes.Length;
            Stream requestStream = request.GetRequestStream();

            requestStream.Write(postBytes, 0, postBytes.Length);
            requestStream.Close();
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result = null;
            using (StreamReader rdr = new StreamReader(response.GetResponseStream()))
            {
                result = rdr.ReadToEnd();

                /**********************************
                 * Get response code from PathFinder
                 **********************************/
                var pathFinderResponse = JsonConvert.DeserializeObject<PathFinderResponseModel>(result);
                int lisr_return_cd = -1;
                if (pathFinderResponse != null) lisr_return_cd = pathFinderResponse.LSIR_RETURN_CD;
                
                pathFinderObjectModel = new PathFinderObjectModel(lisr_return_cd, result);
                
                #if DEBUG
                    Console.WriteLine(result);
                #endif
            }

            /******************************************
             * Verify Policy was created in Ingenium
             * Update the Status to C = Complete if policy
             * exists in Ingenum
             **********************************************/
            return pathFinderObjectModel;
        }

    } 
}
