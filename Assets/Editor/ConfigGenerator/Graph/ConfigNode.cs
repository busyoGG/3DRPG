using LitJson;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ConfigNode : Node, ISelectable
{
    public VisualElement _leftContainer;
    public VisualElement _middleContainer;
    public VisualElement _rightContainer;
    public VisualElement _portsAndContentContainer;
    public ConfigNode(JsonData item)
    {
        title = "配置节点";
        //固定宽高
        this.style.width = 200;
        this.style.height = 400;

        // 创建一个容器来包装输入端口、中间内容和输出端口
        _portsAndContentContainer = new VisualElement();
        _portsAndContentContainer.style.flexDirection = FlexDirection.Row;

        // 创建一个左边的内容
        _leftContainer = new VisualElement();
        _leftContainer.style.backgroundColor = new StyleColor(new Color(0.6f, 0.6f, 0.6f, 0.5f));
        _portsAndContentContainer.Add(_leftContainer);
        // 创建一个中间的内容
        _middleContainer = new VisualElement();
        _portsAndContentContainer.Add(_middleContainer);
        // 创建一个右边的内容
        _rightContainer = new VisualElement();
        _portsAndContentContainer.Add(_rightContainer);
        _rightContainer.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f, 0.5f));

        // 创建输入端口
        //Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Node));
        Port inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Edge));
        inputPort.portName = "P";
        _leftContainer.Add(inputPort);

        // 创建输出端口
        //Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Node));
        Port outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Edge));
        outputPort.portName = "N";
        _rightContainer.Add(outputPort);

        // 添加包装容器到主容器
        mainContainer[1].Add(_portsAndContentContainer);

        // 设置输入端口的位置
        inputPort.style.alignSelf = Align.Center;

        // 设置输出端口的位置
        outputPort.style.alignSelf = Align.Center;


        //隐藏
        inputContainer.visible = false;
        outputContainer.visible = false;

        Init(item);

        RefreshPorts();
        RefreshExpandedState();

        // 注册在构造完成后调用的回调
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        mainContainer[1].style.flexGrow = 1;
        mainContainer.style.flexDirection = FlexDirection.Column;
        _portsAndContentContainer.style.height = 400 - titleContainer.resolvedStyle.height;
        //ConsoleUtils.Log("高度", mainContainer[1].resolvedStyle.height);
        // 设置左边内容的样式
        _leftContainer.style.flexGrow = 1; // 占满剩余的空间
        _leftContainer.style.alignItems = Align.FlexStart;
        _leftContainer.style.justifyContent = Justify.Center;

        // 设置中间内容的样式
        _middleContainer.style.flexGrow = 1; // 占满剩余的空间
        _middleContainer.style.alignItems = Align.FlexStart;
        _middleContainer.style.justifyContent = Justify.FlexStart;

        // 设置右边内容的样式
        _rightContainer.style.flexGrow = 1; // 占满剩余的空间
        _rightContainer.style.alignItems = Align.FlexStart;
        _rightContainer.style.justifyContent = Justify.Center;

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    public void Init(JsonData item)
    {
        foreach (var key in item.Keys)
        {
            JsonType type = item[key].GetJsonType();
            string extra = string.Empty;
            if (type.Equals(JsonType.Array))
            {
                extra = "[ ";
                for (int i = 0; i < item[key].Count; i++)
                {
                    extra += item[key][i].ToString() + (i == item[key].Count - 1 ? string.Empty : " , ");
                }

                extra += " ]";
            }
            else if (type.Equals(JsonType.Object))
            {
                extra = "{ ";
                int i = 0;
                foreach(var prop in item[key].Keys)
                {
                    extra += prop + ":" + item[key][prop] + (i == item[key].Count - 1 ? string.Empty : " , ") ;
                    i++;
                }
                extra += " }";
            }
            else
            {
                extra = item[key].ToString();
            }
            Label content = new Label(key + " : " + extra);
            content.style.fontSize = 15;
            content.style.marginTop = 3;
            _middleContainer.Add(content);
        }
    }
}
