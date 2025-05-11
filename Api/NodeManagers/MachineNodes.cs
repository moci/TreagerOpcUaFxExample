using Opc.UaFx;

using OpcUaApi.Api.Types;

namespace OpcUaApi.Api.NodeManagers;

public sealed class MachineNodes
{
    private readonly OpcDataVariableNode<bool> mIsRunning;
    private readonly OpcDataVariableNode<MachineIdentificationType> mIdentification;
    private readonly OpcDataVariableNode<MachineAlarmType[]> mAlarms;

    public MachineNodes(OpcFolderNode node)
    {
        mIsRunning = new(node, "IsRunning");
        mIdentification = new(node, "Identification")
        {
            Value = new() { Name = string.Empty, Serial = string.Empty },
        };
        mAlarms = new(node, "Alarms")
        {
            Value = [],
        };

        _ = new OpcMethodNode(node, nameof(SetIdentification), new Action<string, string>(SetIdentification));
    }

    public event EventHandler MachineStarted = delegate { };
    public event EventHandler<MachineAlarmType> AlarmObserved = delegate { };

    public bool IsRunning
    {
        get => mIsRunning.Value;
        set
        {
            if (mIsRunning.Value == value) return;

            mIsRunning.Value = value;
            mIsRunning.ApplyChanges(OpcContext.Empty);

            if (value) MachineStarted(this, EventArgs.Empty);
        }
    }
    public IEnumerable<MachineAlarmType> Alarms => mAlarms.Value;

    public void ObserveAlarm(MachineAlarmType alarm)
    {
        mAlarms.Value = [.. Alarms.Append(alarm)];
        mAlarms.ApplyChanges(OpcContext.Empty, true);
        AlarmObserved(this, alarm);
    }
    public void SetIdentification(
        [OpcArgument("Name", Description = "Sets the name of the machine.")]
        string name,
        [OpcArgument("Serial", Description = "Sets the serial of the machine.")]
        string serial)
    {
        mIdentification.Value = new() { Name = name, Serial = serial };
        mIdentification.ApplyChanges(OpcContext.Empty);
    }
}