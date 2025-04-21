using Opc.UaFx;

using OpcUaApi.Api.Types;

namespace OpcUaApi.Api.NodeManagers;

public sealed class ProductionNodes
{
    private readonly Func<OpcContext> mGetContext;
    private readonly OpcDataVariableNode<OperatorType[]> mTeam;

    public ProductionNodes(OpcFolderNode node, Func<OpcContext> getContext)
    {
        mGetContext = getContext;

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
            var before = Team.Count();
            mTeam.Value = [.. value];
            mTeam.ApplyChanges(mGetContext(), true);

            var after = Team.Count();
            if (before == after) return;

            TeamChanged(this, mTeam.Value);
        }
    }
}
