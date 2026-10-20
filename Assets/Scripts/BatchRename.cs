using UnityEngine;
using UnityEditor;

public class BatchRename : EditorWindow
{
    private string baseName = "NewName";
    private int startIndex = 0;

    [MenuItem("Tools/Batch Rename")]
    public static void ShowWindow()
    {
        GetWindow<BatchRename>("Batch Rename");
    }

    private void OnGUI()
    {
        GUILayout.Label("Batch Rename Selected Objects", EditorStyles.boldLabel);
        baseName = EditorGUILayout.TextField("Base Name", baseName);
        startIndex = EditorGUILayout.IntField("Start Index", startIndex);

        if (GUILayout.Button("Rename Selected"))
        {
            // Get all selected items in Hierarchy or Project view
            Object[] selectedObjects = Selection.objects;

            if (selectedObjects.Length == 0)
            {
                Debug.LogWarning("No objects selected to rename.");
                return;
            }

            // Record undo operation so you can Ctrl+Z if you make a mistake
            Undo.RecordObjects(selectedObjects, "Batch Rename");

            for (int i = 0; i < selectedObjects.Length; i++)
            {
                selectedObjects[i].name = $"{baseName}_{startIndex + i}";
            }
            
            Debug.Log($"Successfully renamed {selectedObjects.Length} objects.");
        }
    }
}
