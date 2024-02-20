

using System.Collections.Generic;
using UnityEngine;

public class CollideUtils
{
    public static Vector3 _minValue = new Vector3(float.MinValue, float.MinValue, float.MinValue);
    public static Vector3 _maxValue = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

    public static Dictionary<string, Vector3[]> _seperatingAxes = new Dictionary<string, Vector3[]>();

    public static Dictionary<string, List<Vector2[]>> _limitObb = new Dictionary<string, List<Vector2[]>>();

    //----- AABB ----- start

    /// <summary>
    /// AABB检测
    /// </summary>
    /// <param name="data1">AABB</param>
    /// <param name="data2">AABB</param>
    /// <returns></returns>
    public static bool CollisionAABB(AABBData data1, AABBData data2)
    {
        //包围盒1的最小值比包围盒2的最大值还大 或 包围盒1的最大值比包围盒2的最小值还小 则不碰撞
        if (data2.min.x - data1.max.x > 0.001 || data2.min.y - data1.max.y > 0.001 || data2.min.z - data1.max.z > 0.001 ||
            data1.min.x - data2.max.x > 0.001 || data1.min.y - data2.max.y > 0.001 || data1.min.z - data2.max.z >  0.001)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //----- AABB ----- end

    //----- OBB ----- start

    /// <summary>
    /// SAT分离轴碰撞检测之OBB检测
    /// </summary>
    /// <param name="data1">OBB</param>
    /// <param name="data2">OBB</param>
    /// <returns></returns>
    public static bool CollisionOBB(OBBData data1, OBBData data2)
    {
        //求两个OBB包围盒之间两两坐标轴的法平面轴 共9个
        Vector3[] axes;

        string id = data1.GetHashCode() + "-" + data2.GetHashCode();
        _seperatingAxes.TryGetValue(id, out axes);
        if (axes == null)
        {
            int len1 = data1.axes.Length;
            int len2 = data2.axes.Length;
            axes = new Vector3[len1 + len2 + len1 * len2];
            int k = 0;
            int initJ = len2;
            for (int i = 0; i < len1; i++)
            {
                axes[k++] = data1.axes[i];
                for (int j = 0; j < len2; j++)
                {
                    if (initJ > 0)
                    {
                        initJ--;
                        axes[k++] = data2.axes[j];
                    }
                    axes[k++] = Vector3.Cross(data1.axes[i], data2.axes[j]);
                }
            }
            _seperatingAxes.Add(id, axes);
        }

        List<Vector2[]> limitList;
        _limitObb.TryGetValue(id, out limitList);
        if (limitList == null)
        {
            limitList = new List<Vector2[]>();
            _limitObb.Add(id, limitList);
        }
        else
        {
            limitList.Clear();
        }


        for (int i = 0, len = axes.Length; i < len; i++)
        {
            Vector2[] limit;
            bool isNotInteractive = NotInteractiveOBB(data1.vertexts, data2.vertexts, axes[i], out limit);

            if (isNotInteractive)
            {
                //有一个不相交就退出
                return false;
            }
            else
            {
                limitList.Add(limit);
            }
        }

        return true;
    }

    /// <summary>
    /// 计算投影是否不相交
    /// </summary>
    /// <param name="vertexs1"></param>
    /// <param name="vertexs2"></param>
    /// <param name="axis"></param>
    /// <returns></returns>
    public static bool NotInteractiveOBB(Vector3[] vertexs1, Vector3[] vertexs2, Vector3 axis, out Vector2[] limit)
    {
        limit = new Vector2[2];
        //计算OBB包围盒在分离轴上的投影极限值
        limit[0] = GetProjectionLimit(vertexs1, axis);
        limit[1] = GetProjectionLimit(vertexs2, axis);
        //两个包围盒极限值不相交，则不碰撞
        bool res = limit[0].x - limit[1].y >= 0.001 || limit[1].x - limit[0].y >= 0.001;
        return res;
    }

    /// <summary>
    /// 计算顶点投影极限值
    /// </summary>
    /// <param name="vertexts"></param>
    /// <param name="axis"></param>
    /// <returns></returns>
    public static Vector2 GetProjectionLimit(Vector3[] vertexts, Vector3 axis)
    {
        Vector2 result = new Vector2(float.MaxValue, float.MinValue);
        for (int i = 0, len = vertexts.Length; i < len; i++)
        {
            Vector3 vertext = vertexts[i];
            float dot = Vector3.Dot(vertext, axis);
            result.x = Mathf.Min(dot, result[0]);
            result.y = Mathf.Max(dot, result[1]);
        }
        return result;
    }
    //----- OBB ----- end

    //----- Circle ----- start
    /// <summary>
    /// 球与球检测
    /// </summary>
    /// <param name="data1">球</param>
    /// <param name="data2">球</param>
    /// <returns></returns>
    public static bool CollisionCircle(CircleData data1, CircleData data2)
    {
        //求两个球半径和
        float totalRadius = Mathf.Pow(data1.radius + data2.radius, 2);
        //球两个球心之间的距离
        float distance = (data1.position - data2.position).sqrMagnitude;
        //距离小于等于半径和则碰撞
        if (distance <= totalRadius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 球与AABB检测
    /// </summary>
    /// <param name="data1">球</param>
    /// <param name="data2">AABB</param>
    /// <returns></returns>
    public static bool CollisionCircle2AABB(CircleData data1, AABBData data2, out Vector3 point)
    {
        //求出最近点
        Vector3 center = data1.position;
        Vector3 nearP = GetClosestPoint(data1.position, data2);
        //求出最近点与球心的距离
        float distance = (nearP - center).sqrMagnitude;
        float radius = Mathf.Pow(data1.radius, 2);
        //距离小于半径则碰撞
        if (distance <= radius)
        {
            point = nearP;
            return true;
        }
        else
        {
            point = _minValue;
            return false;
        }
    }

    /// <summary>
    /// 获得一点到AABB最近点
    /// </summary>
    /// <param name="data1"></param>
    /// <param name="data2"></param>
    /// <returns></returns>
    public static Vector3 GetClosestPoint(Vector3 point, AABBData data2)
    {
        Vector3 nearP = Vector3.zero;
        nearP.x = Mathf.Clamp(point.x, data2.min.x, data2.max.x);
        nearP.y = Mathf.Clamp(point.y, data2.min.y, data2.max.y);
        nearP.z = Mathf.Clamp(point.z, data2.min.z, data2.max.z);
        return nearP;
    }

    /// <summary>
    /// 球与OBB检测
    /// </summary>
    /// <param name="data1">球</param>
    /// <param name="data2">OBB</param>
    /// <returns></returns>
    public static bool CollisionCircle2OBB(CircleData data1, OBBData data2, out Vector3 point)
    {
        //求最近点
        Vector3 nearP = GetClosestPoint(data1.position, data2);
        //与AABB检测原理相同
        float distance = (nearP - data1.position).sqrMagnitude;
        float radius = Mathf.Pow(data1.radius, 2);
        if (distance <= radius)
        {
            point = nearP;
            return true;
        }
        else
        {
            point = _minValue;
            return false;
        }
    }

    /// <summary>
    /// 获取一点到OBB的最近点
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetClosestPoint(Vector3 point, OBBData data2, bool isWorldPosition = true)
    {
        Vector3 nearP = data2.position;
        //求球心与OBB中心的距离向量 从OBB中心指向球心
        Vector3 center1 = point;
        Vector3 center2 = data2.position;
        Vector3 dist = center1 - center2;

        float[] size = new float[3] { data2.halfSize.x, data2.halfSize.y, data2.halfSize.z };
        Vector3[] axes = data2.axes;

        for (int i = 0; i < 3; i++)
        {
            //计算距离向量到OBB坐标轴的投影长度 即距离向量在OBB坐标系中的对应坐标轴的长度
            float distance = Vector3.Dot(dist, axes[i]);
            distance = Mathf.Clamp(distance, -size[i], size[i]);
            //还原到世界坐标
            //nearP.x += distance * axes[i].x;
            //nearP.y += distance * axes[i].y;
            //nearP.z += distance * axes[i].z;
            if (isWorldPosition)
            {
                nearP += distance * axes[i];
            }
            else
            {
                switch (i)
                {
                    case 0:
                        nearP.x = Mathf.Clamp(distance, -size[i], size[i]);
                        break;
                    case 1:
                        nearP.y = Mathf.Clamp(distance, -size[i], size[i]);
                        break;
                    case 2:
                        nearP.z = Mathf.Clamp(distance, -size[i], size[i]);
                        break;
                }
            }
        }
        return nearP;
    }

    //----- Circle ----- end

    //----- Ray ----- start

    /// <summary>
    /// 射线和球检测
    /// </summary>
    public static bool CollisionRay2Circle(RayData data1, CircleData data2, out Vector3 point)
    {

        Vector3 centerDis = data2.position - data1.origin;
        Vector3 forward = data1.forward;

        float projection = Vector3.Dot(centerDis, forward);
        float r2 = Mathf.Pow(data2.radius, 2);
        float f = Mathf.Pow(projection, 2) + r2 - centerDis.sqrMagnitude;

        //方向相反
        bool checkforward = projection < 0;
        //射线过短
        bool checkDistance = centerDis.sqrMagnitude > Mathf.Pow(data1.length + data2.radius, 2);
        //射线起点在球内部
        bool checkNotInside = centerDis.sqrMagnitude > r2;
        //不相交
        bool checkNotCollide = f < 0;

        if (checkNotInside && (checkforward || checkDistance || checkNotCollide))
        {
            point = _minValue;
            return false;
        }

        float dis = projection - Mathf.Sqrt(f) * (checkNotInside ? 1 : -1);
        point = data1.origin + data1.forward * dis;

        return true;
    }

    public static bool CollisionRay2AABB(RayData data1, AABBData data2, out Vector3 point)
    {
        //判断是否不在AABB内
        bool checkNotInside = CheckInsideAABB(data1.origin, data2);
        //判断反向情况
        bool checkForawd = Vector3.Dot(data2.position - data1.origin, data1.forward) < 0;
        if (checkNotInside && checkForawd)
        {
            point = _minValue;
            return false;
        }

        //判断是否相交
        Vector3 min = data2.min - data1.origin;
        Vector3 max = data2.max - data1.origin;

        Vector3 projection = new Vector3(1 / data1.forward.x, 1 / data1.forward.y, 1 / data1.forward.z);

        Vector3 pMin = Vector3.Scale(min, projection);
        Vector3 pMax = Vector3.Scale(max, projection);

        if (data1.forward.x < 0) Swap(ref pMin.x, ref pMax.x);
        if (data1.forward.y < 0) Swap(ref pMin.y, ref pMax.y);
        if (data1.forward.z < 0) Swap(ref pMin.z, ref pMax.z);

        float n = Mathf.Max(pMin.x, pMin.y, pMin.z);
        float f = Mathf.Min(pMax.x, pMax.y, pMax.z);

        if (!checkNotInside)
        {

            point = data1.origin + data1.forward * f;

            return true;
        }
        else
        {
            if (n < f && data1.length >= n)
            {
                point = data1.origin + data1.forward * n;
                return true;
            }
            else
            {
                point = _minValue;
                return false;
            }
        }
    }

    /// <summary>
    /// 检测一个点是否在AABB内
    /// </summary>
    /// <param name="point"></param>
    /// <param name="data2"></param>
    /// <returns></returns>
    public static bool CheckInsideAABB(Vector3 point, AABBData data2)
    {
        bool notInside = point.x > data2.max.x || point.x < data2.min.x ||
             point.y > data2.max.y || point.y < data2.min.y ||
             point.z > data2.max.z || point.z < data2.min.z;
        return !notInside;
    }

    public static bool CollisionRay2OBB(RayData data1, OBBData data2, out Vector3 point)
    {
        //判断不在OBB内
        Vector3 centerDis = data1.origin - data2.position;
        bool checkNotInside = CheckInsideObb(centerDis, data2);
        //判断反向情况
        bool checkFoward = Vector3.Dot(data2.position - data1.origin, data1.forward) < 0;
        if (checkNotInside && checkFoward)
        {
            point = _minValue;
            return false;
        }

        //判断是否相交
        Vector3 min = Vector3.zero;
        Vector3 minP = data2.vertexts[0] - data1.origin;
        min.x = Vector3.Dot(minP, data2.axes[0]);
        min.y = Vector3.Dot(minP, data2.axes[1]);
        min.z = Vector3.Dot(minP, data2.axes[2]);

        Vector3 max = Vector3.zero;
        Vector3 maxP = data2.vertexts[7] - data1.origin;
        max.x = Vector3.Dot(maxP, data2.axes[0]);
        max.y = Vector3.Dot(maxP, data2.axes[1]);
        max.z = Vector3.Dot(maxP, data2.axes[2]);


        Vector3 projection = Vector3.zero;
        projection.x = 1 / Vector3.Dot(data1.forward, data2.axes[0]);
        projection.y = 1 / Vector3.Dot(data1.forward, data2.axes[1]);
        projection.z = 1 / Vector3.Dot(data1.forward, data2.axes[2]);

        Vector3 pMin = Vector3.Scale(min, projection);
        Vector3 pMax = Vector3.Scale(max, projection);

        if (projection.x < 0) Swap(ref pMin.x, ref pMax.x);
        if (projection.y < 0) Swap(ref pMin.y, ref pMax.y);
        if (projection.z < 0) Swap(ref pMin.z, ref pMax.z);


        float n = Mathf.Max(pMin.x, pMin.y, pMin.z);
        float f = Mathf.Min(pMax.x, pMax.y, pMax.z);

        //Debug.Log(n + " " + f);
        //Debug.Log(pMin + " " + pMax);
        //Debug.Log(projection);

        if (!checkNotInside)
        {
            point = data1.origin + data1.forward * f;
            return true;
        }
        else
        {
            if (n < f && data1.length >= n)
            {
                point = data1.origin + data1.forward * n;
                return true;
            }
            else
            {
                point = _minValue;
                return false;
            }
        }
    }

    /// <summary>
    /// 检测一个点是否在OBB内
    /// </summary>
    /// <param name="point"></param>
    /// <param name="data2"></param>
    /// <returns></returns>
    public static bool CheckInsideObb(Vector3 point, OBBData data2)
    {
        float ray2ObbX = Vector3.Dot(point, data2.axes[0]);
        float ray2ObbY = Vector3.Dot(point, data2.axes[1]);
        float ray2ObbZ = Vector3.Dot(point, data2.axes[2]);
        bool notInside = ray2ObbX < -data2.halfSize[0] || ray2ObbX > data2.halfSize[0] ||
            ray2ObbY < -data2.halfSize[1] || ray2ObbY > data2.halfSize[1] ||
            ray2ObbZ < -data2.halfSize[2] || ray2ObbZ > data2.halfSize[2];
        return !notInside;
    }

    //----- Ray ----- end

    //----- 获取碰撞法线 -----

    public static Vector3 GetCollideNormal(AABBData data1, AABBData data2, out float len)
    {
        Vector3 normal = Vector3.zero;
        Vector3 len1_2 = data1.max - data2.min;
        Vector3 len2_1 = data2.max - data1.min;

        float[] depth = new float[6] {
            len1_2.x,
            len1_2.y,
            len1_2.z,
            len2_1.x,
            len2_1.y,
            len2_1.z
        };

        float min = depth[0];
        List<int> index = new List<int>() { 0 };
        for (int i = 1; i < depth.Length; i++)
        {
            if (depth[i] < min)
            {
                min = depth[i];
                if (index.Count > 1)
                {
                    index.Clear();
                }

                if (index.Count == 0)
                {
                    index.Add(i);
                }
                else
                {
                    index[0] = i;
                }
            }
            else if (depth[i] == min)
            {
                index.Add(i);
            }
        }

        len = min;

        for (int i = 0; i < index.Count; i++)
        {
            switch (index[i])
            {
                case 0:
                    normal.x = -1;
                    break;
                case 1:
                    normal.y = -1;
                    break;
                case 2:
                    normal.z = -1;
                    break;
                case 3:
                    normal.x = 1;
                    break;
                case 4:
                    normal.y = 1;
                    break;
                case 5:
                    normal.z = 1;
                    break;
            }
        }
        return normal.normalized;
    }

    public static Vector3 GetCollideNormal(OBBData data1, OBBData data2, out float len)
    {
        string id = data1.GetHashCode() + "-" + data2.GetHashCode();
        Vector3 normal = Vector3.zero;

        Vector3[] axes;
        _seperatingAxes.TryGetValue(id, out axes);
        List<Vector2[]> limitList;
        _limitObb.TryGetValue(id, out limitList);

        if (axes == null)
        {
            int len1 = data1.axes.Length;
            int len2 = data2.axes.Length;
            axes = new Vector3[len1 + len2 + len1 * len2];
            int k = 0;
            int initJ = len2;
            for (int i = 0; i < len1; i++)
            {
                axes[k++] = data1.axes[i];
                for (int j = 0; j < len2; j++)
                {
                    if (initJ > 0)
                    {
                        initJ--;
                        axes[k++] = data2.axes[j];
                    }
                    axes[k++] = Vector3.Cross(data1.axes[i], data2.axes[j]);
                }
            }
            _seperatingAxes.Add(id, axes);
        }

        if (limitList == null || limitList.Count != axes.Length)
        {
            if (limitList == null)
            {
                limitList = new List<Vector2[]>();
                _limitObb.Add(id, limitList);
            }
            else
            {
                limitList.Clear();
            }

            for (int i = 0; i < axes.Length; i++)
            {
                Vector2[] limit;
                bool isNotInteractive = NotInteractiveOBB(data1.vertexts, data2.vertexts, axes[i], out limit);

                if (isNotInteractive)
                {
                    //有一个不相交就退出
                    len = 0;
                    return normal;
                }
                else
                {
                    limitList.Add(limit);
                }
            }
        }

        float minOverlap = float.MaxValue;

        for (int i = 0; i < limitList.Count; i++)
        {
            if (axes[i].x == 0 && axes[i].y == 0 && axes[i].z == 0) continue;
            Vector2[] limit = limitList[i];
            float overlap;
            if (limit[0].y > limit[1].y && limit[0].x < limit[1].x)
            {
                overlap = Mathf.Min(limit[0].y - limit[1].x, limit[1].y - limit[0].x);
            }
            else if (limit[1].y > limit[0].y && limit[1].x < limit[0].x)
            {
                overlap = Mathf.Min(limit[1].y - limit[0].x, limit[0].y - limit[1].x);
            }
            else
            {
                overlap = Mathf.Min(limit[0].y, limit[1].y) - Mathf.Max(limit[0].x, limit[1].x);
            }
            if (overlap >= -0.001)
            {
                overlap = overlap / axes[i].magnitude;
                if (overlap < minOverlap)
                {
                    minOverlap = overlap;
                    normal = axes[i];
                }
            }
            else
            {
                len = 0;
                limitList.Clear();
                return normal;
            }
        }

        len = minOverlap;
        //len = minOverlap;
        Vector3 dis = data1.position - data2.position;
        float amount = normal.x * dis.x + normal.y * dis.y + normal.z * dis.z;
        if (amount < 0)
        {
            normal = -normal;
        }
        if (len > 0.5f)
        {
            ConsoleUtils.Log("超长", normal,len);
        }
        return normal.normalized;
    }

    //TODO GJK检测

    //工具函数

    public static void Swap(ref float one, ref float two)
    {
        float temp;
        temp = one;
        one = two;
        two = temp;
    }
}
