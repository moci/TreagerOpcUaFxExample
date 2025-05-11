using Opc.UaFx;

namespace OpcUaApi.Api.Types;

[OpcDataType($"ns=2;s={nameof(OperatorType)}")]
[OpcDataTypeEncoding($"ns=2;s={nameof(OperatorType)}.Binary", Type = OpcEncodingType.Binary)]
public sealed class OperatorType
{
    public required string Id { get; init; }
    public required double Cost { get; init; }
}