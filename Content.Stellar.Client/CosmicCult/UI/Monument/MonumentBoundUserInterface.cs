using Content.Stellar.Shared.CosmicCult;
using Content.Stellar.Shared.CosmicCult.Components;
using Content.Stellar.Shared.CosmicCult.Prototypes;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;

namespace Content.Stellar.Client.CosmicCult.UI.Monument;

[UsedImplicitly]
public sealed class MonumentBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    [ViewVariables]
    private MonumentMenu? _menu;

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<MonumentMenu>();

        _menu.OnSelectGlyphButtonPressed += protoId => SendMessage(new GlyphSelectedMessage(protoId));
        _menu.OnRemoveGlyphButtonPressed += () => SendMessage(new GlyphRemovedMessage());

        _menu.OnGainButtonPressed += OnInfluenceSelected;
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);
        if (state is not MonumentBuiState buiState)
            return;

        _menu?.UpdateState(buiState);
    }

    private void OnInfluenceSelected(ProtoId<InfluencePrototype> selectedInfluence)
    {
        SendMessage(new InfluenceSelectedMessage(selectedInfluence));
    }
}
