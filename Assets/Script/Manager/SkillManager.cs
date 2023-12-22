using Bean;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    /// <summary>
    /// �����б�
    /// </summary>
    private Dictionary<int, SkillConfigData> _skills = new Dictionary<int, SkillConfigData>();

    private Dictionary<int, SkillConfigData> _allSkills = new Dictionary<int, SkillConfigData>();

    public void Init()
    {
        //�����ñ��ʼ��
        _allSkills = ConfigManager.Ins().GetConfig<SkillConfigData>(ConfigsFolderConfig.Null, ConfigsNameConfig.SkillConfig);

        foreach (var skill in _allSkills)
        {
            if (skill.Value.stepId == 0)
            {
                _skills.Add(skill.Value.skillId, skill.Value);
            }
        }

        ConsoleUtils.Log("����", _skills);
    }

    public SkillConfigData GetSkill(int id)
    {
        SkillConfigData skill;
        _skills.TryGetValue(id, out skill);
        return skill;
    }

    /// <summary>
    /// ��ȡ��һ����
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public SkillConfigData NextSkill(SkillConfigData skill, InputKey key)
    {
        for (int i = 0; i < skill.next.Count; i++)
        {
            SkillConfigData next = _allSkills[skill.next[i]];
            if (key == next.key)
            {
                return next;
            }
        }
        return null;
    }

    private void SetSkill(int id, SkillConfigData skill)
    {
        _skills.Add(id, skill);
    }
}
