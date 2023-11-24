using Game;
using System;
using System.Collections.Generic;

public enum ECSRuleType
{
    AllOf,
    AnyOf,
    ExcludeOf
}

public class ECSRule
{
    int compId = -1;

    private ECSMask _mask = new ECSMask();

    private ECSRuleType _type;

    private List<int> _compIds = new List<int>();

    public ECSRule(ECSRuleType ecsType, params Type[] comps)
    {
        _type = ecsType;
        foreach (var comp in comps)
        {
            compId = ECSManager.Ins().GetCompId(comp);

            if (compId == -1)
            {
                ConsoleUtils.Warn("¥Ê‘⁄Œ¥◊¢≤·◊Èº˛", compId);
            }
            else
            {
                _mask.Set(compId);
                _compIds.Add(compId);
            }
        }
        
        if(_compIds.Count > 0)
        {
            _compIds.Sort((a,b) => { return a.CompareTo(b); });
        }
    }

    public List<int> GetCompIds()
    {
        return _compIds;
    }

    /// <summary>
    ///  «∑Ò∆•≈‰
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool isMatch(Entity entity)
    {
        switch (_type)
        {
            case ECSRuleType.AllOf:
                return _mask.And(entity.mask);
            case ECSRuleType.AnyOf:
                return _mask.Or(entity.mask);
            case ECSRuleType.ExcludeOf:
                return !_mask.Or(entity.mask);
        }
        return false;
    }
}
