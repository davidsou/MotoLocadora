namespace MotoLocadora.BuildingBlocks.Options;

public class EventBusOptions
{
    public const string SectionName = "EventBusConfiguration";

    public string HostAddress { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string MainExchange { get; set; } = string.Empty;
    public bool Ssl { get; set; }
    public List<Exchange> Exchanges { get; set; } = new();
    public List<Queue> Queues { get; set; } = new();
}

public class Exchange
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public class Queue
{
    public string Exchange { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RoutingKey { get; set; } = string.Empty;
}
