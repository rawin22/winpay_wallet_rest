namespace GPWebApi.DTO;

public class FXRateTableGetRateFromSymbolResponse : DTOResponseBase
{
    public FXTableRate? Rate { get; set; } = null;
}
