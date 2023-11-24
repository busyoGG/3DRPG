using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group
{
    private ECSMatcher _matcher;

    //private List<Entity> _enters = new List<Entity>();

    //private List<Entity> _removes = new List<Entity>();

    public delegate void EnterListener(Entity entity);

    public delegate void RemoveListener(Entity entity);

    private EnterListener _enterListener;

    private RemoveListener _removeListener;

    private List<Entity> _entities = new List<Entity>();

    public Group(ECSMatcher matcher)
    {
        _matcher = matcher;
    }

    public void OnCompChange(Entity entity)
    {
        if(_matcher.IsMatch(entity))
        {
            _enterListener.Invoke(entity);
            _entities.Add(entity);
        }
        else
        {
            _removeListener.Invoke(entity);
            _entities.Remove(entity);
        }
    }

    public void AddEntityListener(EnterListener enter, RemoveListener remove)
    {
        _enterListener += enter;
        _removeListener += remove;
    }

    public void RemoveEntityListener(EnterListener enter, RemoveListener remove)
    {
        _enterListener -= enter;
        _removeListener -= remove;
    }

    public List<Entity> GetEntities()
    {
        return _entities;
    }

    public void Clear()
    {
        _entities.Clear();
    }
}
