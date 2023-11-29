using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    /// <summary>
    /// �����б�
    /// </summary>
    private Dictionary<int, SkillData> _skills = new Dictionary<int, SkillData>();

    public void Init()
    {
        //�����ñ��ʼ��
    }

    public SkillData GetSkill(int id)
    {
        SkillData skill;
        _skills.TryGetValue(id, out skill);
        return skill;
    }

    /// <summary>
    /// ��ȡ��һ����
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public SkillData NextSkill(SkillData skill, InputKey key)
    {
        for(int i = 0; i < skill.next.Count; i++)
        {
            SkillData next = skill.next[i];
            if(key == next.key)
            {
                return next;
            }
        }
        return null;
    }

    private void SetSkill(int id, SkillData skill)
    {
        _skills.Add(id, skill);
    }
}
