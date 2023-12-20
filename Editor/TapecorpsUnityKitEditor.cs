using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapeCorps.Runtime;
using TapeCorps.Customs;
using System;
using System.Reflection;
using Object = UnityEngine.Object;



#if UNITY_EDITOR
using UnityEditor;

namespace TapeCorps
{

    namespace Editor
    {
        public static class TapecorpsUnityKitEditor
        {
            public static T[] GetAllInstances<T>() where T : ScriptableObject
            {
                string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
                T[] a = new T[guids.Length];
                for (int i = 0; i < guids.Length; i++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                    a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
                }

                return a;
            }
        }
    }

    namespace Inspector
    {
        public static class FieldPresets
        {
            public static void Field(string label, ref string obj, params GUILayoutOption[] layoutOptions)
            {
                obj = EditorGUILayout.TextField(label, obj, layoutOptions);
            }

            public static void TextArea(string label, ref string obj, params GUILayoutOption[] layoutOptions)
            {
                GUILayout.Label(label);
                obj = EditorGUILayout.TextArea(obj, layoutOptions);
            }

            public static void Field(string label, ref int obj, params GUILayoutOption[] layoutOptions)
            {
                obj = EditorGUILayout.IntField(label, obj, layoutOptions);
            }
            public static void Field(string label, ref int obj, int leftValue, int rightValue, params GUILayoutOption[] layoutOptions)
            {
                obj = EditorGUILayout.IntSlider(label, obj, leftValue, rightValue, layoutOptions);
            }
            public static void Field(string label, ref float obj, params GUILayoutOption[] layoutOptions)
            {
                obj = EditorGUILayout.FloatField(label, obj, layoutOptions);
            }
            public static void Field(string label, ref float obj, float leftValue, float rightValue, params GUILayoutOption[] layoutOptions)
            {
                obj = EditorGUILayout.Slider(label, obj, leftValue, rightValue, layoutOptions);
            }
            public static void Field(string label, ref bool obj, params GUILayoutOption[] layoutOptions)
            {
                if (GUILayout.Button($"{label}: {obj.ToString()}", layoutOptions))
                    obj = !obj;
            }
            public static void Field(string label, ref Vector3 obj, params GUILayoutOption[] layoutOptions)
            {
                obj = EditorGUILayout.Vector3Field(label, obj, layoutOptions);
            }
            public static void Field(string label, ref Vector3Int obj, params GUILayoutOption[] layoutOptions)
            {
                obj = EditorGUILayout.Vector3IntField(label, obj, layoutOptions);
            }
            public static void Field(string label, ref Vector2 obj, params GUILayoutOption[] layoutOptions)
            {
                obj = EditorGUILayout.Vector2Field(label, obj, layoutOptions);
            }
            public static void Field(string label, ref Vector2Int obj, params GUILayoutOption[] layoutOptions)
            {
                obj = EditorGUILayout.Vector2IntField(label, obj, layoutOptions);
            }
            public static void Field(string label, ref Object obj, Type type, bool allowSceneObjects = false, params GUILayoutOption[] layoutOptions)
            {
                obj = EditorGUILayout.ObjectField(label, obj, type, allowSceneObjects, layoutOptions);
            }
            public static void Field(string label, ref CheckVector3 obj, params GUILayoutOption[] layoutOptions)
            {
                GUILayout.BeginHorizontal(layoutOptions);
                obj.Checked = EditorGUILayout.ToggleLeft("", obj.Checked, GUILayout.Width(15));
                EditorGUI.BeginDisabledGroup(!obj.Checked);
                obj.Value = EditorGUILayout.Vector3Field(label, obj.Value);
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();
            }
            public static void Field(string label, ref CheckVector2 obj, params GUILayoutOption[] layoutOptions)
            {
                GUILayout.BeginHorizontal(layoutOptions);
                obj.Checked = EditorGUILayout.ToggleLeft("", obj.Checked, GUILayout.Width(15));
                EditorGUI.BeginDisabledGroup(!obj.Checked);
                obj.Value = EditorGUILayout.Vector2Field(label, obj.Value);
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();
            }
            public static void Field(string label, ref CheckVector2Int obj, params GUILayoutOption[] layoutOptions)
            {
                GUILayout.BeginHorizontal(layoutOptions);
                obj.Checked = EditorGUILayout.ToggleLeft("", obj.Checked, GUILayout.Width(15));
                EditorGUI.BeginDisabledGroup(!obj.Checked);
                obj.Value = EditorGUILayout.Vector2IntField(label, obj.Value);
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();
            }
            public static void Field(string label, ref CheckString obj)
            {
                GUILayout.BeginHorizontal();
                obj.Checked = EditorGUILayout.ToggleLeft("", obj.Checked, GUILayout.Width(15));
                EditorGUI.BeginDisabledGroup(!obj.Checked);
                obj.Value = EditorGUILayout.TextField(label, obj.Value);
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();
            }
            public static void Field(string label, ref CheckInt obj)
            {
                GUILayout.BeginHorizontal();
                obj.Checked = EditorGUILayout.ToggleLeft("", obj.Checked, GUILayout.Width(15));
                EditorGUI.BeginDisabledGroup(!obj.Checked);
                obj.Value = EditorGUILayout.IntField(label, obj.Value);
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();
            }

            public static void Field(string label, ref Chance obj)
            {
                GUILayout.BeginHorizontal();
                obj.Value = EditorGUILayout.Slider(label, obj.Value, 0f, 100f);
                GUILayout.Label("%", GUILayout.Width(15));
                if (GUILayout.Button("Roll", GUILayout.Width(32)))
                    Debug.Log($"Rolled: {obj.Roll}");
                GUILayout.EndHorizontal();
            }

            public static void Field(string label, ref Object obj)
            {
                obj = EditorGUILayout.ObjectField(label, obj, obj.GetType(), false);
            }

            public static void Field(string label, ref Sprite obj)
            {
                obj = (Sprite)EditorGUILayout.ObjectField(label, obj, typeof(Sprite), false);
            }


            public static void Field(string label, ref GameObject obj)
            {
                obj = (GameObject)EditorGUILayout.ObjectField(label, obj, typeof(GameObject), false);
            }

            public static void Toggle(string label, ref bool obj)
            {
                obj = EditorGUILayout.Toggle(label, obj);
            }

            public static void ToggleLeft(string label, ref bool obj)
            {
                obj = EditorGUILayout.ToggleLeft(label, obj);
            }

            public static void Foldout(string label, ref bool obj)
            {
                obj = EditorGUILayout.Foldout(obj, label, true);
            }

            public static void EditorFoldout(string label, ref bool obj)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                GUILayout.Space(15);
                obj = EditorGUILayout.Foldout(obj, label, true);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}


#endif