
using Bean;
using System;
using System.Collections;
using UnityEngine;

public class TriggerFuncInit : Singleton<TriggerFuncInit>
{
    DialogConfigData _dialogConfig;

    Entity _item;

    public TriggerBase GetTriggerFunc(TriggerFunction type)
    {
        TriggerBase baseClass = new TriggerBase();
        switch (type)
        {
            case TriggerFunction.Interactive:

                baseClass.OnTriggerEnter = (Entity self, Entity other) =>
                {
                    DialogComp dialog = self.Get<DialogComp>();

                    int randomId = UnityEngine.Random.Range(0, dialog.randomIds.Count - 1);

                    UIManager.Ins().Show<TriggerList>("TriggerList");
                    _dialogConfig = DialogManager.Ins().GetDialog(dialog.randomIds[randomId]);

                    EventManager.TriggerEvent("ChangeItem", new ArrayList()
                    {
                        true,
                        self.id,
                        _dialogConfig.target,
                        (Action)ShowDialog
                    });
                };

                baseClass.OnTriggerExit = (Entity self, Entity other) =>
                {
                    //UIManager.Ins().Hide(UIManager.Ins().Get<TriggerList>("TriggerList"));

                    EventManager.TriggerEvent("ChangeItem", new ArrayList()
                    {
                        false,
                        self.id
                    });
                };
                return baseClass;
            case TriggerFunction.Attack:
                baseClass.OnTriggerEnter = (Entity self, Entity other) =>
                {
                    AttackComp attackSelf = self.Get<AttackComp>();
                    AttackComp attackOther = other.Get<AttackComp>();

                    if (attackSelf.group != -1 && attackSelf.group != attackOther.group)
                    {
                        int damage = PropManager.Ins().GetDamage(attackOther.entityId);
                        PropManager.Ins().AddHp(attackSelf.entityId, -damage);
                        ConsoleUtils.Log(self.id, "被", other.id, "攻击", "属性值", damage);
                    }
                };
                break;
            case TriggerFunction.Collect:
                baseClass.OnTriggerEnter = (Entity self, Entity other) =>
                {
                    _item = self;
                    UIManager.Ins().Show<TriggerList>("TriggerList");
                    EventManager.TriggerEvent("ChangeItem", new ArrayList()
                    {
                        true,
                        self.id,
                        "测试道具0",
                        (Action)PickUpItem
                    });
                };

                baseClass.OnTriggerExit = (Entity self, Entity other) =>
                {
                    //UIManager.Ins().Hide(UIManager.Ins().Get<TriggerList>("TriggerList"));

                    EventManager.TriggerEvent("ChangeItem", new ArrayList()
                    {
                        false,
                        self.id
                    });
                };
                break;
        }
        return baseClass;
    }

    /// <summary>
    /// 展示对话
    /// </summary>
    /// <param name="config"></param>
    public void ShowDialog()
    {
        UIManager.Ins().Show<ConversationView>("ConversationView");
        EventManager.TriggerEvent("start_conversation", new ArrayList() { _dialogConfig });
    }

    private void PickUpItem()
    {
        ConsoleUtils.Log("捡起道具", _item.id);
        ECSManager.Ins().RemoveEntity(_item);
        _item = null;
        MissionManager.Ins().SetCompleteNum(2, 0, 1);
    }
}

