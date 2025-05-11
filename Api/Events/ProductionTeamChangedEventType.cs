using Opc.UaFx;

using OpcUaApi.Api.Types;

namespace OpcUaApi.Api.Events;

[OpcEventType($"ns=2;s={nameof(ProductionTeamChangedEventType)}")]
public sealed class ProductionTeamChangedEventType(IOpcReadOnlyNodeDataStore dataStore) : OpcEvent(dataStore)
{
    public OperatorType[] Members => DataStore.Get<OperatorType[]>($"1:{nameof(Node.Members)}");

    public class Node : OpcEventNode
    {
        public static OpcNodeId NodeId { get; set; } = OpcNodeId.Null;

        public Node(IOpcNode parent, string name) : base(parent, name)
        {
            Members = new(this, $"1:{nameof(Members)}")
            {
                Value = [],
            };
        }

        public OpcPropertyNode<OperatorType[]> Members { get; }

        protected override OpcNodeId DefaultTypeDefinitionId => NodeId;
    }
}