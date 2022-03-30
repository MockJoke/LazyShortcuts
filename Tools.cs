using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MyTools {
    public class Tools : Editor
    {
        [MenuItem("Tools/Shortcuts/Reset Transform &r")]
        static void ResetTransform()
        {
            GameObject[] selection = Selection.gameObjects;
            if (selection.Length < 1) return;
            Undo.RegisterCompleteObjectUndo(selection, "Zero Position");
            foreach (GameObject go in selection)
            {
                InternalZeroPosition(go);
                InternalZeroRotation(go);
                InternalZeroScale(go);
            }
        }
        private static void InternalZeroPosition(GameObject go)
        {
            go.transform.localPosition = Vector3.zero;
        }
        private static void InternalZeroRotation(GameObject go)
        {
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        private static void InternalZeroScale(GameObject go)
        {
            go.transform.localScale = Vector3.one;
        }


        [MenuItem("Tools/Shortcuts/Toggle Lock Inspector &l")]
        static void LockUnlockInspector()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
            ActiveEditorTracker.sharedTracker.activeEditors[0].Repaint();
        }


        [MenuItem("Tools/Shortcuts/Toggle Debug or Normal &d")]
        static void ToggleDebugNormalView()
        {
            EditorWindow inspectorWindow;
            EditorWindow[] editorWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
            foreach (EditorWindow editorWindow in editorWindows)
            {
                if (editorWindow.GetType().Name == "InspectorWindow")
                {
                    if (EditorWindow.focusedWindow == editorWindow)
                    {
                        inspectorWindow = editorWindow;
                        Type inspector = inspectorWindow.GetType();
                        MethodInfo methodInfo = inspector.GetMethod("SetMode", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (inspector.GetField("m_InspectorMode").GetValue(inspectorWindow).ToString() == "Debug")
                        {
                            methodInfo.Invoke(inspectorWindow, new object[] { InspectorMode.Normal });
                        }
                        else methodInfo.Invoke(inspectorWindow, new object[] { InspectorMode.Debug });
                        break;
                    }
                }
            }
        }
        [MenuItem("Tools/Shortcuts/Apply changes to Prefab &a")]
        static void SaveChangesToPrefab()
        {
            GameObject[] selection = Selection.gameObjects;
            if (selection.Length < 1) return;
            Undo.RegisterCompleteObjectUndo(selection, "Apply Prefab");
            foreach (GameObject go in selection)
            {
                if (PrefabUtility.GetPrefabType(go) == PrefabType.PrefabInstance)
                {
                    PrefabUtility.ReplacePrefab(go, PrefabUtility.GetPrefabParent(go));
                    PrefabUtility.RevertPrefabInstance(go);
                }
            }
        }
        [MenuItem("Tools/Shortcuts/Revert Prefab &p")]
        static void RevertPrefab()
        {
            GameObject[] selection = Selection.gameObjects;
            if (selection.Length < 1) return;
            Undo.RegisterCompleteObjectUndo(selection, "Revert Prefab");
            foreach (GameObject go in selection)
            {
                if (PrefabUtility.GetPrefabType(go) == PrefabType.PrefabInstance)
                {
                    PrefabUtility.RevertPrefabInstance(go);
                }
            }
        }

    }
}

