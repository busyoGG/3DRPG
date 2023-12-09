
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    private List<MapData> _chunks = new List<MapData>();

    private float _chunkSize = 0;

    private int _max = 0;

    private Vector2 _mapSize = Vector2.zero;

    private Vector3 _startPos = Vector3.zero;

    private Vector3 _playerPos = Vector3.zero;

    private List<MapData> _cache = new List<MapData>(25);

    private Queue<MapData> _updateList = new Queue<MapData>();

    private Transform _poolNode;

    private Transform _mapNode;

    private MapData _lastChunk;

    private bool _generateQTree = true;

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public void Init()
    {
        _poolNode = new GameObject().transform;
        _mapNode = new GameObject().transform;

        _poolNode.name = "MapPool";
        _mapNode.name = "MapRoot";

        _poolNode.gameObject.SetActive(false);
    }

    /// <summary>
    /// ����Chunk ��ʱ����
    /// </summary>
    /// <param name="root"></param>
    public void CalculateMap(GameObject root)
    {
        List<Transform> children = new List<Transform>();
        for (int i = 0, len = root.transform.childCount; i < len; i++)
        {
            children.Add(root.transform.GetChild(i));
        }

        if (children.Count == 0) return;

        children.Sort((a, b) =>
        {
            int resZ = a.transform.position.z.CompareTo(b.transform.position.z);
            if (resZ == 0)
            {
                int resX = a.transform.position.x.CompareTo(b.transform.position.x);
                return resX;
            }
            return resZ;
        });
        //children.Sort((a, b) => { return a.transform.position.x.CompareTo(b.transform.position.x); });

        _chunkSize = children[0].transform.localScale.x;
        _startPos = children[0].transform.position;

        for (int i = 0, len = children.Count; i < len; i++)
        {
            MapData md = new MapData();
            md.id = i;
            md.pos = children[i].transform.position;
            md.objects = new List<MapObjectData>();
            _chunks.Add(md);
            //ÿ���ؿ鴴���Ĳ���
            if (_generateQTree)
            {
                AABBData bounds = QtreeManager.Ins().CreateBounds(md.pos, new Vector3(_chunkSize, 9999, _chunkSize));
                int deep = 1;
                while(_chunkSize / Mathf.Pow(4, deep) > 16)
                {
                    deep++;
                }
                QtreeManager.Ins().CreateQtree(bounds, deep, 10);
            }
        }

        Vector3 endPos = children[children.Count - 1].transform.position;
        _mapSize.x = (endPos.x - _startPos.x) / _chunkSize + 1;
        _mapSize.y = (endPos.z - _startPos.z) / _chunkSize + 1;

        _lastChunk = new MapData();
        _lastChunk.id = -1;

        _max = _chunks.Count;

        ConsoleUtils.Log(_chunks, _mapSize, _chunkSize);
    }

    /// <summary>
    /// ˢ�����λ��
    /// </summary>
    /// <param name="pos"></param>
    public void RefreshPlayerPos(Vector3 pos)
    {
        _playerPos = pos;
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public int GetIndex(Vector3 position)
    {
        int mapX = Mathf.RoundToInt((position.x - _startPos.x) / _chunkSize);
        int mapZ = Mathf.RoundToInt((position.z - _startPos.z) / _chunkSize);
        return (int)_mapSize.x * mapZ + mapX;
    }

    /// <summary>
    /// ����Chunk
    /// </summary>
    public void Update()
    {
        int playerMapX = Mathf.RoundToInt((_playerPos.x - _startPos.x) / _chunkSize);
        int playerMapZ = Mathf.RoundToInt((_playerPos.z - _startPos.z) / _chunkSize);
        int index = (int)_mapSize.x * playerMapZ + playerMapX;

        if (index < 0 || index > _max - 1) return;
        MapData newCenter = _chunks[index];

        //�������鲻ͬ �������黺��
        if (_lastChunk.id != newCenter.id)
        {
            ConsoleUtils.Log("���index:" + index);
            _lastChunk = newCenter;
            //��ǰ���黺���б�
            List<MapData> newChunks = new List<MapData>();
            int baseMin = (int)_mapSize.x * playerMapZ;
            for (int i = -2; i <= 2; i++)
            {
                int minIndex = baseMin + i * (int)_mapSize.x;
                int maxIndex = baseMin + (i + 1) * (int)_mapSize.x - 1;
                for (int j = -2; j <= 2; j++)
                {
                    int chunkIndex = index + i * (int)_mapSize.x + j;
                    //��ֹ������Χ
                    if (chunkIndex < 0 || chunkIndex > _max - 1 || chunkIndex < minIndex || chunkIndex > maxIndex) continue;
                    ConsoleUtils.Log(chunkIndex);
                    MapData chunk = _chunks[chunkIndex];
                    if (Mathf.Abs(i) == 2 || Mathf.Abs(j) == 2)
                    {
                        chunk.curStatus = MapStatus.Cache;
                    }
                    else
                    {
                        chunk.curStatus = MapStatus.Active;
                    }
                    newChunks.Add(chunk);
                }
            }
            //���¾ɻ����б�
            int count = _cache.Count;
            if (count > 0)
            {
                for (int i = _cache.Count - 1; i >= 0; i--)
                {
                    if (newChunks.Contains(_cache[i]))
                    {
                        if (_cache[i].curStatus != _cache[i].lastStatus)
                        {
                            _updateList.Enqueue(_cache[i]);
                        }
                        newChunks.Remove(_cache[i]);
                    }
                    else
                    {
                        _cache[i].curStatus = MapStatus.Inactive;
                        _updateList.Enqueue(_cache[i]);
                        _cache.Remove(_cache[i]);
                    }
                }
                _cache.AddRange(newChunks);
            }
            else
            {
                _cache = newChunks;
                for (int i = 0, len = _cache.Count; i < len; i++)
                {
                    _updateList.Enqueue(_cache[i]);
                }
            }
        }
    }

    /// <summary>
    /// ˢ��Chunk��Ⱦ
    /// </summary>
    public void RefreshChunk()
    {
        int i = _updateList.Count;
        while (i > 0)
        {
            MapData chunk = _updateList.Dequeue();
            switch (chunk.curStatus)
            {
                case MapStatus.Active:
                    Active(chunk);
                    break;
                case MapStatus.Inactive:
                    //��ʱ��inactive ʵ��Ϊunload
                    Inactive(chunk);
                    break;
                case MapStatus.Cache:
                    Inactive(chunk);
                    break;
            }
            chunk.lastStatus = chunk.curStatus;
            i--;
        }
    }
    /// <summary>
    /// ����Chunk
    /// </summary>
    /// <param name="chunk"></param>
    private void Active(MapData chunk)
    {
        if (chunk.chunk == null)
        {
            Load(chunk);
        }
        chunk.chunk.transform.parent = _mapNode;
        ConsoleUtils.Log("����" + chunk.id);
    }
    /// <summary>
    /// ����Chunk
    /// </summary>
    /// <param name="chunk"></param>
    private void Inactive(MapData chunk)
    {
        if (chunk.chunk == null)
        {
            Load(chunk);
        }
        chunk.chunk.transform.parent = _poolNode;
        ConsoleUtils.Log("����" + chunk.id);
    }
    /// <summary>
    /// ����ģ�� ��ʱ����
    /// </summary>
    /// <param name="chunk"></param>
    private void Load(MapData chunk)
    {
        if (chunk.chunk == null)
        {
            chunk.chunk = GameObject.CreatePrimitive(PrimitiveType.Cube);
            chunk.chunk.transform.localScale = new Vector3(_chunkSize, 1, _chunkSize);
            chunk.chunk.transform.position = chunk.pos;
            chunk.chunk.transform.parent = _poolNode;
        }
        for (int i = 0, len = chunk.objects.Count; i < len; i++)
        {
            MapObjectData obj = chunk.objects[i];
            if (obj.obj == null)
            {
                //�����ؿ�
                obj.obj = new GameObject();
                obj.obj.transform.position = obj.pos;
                obj.obj.transform.rotation = obj.rot;
                obj.obj.transform.localScale = obj.scale;
            }
        }
        ConsoleUtils.Log("����" + chunk.id);
    }
}
