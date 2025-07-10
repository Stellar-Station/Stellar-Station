using Content.Server.Store.Systems;
using Content.Stellar.Shared.Mindflayer;
using Content.Shared.FixedPoint;

namespace Content.Stellar.Server.Mindflayer;

public sealed class MindflayerSystem : SharedMindflayerSystem
{
    [Dependency] private readonly StoreSystem _store = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MindflayerComponent, MindflayerShopActionEvent>(OnShopAction);
    }

    private void OnShopAction(Entity<MindflayerComponent> ent, ref MindflayerShopActionEvent args)
    {
        _store.ToggleUi(args.Performer, ent);
    }

    protected override void OnSiphonMindDoAfter(Entity<MindflayerComponent> ent, ref MindflayerSiphonMindDoAfterEvent args)
    {
        base.OnSiphonMindDoAfter(ent, ref args);

        _store.TryAddCurrency(new Dictionary<string, FixedPoint2>() {
            {ent.Comp.SiphonCurrency, ent.Comp.SiphonQuantity}
        }, ent);
    }
}
