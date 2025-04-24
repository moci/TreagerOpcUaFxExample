using OpcUaApi.Api.NodeManagers;
using Opc.UaFx.Server;

namespace OpcUaApi.Api;

public class Server
{
    private readonly OpcServer mServer;
    private readonly RootNodes mNodes;

    public Server(string address)
    {
        mNodes = new();
        mServer = new(address, mNodes)
        {
            ApplicationName = "opcua.product.company",
            ApplicationUri = new Uri("http://opcua.product.company"),
        };

        Machine = new(mNodes.Machine);
        Production = new(mNodes.Production);
    }
    public void Start() => mServer.Start();
    public void Stop() => mServer.Stop();

    public Machine Machine { get; }
    public Production Production { get; }
}