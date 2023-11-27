using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(TriggerComp), typeof(QTreeComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            TriggerComp trigger = entity.Get<TriggerComp>();

            List<QTreeObj> objs = QTreeSingleton.Ins().GetQtreeObjs(entity.id);

            foreach (var obj in objs)
            {
                TriggerComp triggerComp = obj.entity.Get<TriggerComp>();

                if (triggerComp != null)
                {
                    TriggerStatus state;
                    triggerComp.status.TryGetValue(entity.id, out state);

                    if (!triggerComp.status.ContainsKey(entity.id))
                    {
                        triggerComp.status.Add(entity.id, TriggerStatus.Enter);
                    }
                    else if (state == TriggerStatus.Exit)
                    {
                        triggerComp.status[entity.id] = TriggerStatus.Keeping;
                    }
                    else if (state == TriggerStatus.Idle)
                    {
                        triggerComp.status[entity.id] = TriggerStatus.Enter;
                    }
                }
            }


            var keys = trigger.status.Keys.ToArray();

            foreach (var key in keys)
            {
                TriggerStatus state = trigger.status[key];
                Entity triggerEntity = ECSManager.Ins().GetEntity(key);
                switch (state)
                {
                    case TriggerStatus.Enter:
                        if (trigger.OnTriggerEnter != null)
                        {
                            trigger.OnTriggerEnter(entity, triggerEntity);
                        }
                        trigger.status[key] = TriggerStatus.Keeping;
                        break;
                    case TriggerStatus.Keeping:
                        if (trigger.OnTriggerKeeping != null)
                        {
                            trigger.OnTriggerKeeping(entity, triggerEntity);
                        }
                        trigger.status[key] = TriggerStatus.Exit;
                        break;
                    case TriggerStatus.Exit:
                        if (trigger.OnTriggerExit != null)
                        {
                            trigger.OnTriggerExit(entity, triggerEntity);
                        }
                        trigger.status[key] = TriggerStatus.Idle;
                        break;
                }
            }
        }
    }
}
