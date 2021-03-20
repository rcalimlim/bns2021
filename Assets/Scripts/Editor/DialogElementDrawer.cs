using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogElement))]
public class DialogElementDrawer : Editor
{
    SerializedProperty text;
    SerializedProperty style;

    private void OnEnable()
    {
        text = serializedObject.FindProperty("text");
        style = serializedObject.FindProperty("style");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.PropertyField(text);
        EditorGUILayout.PropertyField(style);
        serializedObject.ApplyModifiedProperties();
    }
}
