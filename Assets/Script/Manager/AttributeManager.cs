using FairyGUI;
using System;
using System.Reflection;
using UnityEngine;

public class AttributeManager : Singleton<AttributeManager>
{
    public void Init()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(ECSManager));
        Type[] types = assembly.GetTypes();
        foreach (var type in types)
        {
            //��ȡ�Զ������ԵĹ������Զ�ִ�й��캯��
            var attrs = type.GetCustomAttributes(true);
            foreach (var attr in attrs)
            {
                //��ʼ���������
                if (attr is CompRegister)
                {
                    ECSManager.Ins().SetTotalCompNum(ECSManager.Ins().GetTotalCompNum() + 1);
                }
            }

            //var props = type.GetProperties();
            //foreach (var prop in props)
            //{
            //    var propAttrs = prop.GetCustomAttributes(true);
            //    foreach (var attr in propAttrs)
            //    {
            //        if (attr is UIBind)
            //        {
            //            GComponent main = null;

            //            if(type is BaseView)
            //            {
            //                main = (BaseView)type.GetField("main").GetValue();
            //            }
            //            else
            //            {

            //            }

            //            UIBind uiBind = (UIBind)attr;
            //            switch (uiBind._type)
            //            {
            //                case "text":

            //                    break;
            //            }
            //        }
            //    }
            //}
        }
    }
}
