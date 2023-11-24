using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapStatus
{
    Inactive,
    Active,
    Cache
}

public class MapData
{
    public int id { get; set; }

    public int qTreeId { get; set; }

    public Vector3 pos { get; set; }

    public GameObject chunk { get; set; }

    public List<MapObjectData> objects { get; set; }

    public MapStatus curStatus { get; set; }

    public MapStatus lastStatus { get; set; }

    public override string ToString()
    {
        //return "{ id:" + id + ", pos:" + pos + ", curStatus:" + curStatus + ", lastStatus:" + lastStatus + " }";
        return pos.ToString();
    }
}
