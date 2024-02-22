using FairyGUI;
using System;
using System.Reflection;

public class UIBase
{
    private BindingFlags flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;


    protected void BindItem()
    {
        Type type = GetType();
        PropertyInfo[] props = type.GetProperties(flag);
        GComponent main = (GComponent)type.GetField("main", flag).GetValue(this);

        MethodInfo[] methods = type.GetMethods(flag);

        foreach (var method in methods)
        {
            var methodAttrs = method.GetCustomAttributes(true);
            foreach (var attr in methodAttrs)
            {
                if (attr is UIActionBind)
                {
                    BindAction(method, attr, main);
                }
            }
        }

        foreach (var prop in props)
        {
            var propAttrs = prop.GetCustomAttributes(true);
            foreach (var attr in propAttrs)
            {
                if (attr is UICompBind)
                {
                    BindComp(prop, attr, main);
                }
                else if (attr is UIDataBind)
                {
                    BindData(prop, attr, main);
                }
            }
        }
    }

    private void BindComp(PropertyInfo prop, object attr, GComponent main)
    {
        UICompBind uiBind = (UICompBind)attr;

        switch (uiBind._type)
        {
            case "Comp":
                GComponent comp = FguiUtils.GetUI<GComponent>(main, uiBind._path);
                prop.SetValue(this, comp);
                break;
            case "TextField":
                GTextField textField = FguiUtils.GetUI<GTextField>(main, uiBind._path);
                prop.SetValue(this, textField);
                break;
            case "TextInput":
                GTextInput textInput = FguiUtils.GetUI<GTextInput>(main, uiBind._path);
                prop.SetValue(this, textInput);
                break;
            case "Image":
                GImage image = FguiUtils.GetUI<GImage>(main, uiBind._path);
                prop.SetValue(this, image);
                break;
            case "Loader":
                GLoader loader = FguiUtils.GetUI<GLoader>(main, uiBind._path);
                prop.SetValue(this, loader);
                break;
            case "List":
                GList list = FguiUtils.GetUI<GList>(main, uiBind._path);
                prop.SetValue(this, list);
                break;
        }
    }

    private void BindData(PropertyInfo prop, object attr, GComponent main)
    {
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
                        list.SetVirtual();
                        list.numItems = data;

                        if (uiBind._extra.Length > 0)
                        {
                            switch (uiBind._extra[0])
                            {
                                case "height":
                                    if (list.numChildren > 0)
                                    {
                                        list.height = data * list.GetChildAt(0).height + list.lineGap * (data - 1) + list.margin.top + list.margin.bottom;
                                    }
                                    break;
                                case "width":
                                    if (list.numChildren > 0)
                                    {
                                        list.width = data * list.GetChildAt(0).width + list.columnGap * (data - 1) + list.margin.left + list.margin.right;
                                    }
                                    break;
                            }
                        }
                    }
                };
                feildInfo.SetValue(value, actionList);
                break;
        }
    }

    private void BindAction(MethodInfo method, object attr, GComponent main)
    {
        UIActionBind uiBind = (UIActionBind)attr;
        GObject obj = FguiUtils.GetUI<GObject>(main, uiBind._path);
        switch (uiBind._type)
        {
            case "Click":
                var click = Delegate.CreateDelegate(typeof(EventCallback0), this, method);
                obj.onClick.Set((EventCallback0)click);
                break;
            case "ListRender":
                var render = Delegate.CreateDelegate(typeof(ListItemRenderer), this, method);
                obj.asList.itemRenderer = (ListItemRenderer)render;
                ConsoleUtils.Log("∞Û∂®‰÷»æ");
                break;
            case "ListProvider":
                var provider = Delegate.CreateDelegate(typeof(ListItemProvider), this, method);
                obj.asList.itemProvider = (ListItemProvider)provider;
                break;
            case "ListClick":
                var listClick = Delegate.CreateDelegate(typeof(EventCallback0), this, method);
                obj.asList.onClickItem.Set((EventCallback0)listClick);
                break;
        }
    }
}
