using Opc.UaFx;

using OpcUaApi.Api.Types;

namespace OpcUaApi.Api.NodeManagers;

public sealed class ProductionNodes
{
    private readonly OpcDataVariableNode<OperatorType[]> mTeam;

    public ProductionNodes(OpcFolderNode node)
    {
        mTeam = new(node, "Team")
        {
            Value = [],
        };
    }

    public event EventHandler<IEnumerable<OperatorType>> TeamChanged = delegate { };

    public IEnumerable<OperatorType> Team
    {
        get => mTeam.Value;
        set
        {
            mTeam.Value = [.. value];
            mTeam.ApplyChanges(OpcContext.Empty, true);

            TeamChanged(this, mTeam.Value);
        }
    }
}
