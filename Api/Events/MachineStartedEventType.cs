using Opc.UaFx;

namespace OpcUaApi.Api.Events;

[OpcEventType(Node.NodeId)]
public sealed class MachineStartedEventType(IOpcReadOnlyNodeDataStore dataStore) : OpcEvent(dataStore)
{
    public sealed class Node : OpcEventNode
    {
        public const string NodeId = $"ns=1;s={nameof(MachineStartedEventType)}";

        public Node(string name) : base(name) { }
        public Node(IOpcNode parent, string name) : base(parent, name) { }

        protected override OpcNodeId DefaultTypeDefinitionId { get; } = NodeId;
    }
}