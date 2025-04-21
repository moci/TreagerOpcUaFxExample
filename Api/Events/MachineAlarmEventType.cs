using Opc.UaFx;

namespace OpcUaApi.Api.Events;

[OpcEventType($"ns=2;s={nameof(MachineAlarmEventType)}")]
public class MachineAlarmEventType(IOpcReadOnlyNodeDataStore dataStore) : OpcEvent(dataStore)
{
    public string AlarmId => DataStore.Get<string>(nameof(Node.AlarmId));
    public int AlarmSeverity => DataStore.Get<int>(nameof(Node.AlarmSeverity));
    public DateTime AlarmObservedAt => DataStore.Get<DateTime>(nameof(Node.AlarmObservedAt));

    public sealed class Node : OpcEventNode
    {
        public Node(IOpcNode parent, string name)
            : base(parent, name)
        {
            AlarmId = new(this, nameof(AlarmId), string.Empty);
            AlarmSeverity = new(this, nameof(AlarmSeverity), 0);
            AlarmObservedAt = new(this, nameof(AlarmObservedAt), DateTime.Now);
        }

        public OpcPropertyNode<string> AlarmId { get; }
        public OpcPropertyNode<int> AlarmSeverity { get; }
        public OpcPropertyNode<DateTime> AlarmObservedAt { get; }

        protected override OpcNodeId DefaultTypeDefinitionId { get; } = $"ns=2;s={nameof(MachineAlarmEventType)}";
    }
}