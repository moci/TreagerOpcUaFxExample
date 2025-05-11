using Opc.UaFx;

namespace OpcUaApi.Api.Events;

[OpcEventType($"ns=2;s={nameof(MachineAlarmEventType)}")]
public class MachineAlarmEventType(IOpcReadOnlyNodeDataStore dataStore) : OpcEvent(dataStore)
{
    public string AlarmId => DataStore.Get<string>($"1:{nameof(Node.AlarmId)}");
    public int AlarmSeverity => DataStore.Get<int>($"1:{nameof(Node.AlarmSeverity)}");
    public DateTime AlarmObservedAt => DataStore.Get<DateTime>($"1:{nameof(Node.AlarmObservedAt)}");

    public sealed class Node : OpcEventNode
    {
        public static OpcNodeId NodeId { get; set; } = OpcNodeId.Null;

        public Node(IOpcNode parent, string name) : base(parent, name)
        {
            AlarmId = new(this, $"1:{nameof(AlarmId)}", string.Empty);
            AlarmSeverity = new(this, $"1:{nameof(AlarmSeverity)}", 0);
            AlarmObservedAt = new(this, $"1:{nameof(AlarmObservedAt)}", DateTime.Now);
        }

        public OpcPropertyNode<string> AlarmId { get; }
        public OpcPropertyNode<int> AlarmSeverity { get; }
        public OpcPropertyNode<DateTime> AlarmObservedAt { get; }

        protected override OpcNodeId DefaultTypeDefinitionId => NodeId;
    }
}