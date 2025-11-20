namespace GPLibrary.DataObjects
{
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

	public class VerifiedLinkTemplateFieldValidator
	{
		public string validatorType { get; set; }
		public string validatorValue { get; set; }
		public int errorCode { get; set; }
		public string errorMessage { get; set; }
	}


	public class VerifiedLinkTemplateFieldList : List<VerifiedLinkTemplateField>
	{
	}

	public class VerifiedLinkTemplateFieldValidatorList : List<VerifiedLinkTemplateFieldValidator>
	{
	}


	public class VerifiedLinkTemplateFieldValidationType
	{
		public const string RegEx = "regex";
		public const string StringLengthMin = "string_length_min";
		public const string StringLengthMax = "string_length_max";
		public const string IntValueMin = "int_value_min";
		public const string IntValueMax = "int_value_max";
		public const string DecimalValueMin = "decimal_value_min";
		public const string DecimalValueMax = "decimal_value_maxx";
		public const string DateValueMin = "date_value_min";
		public const string DateValueMax = "date_value_max";
	}


	public enum VerifiedLinkFieldType
	{
		String = 1,
		Integer = 2,
		Boolean = 3,
		Decimal = 4,
		Date = 5,
		DateTime = 6,
		List = 7
	}
}

