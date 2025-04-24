using Opc.UaFx;

using OpcUaApi.Api.Types;

namespace OpcUaApi.Api.Events;

[OpcEventType(Node.NodeId)]
public sealed class ProductionTeamChangedEventType(IOpcReadOnlyNodeDataStore dataStore) : OpcEvent(dataStore)
{
    public IEnumerable<OperatorType> Members => DataStore.Get<OperatorType[]>(nameof(Node.Members));

    public class Node : OpcEventNode
    {
        public const string NodeId = $"ns=1;s={nameof(ProductionTeamChangedEventType)}";

        public Node(string name) : base(name) 
        {
            Members = new(this, nameof(Members), []);
        }
        public Node(IOpcNode parent, string name)
                : base(parent, name)
        {
            Members = new(this, nameof(Members), []);
        }

        public OpcPropertyNode<OperatorType[]> Members { get; }

        protected override OpcNodeId DefaultTypeDefinitionId { get; } = NodeId;
    }
}