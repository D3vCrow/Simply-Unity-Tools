using UnityEditor;
using UnityEngine;

public class AnimationPoseTool : EditorWindow
{
    private GameObject selectedCharacter;
    private Animator animator;
    private AnimationClip animationClip;
    private float normalizedTime = 0f;

    [MenuItem("Tools/Animation Pose Tool")]
    public static void ShowWindow()
    {
        GetWindow<AnimationPoseTool>("Animation Pose Tool");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Pose Sampler Tool", EditorStyles.boldLabel);

        selectedCharacter = (GameObject)EditorGUILayout.ObjectField("Character", selectedCharacter, typeof(GameObject), true);
        animationClip = (AnimationClip)EditorGUILayout.ObjectField("Animation Clip", animationClip, typeof(AnimationClip), false);
        normalizedTime = EditorGUILayout.Slider("Normalized Time", normalizedTime, 0f, 1f);

        EditorGUI.BeginDisabledGroup(selectedCharacter == null || animationClip == null);

        if (GUILayout.Button("Sample Pose"))
            SamplePose();

        if (GUILayout.Button("Bake Pose To Scene"))
            BakePose();

        EditorGUI.EndDisabledGroup();
    }

    /// <summary>
    /// Temporarily applies the animation at a specific normalized time.
    /// This is essentially a "preview" — the scene isn't modified permanently.
    /// </summary>
    private void SamplePose()
    {
        if (selectedCharacter == null || animationClip == null)
        {
            Debug.LogWarning("Character or AnimationClip not set.");
            return;
        }

        animator = selectedCharacter.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Selected character has no Animator component.");
            return;
        }

        // SampleAnimation directly updates the transforms of the GameObject.
        animationClip.SampleAnimation(selectedCharacter, animationClip.length * normalizedTime);

        // Force the scene to visually update.
        SceneView.RepaintAll();
    }

    /// <summary>
    /// Takes whatever pose is currently shown in the Scene view and turns it into 
    /// real, permanent transform values. Think of this as "freezing" the pose.
    /// </summary>
    private void BakePose()
    {
        if (selectedCharacter == null)
        {
            Debug.LogWarning("No character selected.");
            return;
        }

        // Full undo support — lets the user revert pose baking.
        Undo.RegisterFullObjectHierarchyUndo(selectedCharacter, "Bake Pose");

        // Actually apply (and mark dirty) transforms so Unity saves them.
        ApplyTransformRecursive(selectedCharacter.transform);

        Debug.Log("Pose baked to transform hierarchy.");
    }

    /// <summary>
    /// Re-applies each transform's current values so Unity registers them as modified.
    /// Marking as dirty ensures the scene will save the change.
    /// </summary>
    private void ApplyTransformRecursive(Transform root)
    {
        // Capture current values
        Vector3 pos = root.localPosition;
        Quaternion rot = root.localRotation;
        Vector3 scale = root.localScale;

        // Reassign them so Unity sees them as "changed"
        root.localPosition = pos;
        root.localRotation = rot;
        root.localScale = scale;

        // Tell Unity this object has been modified and should be saved
        EditorUtility.SetDirty(root);

        // Do the same for all children
        foreach (Transform child in root)
            ApplyTransformRecursive(child);
    }
}
