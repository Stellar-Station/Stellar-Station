using Content.Stellar.Shared.Dropshadow;
using Robust.Client.GameObjects;
using Robust.Shared.Utility;

namespace Content.Stellar.Client.Dropshadow;

public sealed partial class DropshadowSystem : SharedDropshadowSystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SpriteSystem _sprite = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DropshadowComponent, ComponentStartup>(OnDropshadowAdded);
        SubscribeLocalEvent<DropshadowComponent, ComponentShutdown>(OnDropshadowRemoved);

        SubscribeLocalEvent<DropshadowComponent, AppearanceChangeEvent>(OnAppearanceChanged);
    }

    private void OnDropshadowAdded(Entity<DropshadowComponent> ent, ref ComponentStartup args)
    {
        if (!TryComp<SpriteComponent>(ent, out var sprite))
            return;

        var layer = _sprite.AddLayer((ent, sprite), ent.Comp.Sprite, 0);
        _sprite.LayerMapSet((ent, sprite), DropshadowLayers.Shadow, layer);
        _sprite.LayerSetOffset((ent, sprite), DropshadowLayers.Shadow, ent.Comp.Offset);
        sprite.LayerSetShader(DropshadowLayers.Shadow, "unshaded");
    }

    private void OnDropshadowRemoved(Entity<DropshadowComponent> ent, ref ComponentShutdown args)
    {
        if (!TryComp<SpriteComponent>(ent, out var sprite))
            return;

        _sprite.RemoveLayer((ent, sprite), DropshadowLayers.Shadow, false);
    }

    private void OnAppearanceChanged(Entity<DropshadowComponent> ent, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (_appearance.TryGetData<bool>(ent, DropshadowVisuals.Visible, out var visible, args.Component))
            _sprite.LayerSetVisible((ent, args.Sprite), DropshadowLayers.Shadow, visible);
    }
}
