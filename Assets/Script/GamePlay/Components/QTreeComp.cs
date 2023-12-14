using System.Collections.Generic;
using UnityEngine;
[CompRegister(typeof(QTreeComp))]
public class QTreeComp : Comp
{
    public QNode qNode;

    public QTreeObj qObj;

    public List<QTreeObj> foundObjs = new List<QTreeObj>();

    public override void Reset()
    {
        qNode = null;
        qObj = null;
        foundObjs.Clear();
    }
}
