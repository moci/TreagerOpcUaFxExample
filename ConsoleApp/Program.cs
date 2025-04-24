using Opc.UaFx;
using Opc.UaFx.Client;

using OpcUaApi.Api;
using OpcUaApi.Api.Events;

namespace OpcUaApi.ConsoleApp;

internal class Program
{
    // https://github.com/Traeger-GmbH/opcuanet-samples/blob/master/cs/Basic/Server.EventTypes/Program.cs
    // https://github.com/Traeger-GmbH/opcuanet-samples/blob/master/cs/Basic/Client.EventTypes/Program.cs

    static void Main(string[] args)
    {
        string address = "opc.tcp://localhost:4840";

        Server server = new(address);
        OpcClient client = new(address);

        server.Start();
        client.Connect();

        Console.WriteLine($"Started server & client at: {address}");

        var filter = OpcFilter
            .Using(client)
            .FromEvents("ns=2;s=MachineStartedEventType")
            .Select();
        client.SubscribeEvent("ns=2;s=Events", filter, OnEventRecievedHandler);

        client.SubscribeDataChange("ns=2;s=Machine/Alarms", OnDataChanged);
        client.SubscribeDataChange("ns=2;s=Production/Team", OnDataChanged);
        client.SubscribeDataChange("ns=2;s=Machine/IsRunning", OnDataChanged);

        var doLoop = true;
        void prependOperator()
        {
            Production.Operator @operator = new()
            {
                Id = Guid.NewGuid().ToString(),
                Cost = Random.Shared.Next(0, 200) / 10.0,
            };
            server.Production.Team = server.Production.Team.Prepend(@operator);
        }
        void deleteFirstOperator() => server.Production.Team = server.Production.Team.Skip(1);
        void quit() => doLoop = false;
        void start() => server.Machine.IsRunning = true;
        void stop() => server.Machine.IsRunning = false;
        void addAlarm()
        {
            Machine.Alarm alarm = new()
            {
                Id = Guid.NewGuid().ToString(),
                Severity = Random.Shared.Next(0, 5),
            };
            server.Machine.ObserveAlarm(alarm);
        }

        Dictionary<string, CliCommand> cli = new()
        {
            ["q"] = new()
            {
                Action = quit,
                Description = "Stop application.",
            },
            ["alarm"] = new()
            {
                Action = addAlarm,
                Description = "Observe a new alarm.",
            },
            ["start"] = new()
            {
                Action = start,
                Description = "Start the machine.",
            },
            ["stop"] = new()
            {
                Action = stop,
                Description = "Stop the machine.",
            },
            ["operator"] = new()
            {
                Action = prependOperator,
                Description = "Prepend operator to operator list."
            },
            ["operator-delete"] = new()
            {
                Action = deleteFirstOperator,
                Description = "Remove first operator of operator list."
            },
        };

        void listCommands()
        {
            foreach (var command in cli)
            {
                Console.WriteLine($"{command.Key}: {command.Value.Description}");
            }
        }

        // Create some data
        addAlarm();
        prependOperator();
        prependOperator();

        while (doLoop)
        {
            var input = (Console.ReadLine()?.Trim() ?? string.Empty).ToLowerInvariant();

            if (cli.TryGetValue(input, out var command)) command.Action();
            else
            {
                Console.WriteLine("Unknown command, enter one of the following commands:");
                listCommands();
            }
        }

        client.Disconnect();
        server.Stop();
    }

    private static void OnDataChanged(object sender, OpcDataChangeReceivedEventArgs e)
    {
        Console.WriteLine($"\tValue change detected for: {e.MonitoredItem.NodeId}");
    }

    private static void OnEventRecievedHandler(object sender, OpcEventReceivedEventArgs e)
    {
        Console.WriteLine($"\tEvent type id: {e.Event.EventTypeId}");
        Console.WriteLine($"\tEvent id: {e.Event.EventId}");
        Console.WriteLine($"\tEvent source name: {e.Event.SourceName}");
        Console.WriteLine($"\tEvent source node id: {e.Event.SourceNodeId}");

        if (e.Event is MachineAlarmEventType alarmEvent) Console.WriteLine("\tMachine alarm event received");
        else if (e.Event is MachineStartedEventType) Console.WriteLine("\tMachine started event received");
        else if (e.Event is ProductionTeamChangedEventType) Console.WriteLine("\tProduction team changed event received");
        else Console.WriteLine("\tUnexpected event received");
    }
}
