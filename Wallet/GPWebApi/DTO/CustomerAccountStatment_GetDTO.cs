namespace GPWebApi.DTO;

public class CustomerAccountStatementGetResponse : DTOResponseBase
{
    public CustomerAccountStatementData AccountInfo { get; set; }

    public List<CustomerAccountStatementEntryData> Entries { get; set; }
}

public class CustomerAccountStatementData
{
    public string AccountId { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string AccountCurrencyCode { get; set; } = string.Empty;
    public int AccountCurrencyScale { get; set; } = 0;
    public decimal BeginningBalance { get; set; } = decimal.Zero;
    public decimal EndingBalance { get; set; } = decimal.Zero;
}

public class CustomerAccountStatementEntryData
{
    public string EntryTypeName { get; set; } = string.Empty;
    public int ItemTypeId { get; set; } = 0;
    public string ItemTypeName { get; set; } = string.Empty;
    public string ItemId { get; set; } = string.Empty;
    public string ItemReference { get; set; } = string.Empty;
    public decimal AmountCredit { get; set; } = decimal.Zero;
    public decimal AmountDebit { get; set; } = decimal.Zero;
    public string ValueDate { get; set; } = String.Empty;
    public string BankMemo { get; set; } = string.Empty;
}

