using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    private BindingFlags flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;


    protected void BindItem()
    {
        Type type = GetType();
        PropertyInfo[] props = type.GetProperties(flag);

        foreach (var prop in props)
        {
            var propAttrs = prop.GetCustomAttributes(true);
            foreach (var attr in propAttrs)
            {
                if (attr is UICompBind)
                {
                    BindComp(prop, type, attr);
                }
                else if (attr is UIDataBind)
                {
                    BindData(prop, type, attr);
                }
                else
                {
                    BindAction(prop, type, attr);
                }
            }
        }
    }

    private void BindComp(PropertyInfo prop, Type type, object attr)
    {

    }

    private void BindData(PropertyInfo prop, Type type, object attr)
    {
        GComponent main = (GComponent)type.GetField("main", flag).GetValue(this);

        UIDataBind uiBind = (UIDataBind)attr;

        var feildInfo = prop.PropertyType.GetField("_onValueChange", flag);

        var value = prop.GetValue(this);
        if (value == null)
        {
            if (prop.PropertyType.Equals(typeof(UIProp)))
            {
                value = new UIProp();
            }
            else
            {
                Type genericType = typeof(UIListProp<>).MakeGenericType(prop.PropertyType.GenericTypeArguments);
                value = Activator.CreateInstance(genericType);
            }
            prop.SetValue(this, value);
        }

        switch (uiBind._type)
        {
            case "TextField":
                GTextField textField = FguiUtils.GetUI<GTextField>(main, uiBind._path);
                Action<string> actionText = (string data) =>
                {
                    if (textField != null)
                    {
                        textField.text = data;
                    }
                };
                feildInfo.SetValue(value, actionText);
                break;
            case "TextInput":
                GTextInput textInput = FguiUtils.GetUI<GTextInput>(main, uiBind._path);
                Action<string> actionInput = (string data) =>
                {
                    if (textInput != null)
                    {
                        textInput.text = data;
                    }
                };
                feildInfo.SetValue(value, actionInput);
                break;
            case "Image":
                GImage image = FguiUtils.GetUI<GImage>(main, uiBind._path);
                Action<string> actionImage = (string data) =>
                {
                    if (image != null)
                    {
                        image.icon = data;
                    }
                };
                feildInfo.SetValue(value, actionImage);
                break;
            case "Loader":
                GLoader loader = FguiUtils.GetUI<GLoader>(main, uiBind._path);
                Action<string> actionLoader = (string data) =>
                {
                    if (loader != null)
                    {
                        loader.url = data;
                    }
                };
                feildInfo.SetValue(value, actionLoader);
                break;
            case "List":
                GList list = FguiUtils.GetUI<GList>(main, uiBind._path);
                Action<int> actionList = (data) =>
                {
                    if (list != null)
                    {
                        list.numItems = data;

                        if (uiBind._extra.Length > 0)
                        {
                            var renderer = type.GetMethod(uiBind._extra[0], flag);

                            var listItemRendererDelegate = Delegate.CreateDelegate(typeof(ListItemRenderer), null, renderer);

                            list.itemRenderer = (ListItemRenderer)listItemRendererDelegate;
                        }

                        if (uiBind._extra.Length > 1)
                        {
                            var provider = type.GetMethod(uiBind._extra[1], flag);

                            var listItemProviderDelegate = Delegate.CreateDelegate(typeof(ListItemProvider), null, provider);

                            list.itemProvider = (ListItemProvider)listItemProviderDelegate;
                        }

                        if (uiBind._extra.Length > 2)
                        {
                            switch (uiBind._extra[2])
                            {
                                case "height":
                                    list.height = data * list.GetChildAt(0).height + list.lineGap * (data - 1) + list.margin.top + list.margin.bottom;
                                    break;
                                case "width":
                                    list.width = data * list.GetChildAt(0).width + list.columnGap * (data - 1) + list.margin.left + list.margin.right;
                                    break;
                            }
                        }
                    }
                };
                feildInfo.SetValue(value, actionList);
                break;
        }
    }

    private void BindAction(PropertyInfo prop, Type type, object attr)
    {

    }
}
