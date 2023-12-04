
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFuncInit : Singleton<TriggerFuncInit>
{
    public TriggerBase GetTriggerFunc(TriggerFunction type)
    {
        TriggerBase baseClass = new TriggerBase();
        switch (type)
        {
            case TriggerFunction.Interactive:
                baseClass.OnTriggerEnter = (Entity self, Entity other) =>
                {
                    UIManager.Ins().Show<TriggerList>("TriggerList");
                };

                baseClass.OnTriggerExit = (Entity self, Entity other) =>
                {
                    UIManager.Ins().Hide(UIManager.Ins().Get<TriggerList>("TriggerList"));
                };
                return baseClass;
        }
        return baseClass;
    }
}
