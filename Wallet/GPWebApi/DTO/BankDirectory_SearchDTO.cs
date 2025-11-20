using System.Runtime.Serialization;

namespace GPWebApi.DTO;

public class BankDirectorySearchRequest
{
    public string BankCode { get; set; } = string.Empty;

    public BankDirectorySearchCodeType CodeType { get; set; } = BankDirectorySearchCodeType.BIC;
}

public enum BankDirectorySearchCodeType
{
    [EnumMember]
    BIC = 0,
    [EnumMember]
    FW = 1,
    [EnumMember]
    AU = 2,
    [EnumMember]
    NZ = 3,
    [EnumMember]
    IN = 4
}

public class BankDirectorySearchResponse : DTOResponseBase
{
    public BankDirectorySearchData BankInfo { get; set; }
}

public class BankDirectorySearchData
{
    public string BankCode { get; set; } 
    public string BIC { get; set; }
    public string BankName { get; set; }
    public string BankNameSwift { get; set; }
    public string BranchName { get; set; }
    public string CountryCode { get; set; }
    public string CountryName { get; set; }
    public string StreetAddress1 { get; set; }
    public string StreetAddress2 { get; set; }
    public string City { get; set; }
    public string StateOrProvince { get; set; }
    public string StateOrProvinceName { get; set; }
    public string SwiftAddress1 { get; set; }
    public string SwiftAddress2 { get; set; }
    public string SwiftAddress3 { get; set; }

    public bool IsACH = false;
}


