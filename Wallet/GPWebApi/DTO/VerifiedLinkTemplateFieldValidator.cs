namespace GPWebApi.DTO;

public class VerifiedLinkTemplateFieldValidator
{
    public string validatorType { get; set; }
    public string validatorValue { get; set; }
    public int errorCode { get; set; }
    public string errorMessage { get; set; }
}

