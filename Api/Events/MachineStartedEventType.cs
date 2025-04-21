using Opc.UaFx;

namespace OpcUaApi.Api.Events;

[OpcEventType($"ns=2;s={nameof(MachineStartedEventType)}")]
public sealed class MachineStartedEventType(IOpcReadOnlyNodeDataStore dataStore) : OpcEvent(dataStore)
{
    public sealed class Node(IOpcNode parent, string name)
        : OpcEventNode(parent, name)
    {
        protected override OpcNodeId DefaultTypeDefinitionId { get; } = $"ns=2;s={nameof(MachineStartedEventType)}";
    }
}