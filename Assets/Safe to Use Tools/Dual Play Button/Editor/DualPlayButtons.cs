using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine.UIElements;

[Overlay(typeof(SceneView), "Dual Play Buttons")]
public class PlayModeOverlay : Overlay
{
    public override VisualElement CreatePanelContent()
    {
        var root = new VisualElement();
        root.style.flexDirection = FlexDirection.Row;

        var full = new Button(() =>
        {
            EditorSettings.enterPlayModeOptionsEnabled = false;
            EditorApplication.isPlaying = !EditorApplication.isPlaying;
        })
        { text = "▶ Reload" };

        var fast = new Button(() =>
        {
            EditorSettings.enterPlayModeOptionsEnabled = true;
            EditorSettings.enterPlayModeOptions =
                EnterPlayModeOptions.DisableDomainReload |
                EnterPlayModeOptions.DisableSceneReload;

            EditorApplication.isPlaying = !EditorApplication.isPlaying;
        })
        { text = "⚡ Fast" };

        root.Add(full);
        root.Add(fast);
        return root;
    }
}
