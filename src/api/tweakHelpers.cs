using System;
using System.Reflection;
using System.Text;
using UnityEngine;

static class TweakExtensions
{
    /// <summary>
    /// Call a private function using reflection
    /// </summary>
    public static object CallPrivate(this object o, string methodName, params object[] args)
    {
        var mi = o.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (mi != null)
        {
            return mi.Invoke(o, args);
        }

        return null;
    }

    /// <summary>
    /// Get the value of a private field using reflection
    /// </summary>
    public static T GetPrivateField<T>(this object o, string name)
    {
        return (T)(o.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(o));
    }

    /// <summary>
    /// Search all decendants for a specifically named component of the correct type
    /// </summary>
    public static T FindAChild<T>(this Transform t, string name) where T : UnityEngine.Object
    {
        Component[] transforms = t.GetComponentsInChildren(typeof(Transform), true);
        foreach (Transform transform in transforms)
        {
            if (transform.gameObject.name == name)
            {
                return transform.GetComponent<T>();
            }
        }

        return null;
    }
}

static class TweakHelpers
{
    /// <summary>
    /// Write out all the fields for the specified object
    /// </summary>
    public static void LogFields(object t, string title)
    {
        StringBuilder sb = new StringBuilder(title + "\r\n");
        var fields = t.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var f in fields)
        {
            sb.AppendFormat("{0} ({1}) = {2}\r\n", f.Name, f.GetType().ToString(), f.GetValue(t));
        }
        Debug.Log(sb.ToString());
    }

    /// <summary>
    /// Write out all the properties for the specified object
    /// </summary>
    public static void LogProperties(object t, string title)
    {
        StringBuilder sb = new StringBuilder(title + "\r\n");
        var props = t.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var p in props)
        {
            sb.AppendFormat("{0} ({1}) = {2}\r\n", p.Name, p.GetType().ToString(), p.GetValue(t, null));
        }
        Debug.Log(sb.ToString());
    }

    /// <summary>
    /// Write out the tree structure for the specified transform
    /// </summary>
    public static void LogTransform(Transform t, string title)
    {
        StringBuilder sb = new StringBuilder(title + "\r\n");
        TweakHelpers.LogTransform(t, 0, ref sb);
        Debug.Log(sb.ToString());
    }

    private static void LogTransform(Transform t, int indent, ref StringBuilder sb)
    {
        Component[] all = t.gameObject.GetComponents<Component>();
        for (int i = 0; i < all.Length; i++)
        {
            string s = new string(' ', indent);
            sb.AppendLine(s + all[i].ToString());
        }

        indent++;
        for (int i = 0; i < t.childCount; i++)
        {
            LogTransform(t.GetChild(i), indent, ref sb);
        }
    }
}