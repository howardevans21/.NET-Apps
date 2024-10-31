using DWPipelineWebService.DataModel;
using System.Configuration;

namespace DWPipelineWebService.DataModel;

internal class Policy_ApplicationInfoModel
{

    internal Policy_ApplicationInfoModel()
    {

    }

    internal Policy_ApplicationInfoModel(bool eConsentAcknowledged, bool optOutContact, string policyDateRequested, int specifiedDay, string uwClassInsured1, string uwClassInsured2)
    {
        this.eConsentAcknowledged = eConsentAcknowledged;
        this.optOutContact = optOutContact;
        this.issueDate_policy_DateRequested = policyDateRequested;
        this.issueDate_specifiedDay = specifiedDay;
        issueDate_Type = getIssueDateType(policyDateRequested);
        this.uwClassInsured1 = uwClassInsured1;
        this.uwClassInsured2 = uwClassInsured2;
     
    }

    private bool eConsentAcknowledged = false;
    public bool EConsentAcknowledged
    {
        get { return eConsentAcknowledged;  } 
    }

    private string uwClassInsured1 = string.Empty;
    public string UWClassInsured1
    {
        get { return uwClassInsured1; }
    }

    private string uwClassInsured2 = string.Empty;
    public string UWClassInsured2 { get { return uwClassInsured2; } }

    private bool optOutContact = false;

    public bool OptOutContact
    {
        get { return optOutContact; }

    }

    /// <summary>
    /// Policy Date Requested for Issue Date
    /// A description of the selectioin 
    /// </summary>
    private string issueDate_policy_DateRequested = string.Empty;
    public string issueDate_PolicyDateRequested { get { return issueDate_policy_DateRequested; } }

    /// <summary>
    /// Specified Day entered for this day of the current month
    /// </summary>
    private int issueDate_specifiedDay = -1;
    public int IssueDate_SpecifiedDay { get { return issueDate_specifiedDay; } }

    private IssueDateType issueDate_Type = IssueDateType.None; 
    public IssueDateType IssueDate_Type { get { return issueDate_Type; } set { issueDate_Type = value; } }

    private IssueDateType getIssueDateType(string translationText)
    {
        IssueDateType t = IssueDateType.None;

        string[] s = issueDate_PolicyDateRequested.Split(' ', ';', ',');
        
        // Bool variables for Current Day of the Month
        // All True = Curent Day of The Month 
        bool containsDate = false;
        bool containsMonth = false;
        bool containsDay = false; 
        
        foreach(string c in s)
        {
            if (c.ToLower().Contains("date"))
                containsDate = true;

            if (c.ToLower().Contains("month"))
                containsMonth = true;

            if (c.ToLower().Contains("day"))
                containsDay = true;
        }

        if (containsDate && containsMonth && containsDay)
            t = IssueDateType.CurrentDayOfTheMonth;


        return t;
    }
    
}
