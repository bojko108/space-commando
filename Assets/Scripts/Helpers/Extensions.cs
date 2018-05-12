using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public static class TransformExtensions
{
    /// <summary>
    /// Find all children of the Transform by name (includes self)
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="names"></param>
    /// <returns></returns>
    public static List<Transform> FindChildrenByName(this Transform transform, params string[] names)
    {
        List<Transform> list = new List<Transform>();
        foreach (var tran in transform.Cast<Transform>().ToList())
            list.AddRange(tran.FindChildrenByName(names)); // recursively check children
        if (names.Any(name => name == transform.name))
            list.Add(transform); // we matched, add this transform
        return list;
    }
    /// <summary>
    /// Find all children of the GameObject by name (includes self)
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="names"></param>
    /// <returns></returns>
    public static List<GameObject> FindChildrenByName(this GameObject gameObject, params string[] names)
    {
        return FindChildrenByName(gameObject.transform, names)
            //.Cast<GameObject>() // Can't use Cast here :(
            .Select(tran => tran.gameObject)
            .Where(gameOb => gameOb != null)
            .ToList();
    }

    /// <summary>
    /// Find all children of the Transform by tag (includes self)
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="tags"></param>
    /// <returns></returns>
    public static List<Transform> FindChildrenByTag(this Transform transform, params string[] tags)
    {
        List<Transform> list = new List<Transform>();
        foreach (var tran in transform.Cast<Transform>().ToList())
            list.AddRange(tran.FindChildrenByTag(tags)); // recursively check children
        if (tags.Any(tag => tag == transform.tag))
            list.Add(transform); // we matched, add this transform
        return list;
    }

    /// <summary>
    /// Find all children of the GameObject by tag (includes self)
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="tags"></param>
    /// <returns></returns>
    public static List<GameObject> FindChildrenByTag(this GameObject gameObject, params string[] tags)
    {
        return FindChildrenByTag(gameObject.transform, tags)
            //.Cast<GameObject>() // Can't use Cast here :(
            .Select(tran => tran.gameObject)
            .Where(gameOb => gameOb != null)
            .ToList();
    }
}