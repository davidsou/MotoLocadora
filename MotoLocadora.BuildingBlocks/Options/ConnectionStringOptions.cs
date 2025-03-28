namespace MotoLocadoraBuildingBlocks.Options;

public class ConnectionStringOptions
{
    public const string SecctionName = "ConnectionStrings";
    public string SqlConnection { get; set; } = string.Empty;
    public string NsqlConnection { get; set; } = string.Empty;
    public string NsqlDataBase { get; set; } = string.Empty;
}
