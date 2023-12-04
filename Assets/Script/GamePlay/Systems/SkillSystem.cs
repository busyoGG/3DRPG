
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : ECSSystem
{
    private string _idle = "Idle";
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(SkillComp), typeof(AniComp), typeof(LogicAniComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            SkillComp skill = entity.Get<SkillComp>();
            AniComp ani = entity.Get<AniComp>();
            LogicAniComp logicAni = entity.Get<LogicAniComp>();

            switch (skill.play)
            {
                case SkillPlayStatus.Idle:
                    int skillId = 0;
                    ani.curAni = _idle;
                    logicAni.curAni = _idle;

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
                    ani.curAni = skill.skill.ani;
                    logicAni.curAni = skill.skill.ani;
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
                    //ConsoleUtils.Log("连招结束", skill.duration);

                    if (skill.duration >= skill.skill.outOfTime)
                    {
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
                            ani.lastAni = _idle;
                            logicAni.lastAni = _idle;
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
