using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ECSWorld
{
    private List<ECSSystem> _systems = new List<ECSSystem>();

    public ECSWorld() {
        SystemAdd();
        Init();
    }

    public void Init()
    {
        foreach (var system in _systems)
        {
            system.Init();
        }
    }

    public abstract void SystemAdd();

    public void Update()
    {
        foreach (var system in _systems)
        {
            system.Execute(Time.deltaTime);
        }
    }

    public void DrawGrizmos()
    {
        foreach (var system in _systems)
        {
            system.DrawGizmos();
        }
    }

    public void Add(ECSSystem system)
    {
        _systems.Add(system);
    }

    public void Remove(ECSSystem system)
    {
        _systems.Remove(system);
    }

    public void Clear()
    {
        _systems.Clear();
    }
}
