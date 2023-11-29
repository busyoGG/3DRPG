using System.Collections.Generic;
using UnityEngine;

public enum SkillTrigger
{
    Click,
    Hold,
    Charge
}

public class SkillData
{
    /// <summary>
    /// 技能名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 技能id 一系列技能使用同一id
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 键位需求
    /// </summary>
    public InputKey key { get; set; }
    /// <summary>
    /// 下一连招列表
    /// </summary>
    public List<SkillData> next { get; set; }
    /// <summary>
    /// 技能持续时间
    /// </summary>
    public float skillTime { get; set; }
    /// <summary>
    /// 连招超时时间
    /// </summary>
    public float outOfTime { get; set; }
    /// <summary>
    /// 是否可以强制打断技能
    /// </summary>
    public bool isCanForceStop { get; set; }
    /// <summary>
    /// 强制打断次数
    /// </summary>
    public int forceTimes { get; set; }
    /// <summary>
    /// 默认强制打断次数
    /// </summary>
    public int defaultForceTimes { get; set; }
    /// <summary>
    /// 强制打断重置时间
    /// </summary>
    public float forceResetTime { get; set; }
    /// <summary>
    /// 技能触发类型
    /// </summary>
    public SkillTrigger trigger { get; set; }
    /// <summary>
    /// 蓄力时间
    /// </summary>
    public float holdingTime { get; set; }

    public SkillData() { 
        trigger = SkillTrigger.Click;
        next = new List<SkillData>();
    }
}
