using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Lever))]
public class LeverEditor : Editor
{
    private SerializedProperty typeProp;
    private SerializedProperty leverUIProp;
    private SerializedProperty controlledDoorProp;
    private SerializedProperty doorListProp;

    private void OnEnable()
    {
        // Cache references to serialized properties
        typeProp = serializedObject.FindProperty("type");
        leverUIProp = serializedObject.FindProperty("leverUI");
        controlledDoorProp = serializedObject.FindProperty("controlledDoor");
        doorListProp = serializedObject.FindProperty("doorList");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Display the type and lever UI fields
        EditorGUILayout.PropertyField(typeProp);
        EditorGUILayout.PropertyField(leverUIProp);

        // Show only the relevant door fields based on LeverType
        LeverType leverType = (LeverType)typeProp.enumValueIndex;
        if (leverType == LeverType.OneDoor)
        {
            EditorGUILayout.PropertyField(controlledDoorProp, new GUIContent("Controlled Door"));
        }
        else if (leverType == LeverType.Multiple)
        {
            EditorGUILayout.PropertyField(doorListProp, new GUIContent("Door List"), true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
