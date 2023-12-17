
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
                    //bool isAttack = other.Has<AttackComp>();
                    //PropComp prop = self.Get<PropComp>();

                    if (attackSelf.group != -1 && attackSelf.group != attackOther.group)
                    {
                        PropData prop = AttackSingleton.Ins().GetPropData(attackSelf.entityId);
                        ConsoleUtils.Log(self.id, "��", other.id, "����", "����ֵ", prop);
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
                        "���Ե���0",
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
    /// չʾ�Ի�
    /// </summary>
    /// <param name="config"></param>
    public void ShowDialog()
    {
        UIManager.Ins().Show<ConversationView>("ConversationView");
        EventManager.TriggerEvent("start_conversation", new ArrayList() { _dialogConfig });
    }

    private void PickUpItem()
    {
        ConsoleUtils.Log("�������", _item.id);
        ECSManager.Ins().RemoveEntity(_item);
        _item = null;
        MissionManager.Ins().SetCompleteNum(2, 0, 1);
    }
}

