using Opc.UaFx;

namespace OpcUaApi.Api.Types;

[OpcDataType($"ns=2;s={nameof(MachineIdentificationType)}")]
[OpcDataTypeEncoding($"ns=2;s={nameof(MachineIdentificationType)}.Binary", Type = OpcEncodingType.Binary)]
public sealed class MachineIdentificationType
{
    public string Name { get; init; } = string.Empty;
    public string Serial { get; init; } = string.Empty;
}
