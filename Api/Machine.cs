using OpcUaApi.Api.NodeManagers;
using OpcUaApi.Api.Types;

namespace OpcUaApi.Api;

public sealed class Machine(MachineNodes nodes)
{
    public bool IsRunning
    {
        get => nodes.IsRunning;
        set => nodes.IsRunning = value;
    }

    public void ObserveAlarm(Alarm alarm)
    {
        MachineAlarmType typedAlarm = new()
        {
            Id = alarm.Id,Severity = 
            alarm.Severity, 
            ObservedAt = DateTime.UtcNow,
        };
        nodes.ObserveAlarm(typedAlarm);
    }

    public sealed record Alarm
    {
        public required string Id { get; init; }
        public required int Severity { get; init; }
    }
}
