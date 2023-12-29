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
        title = "���ýڵ�";
        //�̶����
        this.style.width = 200;
        this.style.height = 400;

        // ����һ����������װ����˿ڡ��м����ݺ�����˿�
        _portsAndContentContainer = new VisualElement();
        _portsAndContentContainer.style.flexDirection = FlexDirection.Row;

        // ����һ����ߵ�����
        _leftContainer = new VisualElement();
        _leftContainer.style.backgroundColor = new StyleColor(new Color(0.6f, 0.6f, 0.6f, 0.5f));
        _portsAndContentContainer.Add(_leftContainer);
        // ����һ���м������
        _middleContainer = new VisualElement();
        _portsAndContentContainer.Add(_middleContainer);
        // ����һ���ұߵ�����
        _rightContainer = new VisualElement();
        _portsAndContentContainer.Add(_rightContainer);
        _rightContainer.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f, 0.5f));

        // ��������˿�
        //Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Node));
        Port inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Edge));
        inputPort.portName = "P";
        _leftContainer.Add(inputPort);

        // ��������˿�
        //Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Node));
        Port outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Edge));
        outputPort.portName = "N";
        _rightContainer.Add(outputPort);

        // ��Ӱ�װ������������
        mainContainer[1].Add(_portsAndContentContainer);

        // ��������˿ڵ�λ��
        inputPort.style.alignSelf = Align.Center;

        // ��������˿ڵ�λ��
        outputPort.style.alignSelf = Align.Center;


        //����
        inputContainer.visible = false;
        outputContainer.visible = false;

        Init(item);

        RefreshPorts();
        RefreshExpandedState();

        // ע���ڹ�����ɺ���õĻص�
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        mainContainer[1].style.flexGrow = 1;
        mainContainer.style.flexDirection = FlexDirection.Column;
        _portsAndContentContainer.style.height = 400 - titleContainer.resolvedStyle.height;
        //ConsoleUtils.Log("�߶�", mainContainer[1].resolvedStyle.height);
        // ����������ݵ���ʽ
        _leftContainer.style.flexGrow = 1; // ռ��ʣ��Ŀռ�
        _leftContainer.style.alignItems = Align.FlexStart;
        _leftContainer.style.justifyContent = Justify.Center;

        // �����м����ݵ���ʽ
        _middleContainer.style.flexGrow = 1; // ռ��ʣ��Ŀռ�
        _middleContainer.style.alignItems = Align.FlexStart;
        _middleContainer.style.justifyContent = Justify.FlexStart;

        // �����ұ����ݵ���ʽ
        _rightContainer.style.flexGrow = 1; // ռ��ʣ��Ŀռ�
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
