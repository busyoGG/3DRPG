
using Bean;
using System;
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

                //int dialogId = 0;
                DialogConfigData config = DialogManager.Ins().GetDialog(1, 0);

                Action action = () =>
                {
                    UIManager.Ins().Show<ConversationView>("ConversationView");
                    EventManager.TriggerEvent("start_conversation", new ArrayList() { config });
                };

                baseClass.OnTriggerEnter = (Entity self, Entity other) =>
                {
                    UIManager.Ins().Show<TriggerList>("TriggerList");

                    EventManager.TriggerEvent("ChangeItem", new ArrayList()
                    {
                        true,
                        action,
                        config.target
                    });
                };

                baseClass.OnTriggerExit = (Entity self, Entity other) =>
                {
                    //UIManager.Ins().Hide(UIManager.Ins().Get<TriggerList>("TriggerList"));

                    EventManager.TriggerEvent("ChangeItem", new ArrayList()
                    {
                        false,
                        action
                    });
                };
                return baseClass;
            case TriggerFunction.Attack:
                baseClass.OnTriggerEnter = (Entity self, Entity other) =>
                {
                    AttackComp attackSelf = self.Get<AttackComp>();
                    AttackComp attackOther = other.Get<AttackComp>();
                    //bool isAttack = other.Has<AttackComp>();
                    //PropComp prop = self.Get<PropComp>();
                    PropData prop = AttackSingleton.Ins().GetPropData(attackSelf.entityId);

                    if (attackSelf.group != -1 && attackSelf.group != attackOther.group)
                    {
                        ConsoleUtils.Log(self.id, "±ª", other.id, "π•ª˜"," Ù–‘÷µ", prop);
                    }
                };
                break;
        }
        return baseClass;
    }
}
