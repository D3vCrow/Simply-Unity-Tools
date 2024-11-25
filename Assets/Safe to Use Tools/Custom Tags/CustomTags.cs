using System.Collections.Generic;
using UnityEngine;

public class CustomTags : MonoBehaviour
{
    public List<string> tags = new List<string>();

    private void OnEnable()
    {
        foreach (var tag in tags)
        {
            TagManager.Register(gameObject, tag);
        }
    }

    private void OnDisable()
    {
        foreach (var tag in tags)
        {
            TagManager.Unregister(gameObject, tag);
        }
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            // Re-register tags when modified in play mode
            OnDisable();
            OnEnable();
        }
    }
}
