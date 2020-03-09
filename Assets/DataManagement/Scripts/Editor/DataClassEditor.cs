using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DataManagement
{
    /// <copyright file="DataClassEditor.cs">
    /// Copyright (c) 2019 All Rights Reserved
    /// </copyright>
    /// <author>Kevin Hummel</author>
    /// <date>18/03/2019 21:41 PM </date>
    /// <summary>
    /// This class handles the easy creation of data classes trough the use of a custom EditorWindow.
    /// </summary>
    public class DataClassEditor : EditorWindow
    {
        private string _path = "";
        private string _addClass = "Create class";

        private DefaultAsset _targetFolder = null;

        [MenuItem("DataManagement/Create Data-classes")]
        static void Init()
        {
            DataClassEditor window =
                (DataClassEditor)GetWindow(typeof(DataClassEditor));
        }

        [Serializable]
        public class ClassReferences
        {
            public string Name;
            public string ID;
            public PropertyReferences[] Properties;

            [Serializable]
            public class PropertyReferences
            {
                public string Name;
                public enum Types { Int, String, Float, Sprite, Vector3, List }
                public Types Type;
            }
        }
        public ClassReferences classReferences;
        private void ClassHandler()
        {
            SerializedObject t_object = new SerializedObject(this);
            SerializedProperty t_property = t_object.FindProperty("classReferences");

            EditorGUILayout.PropertyField(t_property, true);
            t_object.ApplyModifiedProperties();
        }

        private enum OutputType { Main, Data, Initialize }
        private string PropertyHandler(OutputType p_input)
        {
            if (p_input == OutputType.Initialize)
            {
                List<string> t_temp = new List<string>();
                for (int i = 0; i < classReferences.Properties.Length; i++)
                {
                    string t_data = "\t\tLoad" + char.ToUpper(classReferences.Properties[i].Name[0]) + classReferences.Properties[i].Name.Substring(1) + "();\n";

                    t_temp.Add(t_data);
                }
                return string.Join("", t_temp.ToArray());
            }
            if (p_input == OutputType.Main)
            {
                List<string> t_temp = new List<string>();
                for (int i = 0; i < classReferences.Properties.Length; i++)
                {
                    string t_data = "\tpublic void Save" + char.ToUpper(classReferences.Properties[i].Name[0]) + classReferences.Properties[i].Name.Substring(1) + "()\n" + "\t{\n" + "\t\t //TODO IMPLEMENT: Example, _data." + char.ToUpper(classReferences.Properties[i].Name[0]) + classReferences.Properties[i].Name.Substring(1) + " = " + char.ToUpper(classReferences.Properties[i].Name[0]) + classReferences.Properties[i].Name.Substring(1) + ";\n" + "\t\t_data.Save()\n;" + "\t}\n" +
                                    "\tpublic void Load" + char.ToUpper(classReferences.Properties[i].Name[0]) + classReferences.Properties[i].Name.Substring(1) + "()\n" + "\t{\n" + "\t\t //TODO IMPLEMENT: Example, " + char.ToUpper(classReferences.Properties[i].Name[0]) + classReferences.Properties[i].Name.Substring(1) + " = _data." + char.ToUpper(classReferences.Properties[i].Name[0]) + classReferences.Properties[i].Name.Substring(1) + ";\n" + "\t}\n\n";

                    t_temp.Add(t_data);
                }
                return string.Join("", t_temp.ToArray());
            }
            if (p_input == OutputType.Data)
            {
                List<string> t_temp = new List<string>();
                for (int i = 0; i < classReferences.Properties.Length; i++)
                {
                    string t_data = "\tpublic " + TypeCheck(classReferences.Properties[i].Type.ToString()) + " " + char.ToUpper(classReferences.Properties[i].Name[0]) + classReferences.Properties[i].Name.Substring(1) + " { get => _" + classReferences.Properties[i].Name.ToLower() + "; set => _" + classReferences.Properties[i].Name.ToLower() + " = value; }\n" +
                                    "\t[SerializeField] private " + TypeCheck(classReferences.Properties[i].Type.ToString()) + " _" + classReferences.Properties[i].Name.ToLower() + ";\n";

                    t_temp.Add(t_data);
                }
                return string.Join("", t_temp.ToArray()); ;
            }
            return null;
        }

        private string TypeCheck(string p_input)
        {
            if (p_input.Contains("Int")) return p_input.ToLower();
            if (p_input.Contains("String")) return p_input.ToLower();
            if (p_input.Contains("Float")) return p_input.ToLower();
            if (p_input.Contains("List")) return p_input + "<T>";

            return p_input;
        }

        void OnGUI()
        {
            _targetFolder = (DefaultAsset)EditorGUILayout.ObjectField("Folder", _targetFolder, typeof(DefaultAsset), false);

            if (_targetFolder != null)
            {
                EditorGUILayout.HelpBox("Valid folder!", MessageType.Info, true);
            }
            else EditorGUILayout.HelpBox("Not valid!", MessageType.Warning, true);

            _path = Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(_targetFolder);

            EditorGUILayout.Space();

            ClassHandler();

            if (GUILayout.Button(_addClass))
            {
                CreateClass(classReferences.Name);
            }
        }

        private void CreateClass(string p_input)
        {
            if (p_input.Length > 0 && File.Exists(_path + "/" + p_input + ".cs") == false && _targetFolder != null)
            {
                using (StreamWriter t_dataclass = new StreamWriter(_path + "/" + p_input + "Data.cs"))
                {
                    string t_input = ("using UnityEngine;\n") +
                                    ("using DataManagement;\n") +
                                    ("\n") +
                                    ("public class " + p_input + "Data" + " : DataElement {\n") +
                                    ("\n") +
                                    (PropertyHandler(OutputType.Data)) +
                                    ("\n") +
                                    ("\tpublic " + p_input + "Data" + "(string p_id) : base(p_id)\n") +
                                    ("\t{\n") +
                                    ("\t\tID = p_id;\n") +
                                    ("\t}\n") +
                                    ("}");

                    t_dataclass.Write(t_input);

                    File.Create(t_dataclass.ToString()).Dispose();
                }

                using (StreamWriter t_class = new StreamWriter(_path + "/" + p_input + ".cs"))
                {
                    string t_input = ("using UnityEngine;\n") +
                                    ("using DataManagement;\n") +
                                    ("using System.Linq;\n") +
                                    ("\n") +
                                    ("#if UNITY_EDITOR\n") +
                                    ("using UnityEditor.SceneManagement;\n") +
                                    ("#endif\n") +
                                    ("\n") +
                                    ("public class " + p_input + " : MonoBehaviour {\n") +
                                    ("\n") +
                                    ("\t#region Save implementation\n") +
                                    ("\t// A reference too all currently saved data.\n\tprivate DataReferences _dataReferences = null;\n") +
                                    ("\n") +
                                    ("\t// A reference too the data being saved in this class.\n\tprivate " + p_input + "Data" + " _data = null;\n") +
                                    ("\n") +
                                    ("\t// The ID under wich this data will be saved.\n\t[SerializeField] private string _id = " + '"' + classReferences.ID + '"' + ";\n") +
                                    ("\n") +
                                    ("\t[ContextMenu(" + '"' + "Generate ID" + '"' + ")]\n") +
                                    ("\tpublic void GenerateID()\n") +
                                    ("\t{\n") +
                                    ("\t\t#if UNITY_EDITOR\n") +
                                    ("\t\tEditorSceneManager.MarkSceneDirty(gameObject.scene);\n") +
                                    ("\t\t#endif\n") +
                                    ("\n") +
                                    ("\t\t_id = " + '"' + '"' + ";\n") +
                                    ("\t\tSystem.Random t_random = new System.Random();\n") +
                                    ("\t\tconst string t_chars = " + '"' + "AaBbCcDdErFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789" + '"' + ";\n") +
                                    ("\t\t_id = new string( Enumerable.Repeat(t_chars, 8).Select(s => s[t_random.Next(s.Length)]).ToArray());\n") +
                                    ("\t}\n") +
                                    ("\n") +
                                    ("\tprivate void Setup()\n") +
                                    ("\t{\n") +
                                    ("\t\t_dataReferences = SceneManager.Instance.DataReferences;\n") +
                                    ("\n") +
                                    ("\t\t_data = _dataReferences.FindElement<" + p_input + "Data" + ">(_id);\n") +
                                    ("\t\tif (_data == null) {\n") +
                                    ("\t\t\t_data = _dataReferences.AddElement<" + p_input + "Data" + ">(_id);\n") +
                                    ("\t\t\treturn;\n") +
                                    ("\t\t}\n") +
                                    ("\n") +
                                    (PropertyHandler(OutputType.Initialize)) +
                                    ("\t}\n") +
                                    ("\n") +
                                    (PropertyHandler(OutputType.Main)) +
                                    ("\n") +
                                    ("\t#endregion\n") +
                                    ("\n") +
                                    ("\tvoid Start()\n") +
                                    ("\t{\n") +
                                    ("\t\tSetup();\n") +
                                    ("\t}\n") +
                                    ("}");

                    t_class.Write(t_input);

                    File.CreateText(t_class.ToString()).Dispose();
                }

                string[] lines = File.ReadAllLines(Application.dataPath + "/DataManagement/Scripts/Managers/DataManager.cs");
                string allString = "";

                lines[70] = "\n\t\t    DataBuilder.BuildElementsOfType<" + p_input + "Data" + ">(t_sceneManager.DataReferences.SaveData);";

                for (int i = 0; i < lines.Length; i++)
                {
                    allString += lines[i] + "\n";
                }

                File.WriteAllText(Application.dataPath + "/DataManagement/Scripts/Managers/DataManager.cs", allString);

                AssetDatabase.Refresh();
            }
        }
    }
}