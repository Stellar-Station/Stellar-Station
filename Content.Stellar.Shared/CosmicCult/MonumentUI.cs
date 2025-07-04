using Content.Stellar.Shared.CosmicCult.Components;
using Content.Stellar.Shared.CosmicCult.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Stellar.Shared.CosmicCult;

[Serializable, NetSerializable]
public enum MonumentKey : byte
{
    Key,
}

[Serializable, NetSerializable]
public sealed class MonumentBuiState : BoundUserInterfaceState
{
    public int CurrentProgress;
    public int TargetProgress;
    public ProtoId<GlyphPrototype> SelectedGlyph;
    public HashSet<ProtoId<GlyphPrototype>> UnlockedGlyphs;

    public MonumentBuiState(int currentProgress, int targetProgress, int progressOffset, ProtoId<GlyphPrototype> selectedGlyph, HashSet<ProtoId<GlyphPrototype>> unlockedGlyphs)
    {
        CurrentProgress = currentProgress - progressOffset;
        TargetProgress = targetProgress - progressOffset;
        SelectedGlyph = selectedGlyph;
        UnlockedGlyphs = unlockedGlyphs;
    }

    public MonumentBuiState(MonumentComponent comp)
    {
        CurrentProgress = comp.CurrentProgress - comp.ProgressOffset;
        TargetProgress = comp.TargetProgress - comp.ProgressOffset;
        SelectedGlyph = comp.SelectedGlyph;
        UnlockedGlyphs = comp.UnlockedGlyphs;
    }
}
