using System.ComponentModel;
using System.Runtime.Serialization;

namespace GPWebApi.DTO;

public class WhiteLabelProfileSearchRequest
{
    [DefaultValue("0")]
    public int PageIndex { get; set; } = 0;
    [DefaultValue("25")]
    public int PageSize { get; set; } = 0;
    public WhiteLabelProfileSearchSortBy SortBy { get; set; } = WhiteLabelProfileSearchSortBy.None;
    public WhiteLabelProfileSearchSortDirection SortDirection { get; set; } = WhiteLabelProfileSearchSortDirection.Descending;
    public WhiteLabelProfileSearchOptions SearchOptions { get; set; } = new WhiteLabelProfileSearchOptions();
}

public enum WhiteLabelProfileSearchSortBy
{
    [EnumMember]
    None = 0,
    [EnumMember]
    WhiteLabelProfileName = 1
}

public enum WhiteLabelProfileSearchSortDirection
{
    [EnumMember]
    Ascending = 0,
    [EnumMember]
    Descending = 1
}

public class WhiteLabelProfileSearchOptions
{
    public string WhiteLabelProfileName { get; set; } = string.Empty;
}

public class WhiteLabelProfileSearchResponse : DTOResponseBase
{
    public int RecordCount { get; set; } = 0;
    public int TotalRecords { get; set; } = 0;
    public List<WhiteLabelProfileSearchData> WhiteLabelProfiles { get; set; } = new List<WhiteLabelProfileSearchData> ();   
}

public class WhiteLabelProfileSearchData
{
    public string WhiteLabelProfileId { get; set; } = String.Empty;

    public string WhiteLabelProfileName { get; set; } = String.Empty;

    public string AppTheme { get; set; } = string.Empty;

    public string TimeZoneName { get; set; } = string.Empty;
}