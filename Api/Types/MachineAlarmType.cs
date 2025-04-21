using Opc.UaFx;

namespace OpcUaApi.Api.Types;

[OpcDataType($"ns=2;s={nameof(MachineAlarmType)}")]
[OpcDataTypeEncoding($"ns=2;s={nameof(MachineAlarmType)}.Binary", Type = OpcEncodingType.Binary)]
public sealed class MachineAlarmType
{
    public string Id { get; init; } = string.Empty;
    public int Severity { get; init; } = 0;
    public DateTime ObservedAt { get; init; } = DateTime.UtcNow;
}
