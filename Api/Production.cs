using OpcUaApi.Api.NodeManagers;
using OpcUaApi.Api.Types;

namespace OpcUaApi.Api;

public sealed class Production(ProductionNodes nodes)
{
    public IEnumerable<Operator> Team
    {
        get => [.. nodes.Team.Select(o => new Operator() { Id = o.Id, Cost = o.Cost })];
        set => nodes.Team = value.Select(o => new OperatorType(){ Id = o.Id, Cost = o.Cost });
    }

    public sealed record Operator
    {
        public required string Id { get; init; }
        public required double Cost { get; init; }
    }
}