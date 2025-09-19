using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(UINameTable))]
public class UINameTableEditor : Editor
{
    private SerializedProperty nodeList;
    private ReorderableList nodeListRecord;
    private HashSet<int> duplicateIndexs = new HashSet<int>();
    private Object duplicatedObject = null;
    private Dictionary<string, int> checkTable = new Dictionary<string, int>(StringComparer.Ordinal);
    private Dictionary<Object, int> checkGoDuplicateTable = new Dictionary<Object, int>();
    private GameObject newObject = null;
    private static GameObject searchObject = null;
    private string searchText;
    private UINameTable.StringGameObjectPair[] searchResult;

    private void OnEnable()
    {
        if (base.target != null)
        {
            SerializedObject serObj = base.serializedObject;
            this.nodeList = serObj.FindProperty("nodeList");
            this.nodeListRecord = new ReorderableList(serObj, this.nodeList);
            this.nodeListRecord.drawHeaderCallback = delegate (Rect rect)
            {
                this.DrawNodeListHeader(rect);
            };
            this.nodeListRecord.elementHeight = EditorGUIUtility.singleLineHeight;
            this.nodeListRecord.drawElementCallback = delegate (Rect rect, int index, bool selected, bool focused)
            {
                this.DrawNodeList(this.nodeList, rect, index, selected, focused);
            };
        }
    }

    private void DrawNodeListHeader(Rect rect)
    {
        Rect rectLeft = new Rect(rect.x + 13f, rect.y, rect.width / 2f, EditorGUIUtility.singleLineHeight);
        Rect rectRight = new Rect(rect.x + 10f + rect.width / 2f, rect.y, rect.width / 2f, EditorGUIUtility.singleLineHeight);
        GUI.Label(rectLeft, "Name");
        GUI.Label(rectRight, "Object");
    }

    private void DrawNodeList(SerializedProperty property, Rect rect, int index, bool selected, bool focused)
    {
        SerializedProperty element = property.GetArrayElementAtIndex(index);
        bool dupilicate = this.duplicateIndexs.Contains(index);
        Color colorKeeper = GUI.color;
        if (dupilicate)
        {
            GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
        }
        SerializedProperty key = element.FindPropertyRelative("key");
        SerializedProperty value = element.FindPropertyRelative("value");
        if (value.objectReferenceValue == this.duplicatedObject)
        {
            GUI.color = new Color(0.2f, 1f, 1f, 1f);
        }
        else
        {
            if (value.objectReferenceValue == this.newObject || value.objectReferenceValue == UINameTableEditor.searchObject)
            {
                GUI.color = new Color(0.5f, 1f, 0.5f, 1f);
            }
        }
        Rect rectLeft = new Rect(rect.x, rect.y, rect.width / 2f - 5f, EditorGUIUtility.singleLineHeight);
        Rect rectRight = new Rect(rect.x + rect.width / 2f + 5f, rect.y, rect.width / 2f - 5f, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(rectLeft, key, GUIContent.none);
        EditorGUI.PropertyField(rectRight, value, GUIContent.none);
        GUI.color = colorKeeper;
    }

    public override void OnInspectorGUI()
    {
        base.serializedObject.Update();
        this.nodeListRecord.DoLayoutList();

        if (base.serializedObject.ApplyModifiedProperties())
        {
            this.FindDuplicate();
        }
        UINameTable nameTable = (UINameTable)base.target;

        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        this.newObject = (EditorGUILayout.ObjectField(this.newObject, typeof(GameObject), true, Array.Empty<GUILayoutOption>()) as GameObject);
        if (GUILayout.Button("Add", Array.Empty<GUILayoutOption>()))
        {
            if (this.newObject == null)
                return;

            this.duplicatedObject = null;
            if (this.FindDuplicate(this.newObject))
            {
                Debug.LogError("duplicated object");
                return;
            }

            string key = this.newObject.name;
            int idx = 0;
            for (; ; )
            {
                if (!nameTable.Find(key))
                {
                    break;
                }
                key += idx.ToString();
                idx++;
            }
            Undo.RecordObject(nameTable, "add to Name Table");
            base.serializedObject.Update();
            nameTable.Add(key, this.newObject);
            base.serializedObject.ApplyModifiedProperties();
        }
        GUILayout.EndHorizontal();

        string newSearch = EditorGUILayout.TextField("Search:", this.searchText, Array.Empty<GUILayoutOption>());
        if (string.IsNullOrEmpty(newSearch))
        {
            this.searchText = null;
            this.searchResult = null;
        }
        else
        {
            if (newSearch != this.searchText)
            {
                this.searchText = newSearch;
                this.searchResult = nameTable.Search(this.searchText);
            }
        }
        if (this.searchResult != null)
        {
            GUI.enabled = false;
            GUILayout.BeginVertical();
            foreach (UINameTable.StringGameObjectPair item in this.searchResult)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(item.key);
                EditorGUILayout.ObjectField(item.value, item.value.GetType(), true);
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            GUI.enabled = true;
        }
    }

    private bool FindDuplicate(Object go)
    {
        for (int i = 0; i < this.nodeList.arraySize; i++)
        {
            SerializedProperty element = this.nodeList.GetArrayElementAtIndex(i);
            SerializedProperty value = element.FindPropertyRelative("value");
            if (value.objectReferenceValue == go)
            {
                this.duplicatedObject = go;
                return true;
            }
        }
        return false;
    }

    private void FindDuplicate()
    {
        this.duplicateIndexs.Clear();
        this.checkTable.Clear();
        this.checkGoDuplicateTable.Clear();
        for (int i = 0; i < this.nodeList.arraySize; i++)
        {
            SerializedProperty element = this.nodeList.GetArrayElementAtIndex(i);
            SerializedProperty key = element.FindPropertyRelative("key");
            SerializedProperty value = element.FindPropertyRelative("value");
            Object obj = value.objectReferenceValue;
            if (this.checkTable.ContainsKey(key.stringValue))
            {
                this.duplicateIndexs.Add(this.checkTable[key.stringValue]);
                this.duplicateIndexs.Add(i);
            }
            else
            {
                if (obj != null && this.checkGoDuplicateTable.ContainsKey(obj))
                {
                    this.duplicateIndexs.Add(this.checkTable[key.stringValue]);
                    this.duplicateIndexs.Add(i);
                }
                else
                {
                    this.checkTable.Add(key.stringValue, i);
                    if (obj != null)
                    {
                        this.checkGoDuplicateTable.Add(obj, i);
                    }
                }
            }
        }
    }
}
