namespace GPWebApi.DTO;

public class VerifiedLinkTemplateField 
{
    public Guid VerifiedLinkTemplateFieldId { get; set; }
    public string ControlId { get; set; }
    public string ControlLabel { get; set; }
    public VerifiedLinkFieldType FieldType { get; set; }
    public bool IsRequired { get; set; }
    public string Format { get; set; }
    public VerifiedLinkTemplateFieldValidatorList ValidatorList { get; set; }
    public Dictionary<string, string> Options { get; set; }
    public string DefaultValue { get; set; }
    public int SortOrder { get; set; }
}