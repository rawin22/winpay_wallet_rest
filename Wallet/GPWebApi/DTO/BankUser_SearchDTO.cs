using System.ComponentModel;
using System.Runtime.Serialization;

namespace GPWebApi.DTO;

public class BankUserSearchRequest
{
    [DefaultValue("0")]
    public int PageIndex { get; set; } = 0;
    [DefaultValue("25")]
    public int PageSize { get; set; } = 0;
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public BankUserSearchSortBy SortBy { get; set; } = BankUserSearchSortBy.None;
    public BankUserSearchSortDirection SortDirection { get; set; } = BankUserSearchSortDirection.Descending;

}

public enum BankUserSearchSortBy
{
    [EnumMember]
    None = 0,
    [EnumMember]
    UserName = 1,
    [EnumMember]
    FirstName = 2,
    [EnumMember]
    LastName = 3
}

public enum BankUserSearchSortDirection
{
    [EnumMember]
    Ascending = 0,
    [EnumMember]
    Descending = 1
}

public class BankUserSearchResponse : DTOResponseBase
{
    public int RecordCount { get; set; } = 0;
    public int TotalRecords { get; set; } = 0;

    public List<BankUserSearchData> Users { get; set; } 
}

public class BankUserSearchData
{
    public string UserId { get; set; } = String.Empty;

    public string UserName { get; set; } = String.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string BranchName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool IsLockedOut { get; set; }

    public bool IsEnabled { get; set; }

    public string LinkedAccessRightTemplateName { get; set; }

    public string LastLogin { get; set; } 
}