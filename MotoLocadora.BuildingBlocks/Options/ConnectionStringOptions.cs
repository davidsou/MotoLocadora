namespace MotoLocadora.BuildingBlocks.Options;

public class ConnectionStringOptions
{
    public const string SectionName = "ConnectionStrings";
    public string SqlConnection { get; set; } = string.Empty;
    public string NsqlConnection { get; set; } = string.Empty;
    public string NsqlDataBase { get; set; } = string.Empty;
}
