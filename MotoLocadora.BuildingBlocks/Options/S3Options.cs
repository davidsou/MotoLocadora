namespace MotoLocadora.BuildingBlocks.Options;


public class S3Options
{
    public const string SectionName = "S3Configuration";

    public string BucketName { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
}
