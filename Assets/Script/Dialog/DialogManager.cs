using Bean;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogManager : Singleton<DialogManager>
{
    public Dictionary<int, DialogConfigData> _configs = new Dictionary<int, DialogConfigData>();

    public void Init()
    {
        _configs = ConfigManager.Ins().GetConfig<DialogConfigData>(ConfigsFolderConfig.Configs, ConfigsNameConfig.DialogConfig);
        int[] keys = _configs.Keys.ToArray();
        for (int i = 0; i < keys.Length; i++)
        {
            _configs[keys[i]].selection.Remove("null");
        }
    }

    public void GetDialog(int id)
    {
        return _configs[id];
    }
}
