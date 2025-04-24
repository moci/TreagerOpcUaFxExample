using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpcUaApi.Api.NodeManagers;

internal static class NodeSet
{
    public static string Xml { get; } = """
<?xml version="1.0" encoding="utf-8"?>
<UANodeSet xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
           xmlns="http://opcfoundation.org/UA/2011/03/UANodeSet.xsd"
           xmlns:uax="http://opcfoundation.org/UA/2008/02/Types.xsd"
           xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <NamespaceUris>
    <Uri>http://opcua.product.company</Uri>
  </NamespaceUris>

  <Aliases>
    <Alias Alias="HasSubtype">i=45</Alias>
  </Aliases>

  <UAObjectType NodeId="ns=1;s=MachineStartedEventType" BrowseName="1:MachineStartedEventType">
    <DisplayName>MachineStartedEventType</DisplayName>
    <References>
      <Reference ReferenceType="HasSubtype" IsForward="false">i=2041</Reference>
    </References>
  </UAObjectType>

  <UAObjectType NodeId="ns=1;s=MachineAlarmEventType" BrowseName="1:MachineAlarmEventType">
    <DisplayName>MachineAlarmEventType</DisplayName>
    <References>
      <Reference ReferenceType="HasSubtype" IsForward="false">i=2041</Reference>
    </References>
  </UAObjectType>

  <UAObjectType NodeId="ns=1;s=ProductionTeamChangedEventType" BrowseName="1:ProductionTeamChangedEventType">
    <DisplayName>ProductionTeamChangedEventType</DisplayName>
    <References>
      <Reference ReferenceType="HasSubtype" IsForward="false">i=2041</Reference>
    </References>
  </UAObjectType>

</UANodeSet>
""";
}
