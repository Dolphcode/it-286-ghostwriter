using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ButtonAction))]
[CanEditMultipleObjects]
public class ButtonUIEditor : Editor
{
    SerializedProperty buttonAction;

    SerializedProperty targetUI;
    SerializedProperty uiOff1;
    SerializedProperty uiOff2;
    SerializedProperty uiOff3;


    void OnEnable()
    {
        buttonAction = serializedObject.FindProperty("buttonType");
        targetUI = serializedObject.FindProperty("targetUI");
        uiOff1 = serializedObject.FindProperty("uiOff1");
        uiOff2 = serializedObject.FindProperty("uiOff2");
        uiOff3 = serializedObject.FindProperty("uiOff3");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(buttonAction);

        // Show variables related to Computer Buttons
        if (buttonAction.enumValueIndex == 0)
        {
            EditorGUILayout.PropertyField(targetUI);
            EditorGUILayout.PropertyField(uiOff1);
            EditorGUILayout.PropertyField(uiOff2);
            EditorGUILayout.PropertyField(uiOff3);
        }
        else if (buttonAction.enumValueIndex == 1)
        {

        }
        serializedObject.ApplyModifiedProperties();
    }
}
