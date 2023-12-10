using System;
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

            AttackComp attack = entity.Get<AttackComp>();

            //�ж��Ƿ񹥻��ڵ�
            if(attack != null)
            {
                //�ж��Ƿ���Դ���
                bool attackEnable = AttackSingleton.Ins().GetAttackEnable(attack.entityId);
                if (!attackEnable) continue;
                int i = 0;
            }

            if (trigger.isPositive)
            {
                List<QTreeObj> objs = QTreeSingleton.Ins().GetQtreeObjs(entity.id);

                //��ʼ����ʵ�崥�������д�����״̬
                foreach (var obj in objs)
                {
                    if (obj.entity.id == entity.id || attack != null && attack.entityId == obj.entity.id) continue;
                    TriggerComp triggerComp = obj.entity.Get<TriggerComp>();

                    if (triggerComp != null)
                    {
                        foreach(var triggerFunc in trigger.triggerFunc)
                        {
                            Dictionary<TriggerFunction, TriggerStatus> states;
                            triggerComp.status.TryGetValue(entity.id, out states);

                            if(states == null)
                            {
                                states = new Dictionary<TriggerFunction, TriggerStatus>();
                                triggerComp.status.Add(entity.id, states);
                            }

                            TriggerStatus state;
                            states.TryGetValue(triggerFunc,out state);

                            if (!states.ContainsKey(triggerFunc))
                            {
                                //triggerComp.status.Add(entity.id, TriggerStatus.Enter);
                                states[triggerFunc] = TriggerStatus.Enter;
                            }
                            else if (state == TriggerStatus.Exit)
                            {
                                //triggerComp.status[entity.id] = TriggerStatus.Keeping;
                                states[triggerFunc] = TriggerStatus.Keeping;
                            }
                            else if (state == TriggerStatus.Idle)
                            {
                                states[triggerFunc] = TriggerStatus.Enter;
                            }
                        }
                    }
                }
            }


            var keys = trigger.status.Keys.ToArray();
            //ִ�б�ʵ�����д����¼�
            foreach (var key in keys)
            {
                Entity triggerEntity = ECSManager.Ins().GetEntity(key);

                foreach(var triggerFunc in trigger.triggerFunc)
                {
                    TriggerStatus state;
                    trigger.status[key].TryGetValue(triggerFunc,out state);
                    Action<Entity, Entity> action;
                    switch (state)
                    {
                        case TriggerStatus.Enter:
                            trigger.OnTriggerEnter.TryGetValue(triggerFunc, out action);
                            if (action != null)
                            {
                                action.Invoke(entity, triggerEntity);
                            }
                            trigger.status[key][triggerFunc] = TriggerStatus.Keeping;
                            break;
                        case TriggerStatus.Keeping:
                            trigger.OnTriggerKeeping.TryGetValue(triggerFunc, out action);
                            if (action != null)
                            {
                                action.Invoke(entity, triggerEntity);
                            }
                            trigger.status[key][triggerFunc] = TriggerStatus.Exit;
                            break;
                        case TriggerStatus.Exit:
                            trigger.OnTriggerExit.TryGetValue(triggerFunc, out action);
                            if (action != null)
                            {
                                action.Invoke(entity, triggerEntity);
                            }
                            trigger.status[key][triggerFunc] = TriggerStatus.Idle;
                            break;
                    }
                }
            }
        }
    }
}
