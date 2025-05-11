using Opc.UaFx;
using Opc.UaFx.Server;

using OpcUaApi.Api.Events;
using OpcUaApi.Api.Types;


namespace OpcUaApi.Api.NodeManagers;

public class RootNodes : OpcNodeManager
{
    private readonly OpcFolderNode mMachineNode;
    private readonly OpcFolderNode mProductionNode;

    public RootNodes(string defaultNamespace)
        : base(defaultNamespace)
    {
        mMachineNode = new("Machine");
        mProductionNode = new("Production");

        Machine = new(mMachineNode);
        Production = new(mProductionNode);
    }

    protected override IEnumerable<OpcNodeSet> ImportNodes()
    {
        var xml = $"""
<?xml version="1.0" encoding="utf-8"?>
<UANodeSet xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
           xmlns="http://opcfoundation.org/UA/2011/03/UANodeSet.xsd"
           xmlns:uax="http://opcfoundation.org/UA/2008/02/Types.xsd"
           xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <NamespaceUris>
    <Uri>{DefaultNamespace.Value}</Uri>
  </NamespaceUris>

  <Aliases>
    <Alias Alias="HasSubtype">i=45</Alias>
  </Aliases>

  <UAObjectType NodeId="ns=1;s={nameof(MachineStartedEventType)}" BrowseName="1:{nameof(MachineStartedEventType)}">
    <DisplayName>{nameof(MachineStartedEventType)}</DisplayName>
    <References>
      <Reference ReferenceType="HasSubtype" IsForward="false">i=2041</Reference>
    </References>
  </UAObjectType>

  <UAObjectType NodeId="ns=1;s={nameof(MachineAlarmEventType)}" BrowseName="1:{nameof(MachineAlarmEventType)}">
    <DisplayName>{nameof(MachineAlarmEventType)}</DisplayName>
    <References>
      <Reference ReferenceType="HasSubtype" IsForward="false">i=2041</Reference>
    </References>
  </UAObjectType>

  <UAObjectType NodeId="ns=1;s={nameof(ProductionTeamChangedEventType)}" BrowseName="1:{nameof(ProductionTeamChangedEventType)}">
    <DisplayName>{nameof(ProductionTeamChangedEventType)}</DisplayName>
    <References>
      <Reference ReferenceType="HasSubtype" IsForward="false">i=2041</Reference>
    </References>
  </UAObjectType>

</UANodeSet>
""";
        var nodeSet = OpcNodeSet.Parse(xml);
        yield return nodeSet;

        // Get, and store, the finale node ids for each event.

        ProductionTeamChangedEventType.Node.NodeId = DefaultNamespace.GetId(nameof(ProductionTeamChangedEventType));
        MachineAlarmEventType.Node.NodeId = DefaultNamespace.GetId(nameof(MachineAlarmEventType));
        MachineStartedEventType.Node.NodeId = DefaultNamespace.GetId(nameof(MachineStartedEventType));
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
        eventsNode.AddNotifier(SystemContext, productionTeamChanged);

        MachineAlarmEventType.Node machineAlarm = new(eventsNode, "MachineAlarm");
        eventsNode.AddNotifier(SystemContext, machineAlarm);

        MachineStartedEventType.Node machineStarted = new(eventsNode, "MachineStarted");
        eventsNode.AddNotifier(SystemContext, machineStarted);

        Production.TeamChanged += (o, e) =>
        {
            productionTeamChanged.Members.Value = [.. e];

            productionTeamChanged.Time = DateTime.UtcNow;
            productionTeamChanged.ReportEventFrom(SystemContext, eventsNode);
        };
        Machine.AlarmObserved += (o, e) =>
        {
            machineAlarm.AlarmId.Value = e.Id;
            machineAlarm.AlarmSeverity.Value = e.Severity;
            machineAlarm.AlarmObservedAt.Value = e.ObservedAt;

            machineAlarm.Time = DateTime.UtcNow;
            machineAlarm.ReportEventFrom(SystemContext, eventsNode);
        };
        Machine.MachineStarted += (o, e) =>
        {
            machineStarted.Time = DateTime.UtcNow;
            machineStarted.ReportEventFrom(SystemContext, eventsNode);
        };
    }

    public MachineNodes Machine { get; }
    public ProductionNodes Production { get; }
}