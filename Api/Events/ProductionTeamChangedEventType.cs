using Opc.UaFx;

using OpcUaApi.Api.Types;

namespace OpcUaApi.Api.Events;

[OpcEventType($"ns=2;s={nameof(ProductionTeamChangedEventType)}")]
public sealed class ProductionTeamChangedEventType(IOpcReadOnlyNodeDataStore dataStore) : OpcEvent(dataStore)
{
    public IEnumerable<OperatorType> Members => DataStore.Get<OperatorType[]>(nameof(Node.Members));

    public class Node : OpcEventNode
        {
            public Node(IOpcNode parent, string name)
                : base(parent, name)
            {
                Members = new(this, "Members")
                {
                    Value = [],
                };
            }

            public OpcPropertyNode<OperatorType[]> Members { get; }

            protected override OpcNodeId DefaultTypeDefinitionId { get; } = $"ns=2;s={nameof(ProductionTeamChangedEventType)}";
        }
}