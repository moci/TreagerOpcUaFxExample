using Opc.UaFx;

using OpcUaApi.Api.Types;

namespace OpcUaApi.Api.NodeManagers;

public sealed class MachineNodes
{
    private readonly Func<OpcContext> mGetContext;

    private readonly OpcDataVariableNode<bool> mIsRunning;
    private readonly OpcDataVariableNode<MachineIdentificationType> mIdentification;
    private readonly OpcDataVariableNode<MachineAlarmType[]> mAlarms;

    public MachineNodes(OpcFolderNode node, Func<OpcContext> getContext)
    {
        mGetContext = getContext;

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
            mIsRunning.ApplyChanges(mGetContext());

            if (value) MachineStarted(this, EventArgs.Empty);
        }
    }
    public IEnumerable<MachineAlarmType> Alarms => mAlarms.Value;

    public void ObserveAlarm(MachineAlarmType alarm)
    {
        mAlarms.Value = [.. Alarms.Append(alarm)];
        mAlarms.ApplyChanges(mGetContext(), true);
        AlarmObserved(this, alarm);
    }
    public void SetIdentification(
        [OpcArgument("Name", Description = "Sets the name of the machine.")]
        string name,
        [OpcArgument("Serial", Description = "Sets the serial of the machine.")]
        string serial)
    {
        mIdentification.Value = new() { Name = name, Serial = serial };
        mIdentification.ApplyChanges(mGetContext());
    }
}