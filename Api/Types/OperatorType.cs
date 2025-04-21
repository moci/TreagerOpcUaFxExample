using Opc.UaFx;

namespace OpcUaApi.Api.Types;

[OpcDataType($"ns=2;s={nameof(OperatorType)}")]
[OpcDataTypeEncoding($"ns=2;s={nameof(OperatorType)}.Binary", Type = OpcEncodingType.Binary)]
public sealed class OperatorType
{
    public string Id { get; set; } = string.Empty;
    public double Cost { get; set; } = 0;
}