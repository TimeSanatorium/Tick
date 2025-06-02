#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
public class NamespaceAdder : EditorWindow
{
    private string folderPath = "Assets/Runtime/"; // 修改为你的插件文件夹路径
    private string targetNamespace = "要添加的命名空间"; // 修改为你想添加的命名空间

    [MenuItem("Tools/NameSpaceTool/Add")]
    public static void ShowWindow()
    {
        GetWindow<NamespaceAdder>("批量添加命名空间");
    }

    private void OnGUI()
    {
        GUILayout.Label("批量添加命名空间 【此工具在设置完成后中文可能会出现乱码 使用前请进行备份！！！！】", EditorStyles.boldLabel);

        folderPath = EditorGUILayout.TextField("目标文件夹路径", folderPath);
        targetNamespace = EditorGUILayout.TextField("命名空间", targetNamespace);

        if (GUILayout.Button("添加命名空间"))
        {
            if (!Directory.Exists(folderPath))
            {
                Debug.LogError("指定文件夹不存在: " + folderPath);
                return;
            }

            string[] files = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);
            int modifiedCount = 0;

            foreach (string file in files)
            {
                string code = File.ReadAllText(file,System.Text.Encoding.UTF8);

                // 跳过已经有命名空间的文件
                if (Regex.IsMatch(code, @"namespace\s+\w"))
                    continue;

                // 找到第一个类/结构/接口定义
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
            Debug.Log($"处理完成，共添加命名空间到 {modifiedCount} 个脚本。");
        }
    }

    private static string InsertNameSpace(string code, string addnamespace)
    {
        string[] lines = code.Split('\n');
        List<string> newCodes = new List<string>();
        bool isAdd = false;
        for (int i = 0; i < lines.Length; i++)
        {
            // 跳过 using 语句或空行
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