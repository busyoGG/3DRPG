using Bean;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DialogManager : Singleton<DialogManager>
{
    private Dictionary<int, DialogConfigData> _allConfigs;
    public Dictionary<int, DialogConfigData> _rootConfigs = new Dictionary<int, DialogConfigData>();

    public void Init()
    {
        _allConfigs = ConfigManager.Ins().GetConfig<DialogConfigData>(ConfigsFolderConfig.Null, ConfigsNameConfig.DialogConfig);

        foreach (var config in _allConfigs.Values)
        {
            if (!_rootConfigs.ContainsKey(config.dialogId) && config.stepId == 0)
            {
                _rootConfigs.Add(config.dialogId, config);
            }
        }
    }

    /// <summary>
    /// 获取对话
    /// </summary>
    /// <param name="dialogId"></param>
    /// <param name="stepId"></param>
    /// <returns></returns>
    public DialogConfigData GetDialog(int dialogId)
    {
        return _rootConfigs[dialogId];
    }

    /// <summary>
    /// 下一对话
    /// </summary>
    /// <param name="cur"></param>
    /// <param name="selection"></param>
    /// <returns></returns>
    public DialogConfigData Next(DialogConfigData cur, int selection)
    {
        if (cur.selection.Count == 0)
        {
            if (cur.next.Count > 0)
            {
                return _allConfigs[cur.next[0]];
            }
            else
            {
                return null;
            }
        }
        else
        {
            return _allConfigs[cur.next[selection]];
        }
    }
}
