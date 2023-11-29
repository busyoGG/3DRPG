using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(SkillComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            SkillComp skill = entity.Get<SkillComp>();

            switch (skill.play)
            {
                case SkillPlayStatus.Idle:
                    int skillId = 0;

                    if (skill.status == InputStatus.Down)
                    {
                        skill.id.TryGetValue(skill.key, out skillId);
                    }

                    if (skillId > 0)
                    {
                        ConsoleUtils.Log("ƥ�䵽���ܰ���", skill.key);
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
                    }
                    break;
                case SkillPlayStatus.Charge:
                    if (skill.trigger != skill.skill.trigger)
                    {
                        //ֹͣ����
                        skill.duration = 0;
                        skill.play = SkillPlayStatus.End;
                    }
                    else if (skill.duration >= skill.skill.holdingTime)
                    {
                        //�������
                        skill.duration = 0;
                        skill.play = SkillPlayStatus.Play;
                    }
                    skill.duration += _dt;
                    break;
                case SkillPlayStatus.Play:
                    //�ͷż���
                    if (skill.duration >= skill.skill.skillTime)
                    {
                        //�����ͷŽ���
                        skill.duration = 0;
                        skill.play = SkillPlayStatus.End;
                    }
                    skill.duration += _dt;
                    break;
                case SkillPlayStatus.End:

                    skill.duration += _dt;

                    if (skill.duration >= skill.skill.outOfTime)
                    {
                        //���г�ʱ
                        skill.duration = 0;
                        skill.play = SkillPlayStatus.Idle;
                    }
                    else if (skill.status == InputStatus.Down)
                    {
                        skill.skill = SkillManager.Ins().NextSkill(skill.skill, skill.key);
                        if (skill.skill != null)
                        {
                            //�к�������
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
                            //û�к�������
                            skill.play = SkillPlayStatus.Idle;
                        }
                    }

                    break;
            }
        }
    }
}
