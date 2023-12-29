using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class ConfigViewer : EditorWindow
{
    static string _folder;
    static string _name;
    ConfigViewer()
    {
        titleContent = new GUIContent("ÅäÖÃ²é¿´Æ÷");
    }

    public static void Show(string folder, string name)
    {
        _folder = folder;
        _name = name;
        GetWindow(typeof(ConfigViewer));
    }

    private void OnEnable()
    {
        ConfigGraph graph = new ConfigGraph()
        {
            style = { flexGrow = 1 }
        };
        rootVisualElement.Add(graph);

        graph.Init(_folder, _name);
    }
}
