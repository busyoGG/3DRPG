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
    /// ��������
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// ����id һϵ�м���ʹ��ͬһid
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// ��λ����
    /// </summary>
    public InputKey key { get; set; }
    /// <summary>
    /// ��һ�����б�
    /// </summary>
    public List<SkillData> next { get; set; }
    /// <summary>
    /// ���ܳ���ʱ��
    /// </summary>
    public float skillTime { get; set; }
    /// <summary>
    /// ���г�ʱʱ��
    /// </summary>
    public float outOfTime { get; set; }
    /// <summary>
    /// �Ƿ����ǿ�ƴ�ϼ���
    /// </summary>
    public bool isCanForceStop { get; set; }
    /// <summary>
    /// ǿ�ƴ�ϴ���
    /// </summary>
    public int forceTimes { get; set; }
    /// <summary>
    /// Ĭ��ǿ�ƴ�ϴ���
    /// </summary>
    public int defaultForceTimes { get; set; }
    /// <summary>
    /// ǿ�ƴ������ʱ��
    /// </summary>
    public float forceResetTime { get; set; }
    /// <summary>
    /// ���ܴ�������
    /// </summary>
    public SkillTrigger trigger { get; set; }
    /// <summary>
    /// ����ʱ��
    /// </summary>
    public float holdingTime { get; set; }

    public SkillData() { 
        trigger = SkillTrigger.Click;
        next = new List<SkillData>();
    }
}
