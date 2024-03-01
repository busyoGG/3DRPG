using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器系统，区分武器是攻击状态还是待机状态，攻击状态才允许其触发触发器
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
