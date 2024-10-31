namespace DWPipelineWebService.DataModel;

internal class ArrangementModel
{
    public ArrangementModel() { }

    private string paymentMethod = string.Empty;
    public string PaymentMethod { get { return paymentMethod; } set { paymentMethod = value; } }

    private string arrMode = string.Empty;
    public string ArrMode { get { return arrMode; } set { arrMode = value; } }
}

