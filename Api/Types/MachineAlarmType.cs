using Opc.UaFx;

namespace OpcUaApi.Api.Types;

[OpcDataType($"ns=2;s={nameof(MachineAlarmType)}")]
[OpcDataTypeEncoding($"ns=2;s={nameof(MachineAlarmType)}.Binary", Type = OpcEncodingType.Binary)]
public sealed class MachineAlarmType
{
    public required string Id { get; init; }
    public required int Severity { get; init; }
    public required DateTime ObservedAt { get; init; }
}
