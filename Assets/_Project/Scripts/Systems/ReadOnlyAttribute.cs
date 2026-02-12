using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// ────────────────────────────────────────────────
// Put NOTHING between the using statements and the attribute
// ────────────────────────────────────────────────

public class ReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif
