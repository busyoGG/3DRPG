


using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ConfigGraph : GraphView
{
    private Vector2 _mousePos;
    public ConfigGraph() : base()
    {
        //��ʼ��������ͼ
        var bg = new GridBackground();
        var obj = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/ConfigGenerator/Graph/GridBackground.uss");
        if (obj != null)
        {
            bg.styleSheets.Add(obj);
        }
        Insert(0, bg);
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        //��ʼ��ͼ��Ԫ��
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new ContentDragger());

        //nodeCreationRequest += context =>
        //{
        //    MakeNode(_mousePos);
        //};

        // �������ƶ��¼��ļ�����
        this.RegisterCallback<MouseDownEvent>(OnMouseDown);

    }

    public void Init(string folder, string name)
    {
        string config = FileUtils.ReadFile(Application.dataPath + "/Configs/" + (folder != null ? folder + "/" : "") + name + ".json");
        config = Regex.Replace(config, "(\\|).*(?=\")", "");

        ConsoleUtils.Log(config);
        JsonData json = JsonMapper.ToObject(config);

        Dictionary<string, ConfigNode> _rootNode = new Dictionary<string, ConfigNode>();
        Dictionary<string, ConfigNode> _allNode = new Dictionary<string, ConfigNode>();

        foreach (JsonData item in json)
        {
            ConfigNode node = MakeNode(Vector2.zero, item);
            if (item["stepId"] == null || int.Parse(item["stepId"].ToString()) == 0)
            {
                if (item["mainId"] != null)
                {
                    _rootNode.Add(item["mainId"].ToString(), node);
                }
                else
                {
                    _rootNode.Add(item["id"].ToString(), node);
                }
                //Add(node);
            }

            _allNode.Add(item["id"].ToString(), node);
        }

        foreach (JsonData item in json)
        {
            ConfigNode parent = _allNode[item["id"].ToString()];
            ConfigNode next;
            if (item["next"] != null)
            {
                JsonType type = item["next"].GetJsonType();
                if (type.Equals(JsonType.Array))
                {
                    List<string> list = new List<string>();

                    for (int i = 0; i < item["next"].Count; i++)
                    {
                        list.Add(item["next"][i].ToString());
                    }

                    foreach (string data in list)
                    {
                        _allNode.TryGetValue(data, out next);
                        if (next != null)
                        {
                            MakeEdge(parent, next);
                        }
                    }
                }
                else
                {
                    _allNode.TryGetValue(item["next"].ToString(), out next);
                    if (next != null)
                    {
                        MakeEdge(parent, next);
                    }
                }
            }

        }

        AutoLayout();
    }

    private void OnMouseDown(MouseDownEvent evt)
    {
        // ��ȡ����� GraphView �е�����
        _mousePos = contentViewContainer.WorldToLocal(evt.mousePosition);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        //�洢���������ļ��ݵĶ˿�
        List<Port> compatiblePorts = new List<Port>();
        //����Graphview�����е�Port ����Ѱ��
        ports.ForEach(
           (port) =>
           {
               if (startPort.node != port.node && startPort.direction != port.direction)
               {
                   compatiblePorts.Add(port);
               }
           }
        );
        return compatiblePorts;
    }

    /// <summary>
    /// �˿�����
    /// </summary>
    /// <param name="oput"></param>
    /// <param name="iput"></param>
    /// <returns></returns>
    public Edge MakeEdge(ConfigNode oput, ConfigNode iput)
    {
        var edge = new Edge { output = oput._rightContainer[0] as Port, input = iput._leftContainer[0] as Port };
        edge.input.Connect(edge);
        edge.output.Connect(edge);
        AddElement(edge);

        // �ֶ����������б�
        //(oput._rightContainer[0] as Port).connections.Add(edge);
        //(iput._leftContainer[0] as Port).connections.Add(edge);

        return edge;
    }

    /// <summary>
    /// �����ڵ�
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public ConfigNode MakeNode(Vector2 position, JsonData item)
    {
        ConfigNode node = new ConfigNode(item);
        if (item["stepId"] != null)
        {
            node.userData = int.Parse(item["stepId"].ToString());
        }
        else
        {
            node.userData = 0;
        }
        node.SetPosition(new Rect(position, Vector2.zero));
        AddElement(node);

        node.name = item["id"].ToString();
        return node;
    }

    private void AutoLayout()
    {
        var rootNodes = this.Query<ConfigNode>().Where(node => GetNodeDepth(node) == 0).ToList();

        // ���ڵ�λ��
        var rootPosition = new Vector2(200, 100);

        // ���㲼��
        foreach (var rootNode in rootNodes)
        {
            AutoLayoutRecursively(rootNode, rootPosition, 0);
            rootPosition.y += 300; // ������
        }
    }

    private void AutoLayoutRecursively(ConfigNode parentNode, Vector2 position, int depth)
    {
        //var nodesAtDepth = this.Query<ConfigNode>().Where(node =>
        //{
        //    bool isRoot = depth == 0;
        //    return (GetNodeDepth(node) == depth && GetNodeParent(node) == parentNode) || isRoot;
        //}).ToList();
        var nodesAtDepth = this.Query<ConfigNode>().Where(node => GetNodeDepth(node) == depth).ToList();

        foreach (var node in nodesAtDepth)
        {
            node.SetPosition(new Rect(position, Vector2.zero));
            ConsoleUtils.Log("��������", node, position);
            // �ݹ鲼���ӽڵ�
            AutoLayoutRecursively(node, position + new Vector2(300, 0), depth + 1);
        }
    }

    private int GetNodeDepth(ConfigNode node)
    {
        //ConsoleUtils.Log("���", node.userData);
        return (int)node.userData;
    }

    private List<ConfigNode> GetNodeChild(ConfigNode node)
    {
        //var edges = node.Query<Edge>().ToList();
        var ports = node._rightContainer.Query<Port>().ToList();
        var edges = ports[0].connections.ToList();


        if (edges.Count > 0)
        {
            List<ConfigNode> list = new List<ConfigNode>();
            foreach (var edge in edges)
            {
                list.Add(edge.input.node as ConfigNode);
            }
            return list;
        }

        return null;
    }
}
