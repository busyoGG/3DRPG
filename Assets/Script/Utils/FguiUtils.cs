using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class FguiUtils
{
    /// <summary>
    /// 根据路径获取UI组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="comp"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T GetUI<T>(GComponent comp, string path) where T : GObject
    {
        string[] paths = path.Split('/');
        GObject res = null;
        GComponent parent = comp;
        foreach (string s in paths)
        {
            if (s == "") continue;
            int output;
            bool isNumeric = int.TryParse(s, out output);
            if (isNumeric)
            {
                res = parent.GetChildAt(output);
            }
            else
            {
                res = parent.GetChild(s);
            }
            if (res == null)
            {
                ConsoleUtils.Error("ui路径错误", path);
                return null;
            }
            if (res is GComponent)
            {
                parent = res.asCom;
            }
            else
            {
                break;
            }
        }
        return res as T;
    }
}
