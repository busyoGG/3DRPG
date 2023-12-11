using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ϵͳ�����������ǹ���״̬���Ǵ���״̬������״̬�������䴥��������
/// </summary>
public class WeaponSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(WeaponComp),typeof(TriggerComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            TriggerComp trigger = entity.Get<TriggerComp>();
            WeaponComp weapon = entity.Get<WeaponComp>();

            bool isAttack = AttackSingleton.Ins().GetAttackEnable(weapon.entityId);
            trigger.isEnable = isAttack;
        }
    }
}
