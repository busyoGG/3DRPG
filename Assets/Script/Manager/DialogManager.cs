using Bean;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DialogManager : Singleton<DialogManager>
{
    public Dictionary<int, List<DialogConfigData>> _configs = new Dictionary<int, List<DialogConfigData>>();

    public void Init()
    {
        Dictionary<int, DialogConfigData> configs = ConfigManager.Ins().GetConfig<DialogConfigData>(ConfigsFolderConfig.Null, ConfigsNameConfig.DialogConfig);

        foreach (var config in configs)
        {
            if (!_configs.ContainsKey(config.Key))
            {
                _configs.Add(config.Key, new List<DialogConfigData>());
            }

            _configs[config.Key].Add(config.Value);
        }

        int[] keys = _configs.Keys.ToArray();
        for (int i = 0; i < keys.Length; i++)
        {
            var list = _configs[keys[i]];
            for (int j = 0; j < list.Count; j++)
            {
                list[j].selection.Remove("null");
            }
        }
    }

    /// <summary>
    /// 获取对话
    /// </summary>
    /// <param name="dialogId"></param>
    /// <param name="stepId"></param>
    /// <returns></returns>
    public DialogConfigData GetDialog(int dialogId,int stepId)
    {
        List<DialogConfigData> list;
        _configs.TryGetValue(dialogId, out list);
        if(list != null)
        {
            if(stepId < list.Count)
            {
                return list[stepId];
            }
        }
        return null;
    }

    /// <summary>
    /// 下一对话
    /// </summary>
    /// <param name="cur"></param>
    /// <param name="selection"></param>
    /// <returns></returns>
    public DialogConfigData Next(DialogConfigData cur,int selection)
    {
        if (cur.selection.Count == 0)
        {
            if (cur.next.Count > 0)
            {
                return cur.next[0];
            }
            else
            {
                return null;
            }
        }
        else
        {
            return cur.next[selection];
        }
    }
}
