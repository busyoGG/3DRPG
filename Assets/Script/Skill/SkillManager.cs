using Bean;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    /// <summary>
    /// 技能列表
    /// </summary>
    private Dictionary<int, SkillConfigData> _skills = new Dictionary<int, SkillConfigData>();

    public void Init()
    {
        //读配置表初始化
        _skills = ConfigManager.Ins().GetConfig<SkillConfigData>(ConfigsFolderConfig.Null, ConfigsNameConfig.SkillConfig);
        ConsoleUtils.Log("技能", _skills);
    }

    public SkillConfigData GetSkill(int id)
    {
        SkillConfigData skill;
        _skills.TryGetValue(id, out skill);
        return skill;
    }

    /// <summary>
    /// 获取下一技能
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public SkillConfigData NextSkill(SkillConfigData skill, InputKey key)
    {
        for(int i = 0; i < skill.next.Count; i++)
        {
            SkillConfigData next = skill.next[i];
            if(key == next.key)
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
