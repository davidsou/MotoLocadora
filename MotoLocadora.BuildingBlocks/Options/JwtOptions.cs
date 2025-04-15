namespace MotoLocadora.BuildingBlocks.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpireMinutes { get; set; }
}
