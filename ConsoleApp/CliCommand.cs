namespace OpcUaApi.ConsoleApp;

public sealed class CliCommand
{
    public required string Description { get; init; }
    public required Action Action { get; init; }
}
