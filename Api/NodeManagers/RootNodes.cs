using Opc.UaFx;
using Opc.UaFx.Server;

using OpcUaApi.Api.Events;
using OpcUaApi.Api.Types;


namespace OpcUaApi.Api.NodeManagers;

public class RootNodes : OpcNodeManager
{
    private readonly OpcFolderNode mMachineNode;
    private readonly OpcFolderNode mProductionNode;

    public RootNodes()
        : base("http://opcua.product.company")
    {
        mMachineNode = new("Machine");
        mProductionNode = new("Production");

        Machine = new(mMachineNode, () => SystemContext);
        Production = new(mProductionNode, () => SystemContext);
    }

    protected override IEnumerable<IOpcNode> CreateNodes(OpcNodeReferenceCollection references)
    {
        yield return new OpcDataTypeNode<OperatorType>();
        yield return new OpcDataTypeNode<MachineAlarmType>();
        yield return new OpcDataTypeNode<MachineIdentificationType>();

        OpcFolderNode eventsNode = new("Events");
        OpcDataVariableNode<string> versionNode = new("Version", "1.2.3");

        references.Add(eventsNode, OpcObjectTypes.ObjectsFolder);
        references.Add(versionNode, OpcObjectTypes.ObjectsFolder);
        references.Add(mMachineNode, OpcObjectTypes.ObjectsFolder);
        references.Add(mProductionNode, OpcObjectTypes.ObjectsFolder);

        yield return eventsNode;
        yield return versionNode;
        yield return mMachineNode;
        yield return mProductionNode;

        ProductionTeamChangedEventType.Node productionTeamChanged = new(eventsNode, "ProductionTeamChanged");
        MachineAlarmEventType.Node machineAlarm = new(eventsNode, "MachineAlarm");
        MachineStartedEventType.Node machineStarted = new(eventsNode, "MachineStarted");

        references.Add(productionTeamChanged, OpcObjectTypes.EventTypesFolder);
        references.Add(machineAlarm, OpcObjectTypes.EventTypesFolder);
        references.Add(machineStarted, OpcObjectTypes.EventTypesFolder);

        eventsNode.AddNotifier(SystemContext, productionTeamChanged);
        eventsNode.AddNotifier(SystemContext, machineAlarm);
        eventsNode.AddNotifier(SystemContext, machineStarted);

        Production.TeamChanged += (o, e) =>
        {
            //productionTeamChanged.EventId = Guid.NewGuid().ToByteArray();
            productionTeamChanged.Time = DateTime.UtcNow;
            productionTeamChanged.ReceiveTime = DateTime.UtcNow;
            productionTeamChanged.Members.Value = [.. e];

            productionTeamChanged.ReportEventFrom(SystemContext, eventsNode);
        };
        Machine.AlarmObserved += (o, e) =>
        {
            //machineAlarm.EventId = Guid.NewGuid().ToByteArray();
            machineAlarm.Time = DateTime.UtcNow;
            machineAlarm.ReceiveTime = DateTime.UtcNow;
            machineAlarm.AlarmId.Value = e.Id;
            machineAlarm.AlarmSeverity.Value = e.Severity;
            machineAlarm.AlarmObservedAt.Value = e.ObservedAt;

            machineAlarm.ReportEventFrom(SystemContext, eventsNode);
        };
        Machine.MachineStarted += (o, e) =>
        {
            //machineStarted.EventId = Guid.NewGuid().ToByteArray();
            machineStarted.Time = DateTime.UtcNow;
            machineStarted.ReceiveTime = DateTime.UtcNow;

            machineStarted.ReportEventFrom(SystemContext, eventsNode);
        };
    }

    public MachineNodes Machine { get; }
    public ProductionNodes Production { get; }
}