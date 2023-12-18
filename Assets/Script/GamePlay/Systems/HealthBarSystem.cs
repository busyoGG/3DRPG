using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarSystem : ECSSystem
{
    private List<Entity> _entity2Die = new List<Entity>();

    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(HealthBarComp), typeof(RenderComp));
    }

    public override void OnEnter(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            HealthBarComp hp = entity.Get<HealthBarComp>();
            RenderComp render = entity.Get<RenderComp>();

            PropData prop = PropManager.Ins().GetPropData(entity.id);

            if (hp.ui == null)
            {
                hp.ui = UIManager.Ins().AddUI<HealthBar>("HealthBar", render.node.transform);
            }

            hp.ui.SetOffsetY(-120);
            hp.ui.SetProp(prop.hp);
            hp.ui.SetHp(prop.curHp);
            hp.ui.Show();
        }
    }

    public override void OnUpdate(List<Entity> entities)
    {
        Vector3 camPos = Global.Cam.GetPosition();
        foreach (Entity entity in entities)
        {
            HealthBarComp hp = entity.Get<HealthBarComp>();
            PropData prop = PropManager.Ins().GetPropData(entity.id);

            hp.ui.SetHp(prop.curHp);

            hp.ui.panel.transform.LookAt(camPos);

            if (prop.curHp <= 0)
            {
                _entity2Die.Add(entity);
            }
        }

        for (int i = 0; i < _entity2Die.Count; i++)
        {
            ECSManager.Ins().RemoveEntity(_entity2Die[i]);
        }
        _entity2Die.Clear();
    }

    public override void OnRemove(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            HealthBarComp hp = entity.Get<HealthBarComp>();
            if (hp != null)
            {
                //entity非回收状态情况下
                hp.ui.Hide();
            }
        }
    }
}
