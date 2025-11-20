namespace GPWebApi.DTO;

public class FXDealGetInstructionListRequest
{
    public Guid FXDealId { get; set; }
}

public class FXDealGetInstructionListResponse : DTOResponseBase
{
    public List<FXDealGetInstructionListData> Instructions { get; set; } = new List<FXDealGetInstructionListData>();
}

public class FXDealGetInstructionListData
{
    public Guid ItemId { get; set; }
    public int ItemTypeId { get; set; }
    public string ItemTypeName { get; set; }
    public string ItemReference { get; set; }
    public string PayTo { get; set; }
    public string BeneficiaryStreetAddress1 { get; set; } = String.Empty;
    public string BeneficiaryStreetAddress2 { get; set; } = String.Empty;
    public string BeneficiaryCity { get; set; } = String.Empty;
    public string BeneficiaryStateOrProvince { get; set; } = String.Empty;
    public string BeneficiaryStateOrProvinceText { get; set; } = String.Empty;
    public string BeneficiaryPostalCode { get; set; } = String.Empty;
    public string BeneficiaryCountryCode { get; set; } = String.Empty;
    public string BeneficiaryCountryName { get; set; } = String.Empty;
    public string BeneficiaryAddress1 { get; set; } = String.Empty;
    public string BeneficiaryAddress2 { get; set; } = String.Empty;
    public string BeneficiaryAddress3 { get; set; } = String.Empty;
    public string BeneficiaryAccountNumber { get; set; }
    public string BeneficiaryBankName { get; set; }
    public string BeneficiaryBankBIC { get; set; }
    public string BeneficiaryInfoLine1 { get; set; }
    public string BeneficiaryInfoLine2 { get; set; }
    public string BeneficiaryInfoLine3 { get; set; }
    public string BeneficiaryInfoLine4 { get; set; }
    public decimal Amount { get; set; }
    public string AmountCurrencyCode { get; set; }
    public string AmountCurrencyScale { get; set; }
    public decimal FeeAmount { get; set; }
    public string FeeAmountCurrencyCode { get; set; }
    public string FeeAmountCurrencyScale { get; set; }
    public string ValueDate { get; set; } = String.Empty;
    public string StatusTypeName { get; set; }
}