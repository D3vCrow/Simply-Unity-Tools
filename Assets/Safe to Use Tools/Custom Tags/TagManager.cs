using System.Collections.Generic;
using UnityEngine;

public static class TagManager
{
    // Dictionary to store tagged objects
    private static Dictionary<string, List<GameObject>> taggedObjects = new Dictionary<string, List<GameObject>>();

    public static void Register(GameObject obj, string tag)
    {
        if (!taggedObjects.ContainsKey(tag))
        {
            taggedObjects[tag] = new List<GameObject>();
        }

        if (!taggedObjects[tag].Contains(obj))
        {
            taggedObjects[tag].Add(obj);
        }
    }

    public static void Unregister(GameObject obj, string tag)
    {
        if (taggedObjects.ContainsKey(tag))
        {
            taggedObjects[tag].Remove(obj);

            if (taggedObjects[tag].Count == 0)
            {
                taggedObjects.Remove(tag);
            }
        }
    }

    // Get a List of GameObjects by tag
	public static List<GameObject> FindByTag(string tag)
    {
        if (taggedObjects.ContainsKey(tag))
        {
            return new List<GameObject>(taggedObjects[tag]);
        }

        return new List<GameObject>();
    }

    // Get a single GameObject by tag
    public static GameObject FindUniqueByTag(string tag)
    {
        if (!taggedObjects.ContainsKey(tag))
        {
            Debug.LogWarning($"No GameObject found with the tag '{tag}'.");
            return null;
        }

        var objectsWithTag = taggedObjects[tag];

        if (objectsWithTag.Count == 1)
        {
            return objectsWithTag[0]; // Return the single object if unique
        }

        if (objectsWithTag.Count > 1)
        {
            Debug.LogWarning($"Multiple GameObjects found with the tag '{tag}'. Returning null.");
        }

        return null; // Return null if no unique object is found
    }
}
