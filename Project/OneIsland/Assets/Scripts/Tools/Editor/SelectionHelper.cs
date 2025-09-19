using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SelectionHelper
{
    [InitializeOnLoadMethod]
    private static void Start()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
    }

    private static void ProjectWindowItemOnGUI(string guid, Rect selectionRect)
    {
        if (Event.current.type == EventType.KeyDown &&  Event.current.keyCode == KeyCode.Space
            && selectionRect.Contains(Event.current.mousePosition))
        {
            string strPath = AssetDatabase.GUIDToAssetPath(guid);
            if (Event.current.alt)
            {
                UnityEngine.Debug.Log(strPath);
                Event.current.Use();
                return;
            }
            if (Path.GetExtension(strPath) == string.Empty)
            {
                Process.Start(Path.GetFullPath(strPath));
            }
            else
            {
                Process.Start("explorer.exe", "/select," + Path.GetFullPath(strPath));
            }
            Event.current.Use();
        }
    }

    private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        Event e = Event.current;
        if (EventType.KeyDown == e.type)
        {
            switch (e.keyCode)
            {
                case KeyCode.Space:
                    ChangeGameobjectActiveself();
                    e.Use();
                    break;
            }
        }
    }

    private static void ChangeGameobjectActiveself()
    {
        Undo.RecordObjects(Selection.gameObjects, "Active");
        foreach (var go in Selection.gameObjects)
        {
            go.SetActive(!go.activeSelf);
        }
    }
}
