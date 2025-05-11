using Opc.UaFx;

namespace OpcUaApi.Api.Events;

[OpcEventType($"ns=2;s={nameof(MachineStartedEventType)}")]
public sealed class MachineStartedEventType(IOpcReadOnlyNodeDataStore dataStore) : OpcEvent(dataStore)
{
    public sealed class Node : OpcEventNode
    {
        public static OpcNodeId NodeId { get; set; } = OpcNodeId.Null;

        public Node(IOpcNode parent, string name) : base(parent, name) { }

        protected override OpcNodeId DefaultTypeDefinitionId => NodeId;
    }
}