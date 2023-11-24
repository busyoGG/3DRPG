using UnityEngine;
[CompRegister(typeof(QTreeComp))]
public class QTreeComp : Comp
{
    public QNode qNode;

    public QTreeObj qObj;

    public override void Reset()
    {
        qNode = null;
        qObj = null;
    }
}
