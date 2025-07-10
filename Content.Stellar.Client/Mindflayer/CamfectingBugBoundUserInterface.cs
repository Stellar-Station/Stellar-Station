using Content.Stellar.Shared.Mindflayer;
using Robust.Client.UserInterface;

namespace Content.Stellar.Client.Mindflayer;

public sealed class CamfectingBugBoundUserInterface : BoundUserInterface
{
    private CamfectingBugWindow? _window;

    public CamfectingBugBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<CamfectingBugWindow>();

        _window.CameraSelected += OnCameraSelected;

        Update();
    }

    public override void Update()
    {
        base.Update();

        if (_window is not {} window || !EntMan.TryGetComponent<MindflayerCamfectingBugComponent>(Owner, out var camfecting))
            return;

        window.Update(camfecting);
    }

    private void OnCameraSelected(EntityUid uid)
    {
        SendMessage(new CamfectingBugViewCameraMessage(EntMan.GetNetEntity(uid)));
    }
}
