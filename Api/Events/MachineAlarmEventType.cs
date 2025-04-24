using Opc.UaFx;

namespace OpcUaApi.Api.Events;

[OpcEventType(Node.NodeId)]
public class MachineAlarmEventType(IOpcReadOnlyNodeDataStore dataStore) : OpcEvent(dataStore)
{
    public string AlarmId => DataStore.Get<string>(nameof(Node.AlarmId));
    public int AlarmSeverity => DataStore.Get<int>(nameof(Node.AlarmSeverity));
    public DateTime AlarmObservedAt => DataStore.Get<DateTime>(nameof(Node.AlarmObservedAt));

    public sealed class Node : OpcEventNode
    {
        public const string NodeId = $"ns=1;s={nameof(MachineAlarmEventType)}";

        public Node(string name) : base(name)
        {
            AlarmId = new(this, nameof(AlarmId), string.Empty);
            AlarmSeverity = new(this, nameof(AlarmSeverity), 0);
            AlarmObservedAt = new(this, nameof(AlarmObservedAt), DateTime.Now);
        }
        public Node(IOpcNode parent, string name) : base(parent, name)
        {
            AlarmId = new(this, nameof(AlarmId), string.Empty);
            AlarmSeverity = new(this, nameof(AlarmSeverity), 0);
            AlarmObservedAt = new(this, nameof(AlarmObservedAt), DateTime.Now);
        }

        public OpcPropertyNode<string> AlarmId { get; }
        public OpcPropertyNode<int> AlarmSeverity { get; }
        public OpcPropertyNode<DateTime> AlarmObservedAt { get; }

        protected override OpcNodeId DefaultTypeDefinitionId { get; } = NodeId;
    }
}