using Content.Server.RoundEnd;
using Content.Stellar.Shared.CosmicCult.Components;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Stellar.Server.CosmicCult.Components;

/// <summary>
/// Component for the CosmicCultRuleSystem that should store gameplay info.
/// </summary>
[RegisterComponent, Access(typeof(CosmicCultRuleSystem))]
[AutoGenerateComponentPause]
public sealed partial class CosmicCultRuleComponent : Component
{
    /// <summary>
    /// What happens if all of the cultists die.
    /// </summary>
    [DataField]
    public RoundEndBehavior RoundEndBehavior = RoundEndBehavior.ShuttleCall;

    /// <summary>
    /// Sender for shuttle call.
    /// </summary>
    [DataField]
    public LocId RoundEndTextSender = "comms-console-announcement-title-centcom";

    /// <summary>
    /// Text for shuttle call.
    /// </summary>
    [DataField]
    public LocId RoundEndTextShuttleCall = "cosmiccult-elimination-shuttle-call";

    /// <summary>
    /// Text for announcement.
    /// </summary>
    [DataField]
    public LocId RoundEndTextAnnouncement = "cosmiccult-elimination-announcement";

    /// <summary>
    /// List of entities non-cultists are turned into at the end of the round.
    /// </summary>
    [DataField]
    public List<EntProtoId> CosmicMobs =
    [
        "MobCosmicCustodian",
        "MobCosmicOracle",
        "MobCosmicLodestar",
    ];

    /// <summary>
    /// The entity cultists are turned into at the end of the round.
    /// </summary>
    [DataField]
    public EntProtoId CosmicAscended = "MobCosmicAstralAscended";

    /// <summary>
    /// Time for emergency shuttle arrival.
    /// </summary>
    [DataField]
    public TimeSpan EvacShuttleTime = TimeSpan.FromMinutes(5);

    [DataField]
    public HashSet<EntityUid> Cultists = [];

    /// <summary>
    /// When true, prevents the wincondition state of Cosmic Cult from being changed.
    /// </summary>
    [DataField]
    public bool WinLocked;

    /// <summary>
    /// When true, Malign Rifts are unable to spawn.
    /// </summary>
    [DataField]
    public bool RiftStop;

    [DataField]
    public WinType WinType = WinType.CrewMinor;

    /// <summary>
    ///     The cult's monument
    /// </summary>
    public Entity<MonumentComponent> MonumentInGame;

    /// <summary>
    ///     The slow zone of the spawned monument
    /// </summary>
    [DataField]
    public EntityUid MonumentSlowZone;

    /// <summary>
    ///     Current tier of the cult
    /// </summary>
    [DataField]
    public int CurrentTier;

    /// <summary>
    ///     Amount of present crew
    /// </summary>
    [DataField]
    public int TotalCrew;

    /// <summary>
    ///     Amount of cultists
    /// </summary>
    [DataField]
    public int TotalCult;

    /// <summary>
    ///     Percentage of crew that have been converted into cultists
    /// </summary>
    [DataField]
    public double PercentConverted;

    /// <summary>
    ///     How much entropy has been siphoned by the cult
    /// </summary>
    [DataField]
    public int EntropySiphoned;

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan? StewardVoteTimer;

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan? PrepareFinaleTimer;

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan? Tier3DelayTimer;

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan? Tier2DelayTimer;

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan? ExtraRiftTimer;

    [DataField] public SoundSpecifier WarpSFX = new SoundPathSpecifier("/Audio/_ST/CosmicCult/ability_blank.ogg");

    [DataField] public EntProtoId WarpVFX = "CosmicBlankAbilityVFX";
}

public enum WinType : byte
{
    /// <summary>
    ///     Cult complete win. The Cosmic Cult beckoned the final curtain call.
    /// </summary>
    CultComplete,
    /// <summary>
    ///    Cult major win. The Monument reached Stage 3 and was fully empowered.
    /// </summary>
    CultMajor,
    /// <summary>
    ///    Cult minor win. Even if the crew escaped, The Monument reached Stage 3.
    /// </summary>
    CultMinor,
    /// <summary>
    ///     Neutral. The Monument didn't reach Stage 3, The crew escaped, but the Cult Leader also escaped.
    /// </summary>
    Neutral,
    /// <summary>
    ///     Crew minor win. The monument didn't reach Stage 3, The crew escaped, and Cult leader was killed, deconverted, or left on the station.
    /// </summary>
    CrewMinor,
    /// <summary>
    ///     Crew major win. The monument didn't reach Stage 3, The crew escaped, and the cult was killed.
    /// </summary>
    CrewMajor,
    /// <summary>
    ///     Crew complete win. The cult was completely deconverted.
    /// </summary>
    CrewComplete,
}
