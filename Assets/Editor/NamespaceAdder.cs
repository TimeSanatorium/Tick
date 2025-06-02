#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
public class NamespaceAdder : EditorWindow
{
    private string folderPath = "Assets/Runtime/"; // �޸�Ϊ��Ĳ���ļ���·��
    private string targetNamespace = "Ҫ��ӵ������ռ�"; // �޸�Ϊ������ӵ������ռ�

    [MenuItem("Tools/NameSpaceTool/Add")]
    public static void ShowWindow()
    {
        GetWindow<NamespaceAdder>("������������ռ�");
    }

    private void OnGUI()
    {
        GUILayout.Label("������������ռ� ���˹�����������ɺ����Ŀ��ܻ�������� ʹ��ǰ����б��ݣ���������", EditorStyles.boldLabel);

        folderPath = EditorGUILayout.TextField("Ŀ���ļ���·��", folderPath);
        targetNamespace = EditorGUILayout.TextField("�����ռ�", targetNamespace);

        if (GUILayout.Button("��������ռ�"))
        {
            if (!Directory.Exists(folderPath))
            {
                Debug.LogError("ָ���ļ��в�����: " + folderPath);
                return;
            }

            string[] files = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);
            int modifiedCount = 0;

            foreach (string file in files)
            {
                string code = File.ReadAllText(file,System.Text.Encoding.UTF8);

                // �����Ѿ��������ռ���ļ�
                if (Regex.IsMatch(code, @"namespace\s+\w"))
                    continue;

                // �ҵ���һ����/�ṹ/�ӿڶ���
                Match match = Regex.Match(code, @"(^|\n)(\s*)(public|internal|private|protected)?\s*(class|struct|interface|enum)\s+\w+");

                if (match.Success)
                {
                    int insertIndex = match.Index;
                    string indent = match.Groups[2].Value;

                    //string wrappedCode = $"namespace {targetNamespace}\n{{\n{InsertNameSpace(code, 1)}\n}}";
                    string wrappedCode = InsertNameSpace(code, targetNamespace) + "\n}";
                    File.WriteAllText(file, wrappedCode);
                    modifiedCount++;
                }
            }

            AssetDatabase.Refresh();
            Debug.Log($"������ɣ�����������ռ䵽 {modifiedCount} ���ű���");
        }
    }

    private static string InsertNameSpace(string code, string addnamespace)
    {
        string[] lines = code.Split('\n');
        List<string> newCodes = new List<string>();
        bool isAdd = false;
        for (int i = 0; i < lines.Length; i++)
        {
            // ���� using �������
            if (!lines[i].TrimStart().StartsWith("using") && !isAdd )
            {

                newCodes.Add("namespace " + addnamespace+ "{");
                isAdd = true;
            }
            newCodes.Add(lines[i]);
        }
        return string.Join("\n", newCodes);
    }
}
#endif