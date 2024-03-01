
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : ECSSystem
{
    private string _idle = "Idle";
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(SkillComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            SkillComp skill = entity.Get<SkillComp>();
            //AniComp ani = entity.Get<AniComp>();
            //LogicAniComp logicAni = entity.Get<LogicAniComp>();

            switch (skill.play)
            {
                case SkillPlayStatus.Idle:
                    int skillId = 0;
                    AniSingleton.Ins().SetCurAni(entity.id, _idle);
                    AttackSingleton.Ins().SetAttackEnable(entity.id, false);

                    if (skill.status == InputStatus.Down)
                    {
                        skill.id.TryGetValue(skill.key, out skillId);
                    }

                    if (skillId > 0)
                    {
                        ConsoleUtils.Log("匹配到技能按键", skill.key);
                        skill.skill = SkillManager.Ins().GetSkill(skillId);
                    }

                    if (skill.skill != null)
                    {
                        if (skill.skill.trigger != SkillTrigger.Charge)
                        {
                            skill.play = SkillPlayStatus.Play;
                        }
                        else
                        {
                            skill.play = SkillPlayStatus.Charge;
                        }
                        ConsoleUtils.Log("释放技能", skill.skill.name);
                    }
                    break;
                case SkillPlayStatus.Charge:
                    if (skill.trigger != skill.skill.trigger)
                    {
                        //停止蓄力
                        skill.duration = 0;
                        skill.play = SkillPlayStatus.End;
                    }
                    else if (skill.duration >= skill.skill.holdingTime)
                    {
                        //蓄力完成
                        skill.duration = 0;
                        skill.play = SkillPlayStatus.Play;
                    }
                    skill.duration += _dt;
                    break;
                case SkillPlayStatus.Play:
                    if (skill.duration == 0)
                    {
                        AniSingleton.Ins().SetCurAni(entity.id, skill.skill.ani, true);
                        AttackSingleton.Ins().SetAttackEnable(entity.id, skill.skill.attackEnable);
                        if (skill.skill.action != "")
                        {
                            SkillManager.Ins().PlaySkill(entity, skill.skill.action);
                        }
                    }
                    //释放技能
                    if (skill.duration >= skill.skill.skillTime)
                    {
                        //技能释放结束
                        skill.duration = 0;
                        skill.play = SkillPlayStatus.End;
                    }
                    skill.duration += _dt;
                    break;
                case SkillPlayStatus.End:

                    skill.duration += _dt;

                    if (skill.duration >= skill.skill.outOfTime)
                    {
                        ConsoleUtils.Log("连招结束", skill.duration);
                        //连招超时
                        skill.duration = 0;
                        skill.play = SkillPlayStatus.Idle;
                        skill.skill = null;
                    }
                    else if (skill.status == InputStatus.Down)
                    {
                        skill.skill = SkillManager.Ins().NextSkill(skill.skill, skill.key);

                        if (skill.skill == null)
                        {
                            //判断是否成功变招
                            skill.id.TryGetValue(skill.key, out skillId);
                            skill.skill = SkillManager.Ins().GetSkill(skillId);
                        }

                        if (skill.skill != null)
                        {
                            AniSingleton.Ins().SetCurAni(entity.id, _idle, true);
                            AttackSingleton.Ins().SetAttackEnable(entity.id, false);

                            skill.duration = 0;
                            ConsoleUtils.Log("下一招", skill.skill.name);
                            //有后续技能
                            if (skill.skill.trigger != SkillTrigger.Charge)
                            {
                                skill.play = SkillPlayStatus.Play;
                            }
                            else
                            {
                                skill.play = SkillPlayStatus.Charge;
                            }
                        }
                        else
                        {
                            //没有后续技能
                            skill.play = SkillPlayStatus.Idle;
                        }
                    }

                    break;
            }
        }
    }
}
