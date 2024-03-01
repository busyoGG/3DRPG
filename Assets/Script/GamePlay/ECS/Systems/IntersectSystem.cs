using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IntersectSystem : ECSSystem
{
    private Dictionary<int, BoxComp> _intersectObjs = new Dictionary<int, BoxComp>();
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(BoxComp), typeof(QTreeComp), typeof(TransformComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            BoxComp box = entity.Get<BoxComp>();
            if (!box.isPositive) continue;

            TransformComp transform = entity.Get<TransformComp>();

            //寻找相交物体
            LinkedList<(int, Entity)> intersectObjs = IntersectSingleton.Ins().GetIntersectObjs(entity.id);
            Dictionary<int, bool> intersectDic = IntersectSingleton.Ins().GetIntersectDic(entity.id);

            QTreeComp qtree = entity.Get<QTreeComp>();

            _intersectObjs.Clear();
            Vector3 point;

            foreach (var obj in qtree.foundObjs)
            {
                if (obj.entity.id == entity.id) continue;
                BoxComp boxComp = obj.entity.Get<BoxComp>();
                if (boxComp != null)
                {
                    bool isIntersect = CheckIntersect(box, boxComp, out point);
                    if (isIntersect)
                    {
                        if (!intersectDic.ContainsKey(obj.entity.id))
                        {
                            intersectObjs.AddLast((obj.entity.id, obj.entity));
                            intersectDic.Add(obj.entity.id, true);
                        }
                        _intersectObjs.Add(obj.entity.id, boxComp);
                    }
                }
            }

            //删除多余相交物体
            foreach(var intersectObj in intersectObjs.ToList())
            {
                if (!_intersectObjs.ContainsKey(intersectObj.Item1))
                {
                    intersectObjs.Remove(intersectObj);
                    intersectDic.Remove(intersectObj.Item1);
                }
            }
        }
    }

    public bool CheckIntersect(BoxComp box1, BoxComp box2, out Vector3 point)
    {
        switch (box1.type)
        {
            case CollisionType.AABB:
                return SubCheckIntersect(box1.aabb, box2, out point);
            case CollisionType.OBB:
                return SubCheckIntersect(box1.obb, box2, out point);
            case CollisionType.Circle:
                return SubCheckIntersect(box1.circle, box2, out point);
            case CollisionType.Ray:
                return SubCheckIntersect(box1.ray, box2, out point);
        }
        point = CollideUtils._minValue;
        return false;
    }

    public bool SubCheckIntersect(ICollide data1, BoxComp box2, out Vector3 point)
    {
        switch (box2.type)
        {
            case CollisionType.AABB:
                //return data1.Interactive(box2.aabb, out point);
                return data1.Interactive(box2.aabb, out point);
            case CollisionType.OBB:
                return data1.Interactive(box2.obb, out point);
            case CollisionType.Circle:
                return data1.Interactive(box2.circle, out point);
            case CollisionType.Ray:
                return data1.Interactive(box2.ray, out point);
        }
        point = CollideUtils._minValue;
        return false;
    }
}
