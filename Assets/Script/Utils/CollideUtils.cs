
using Game;
using System.Collections.Generic;
using UnityEngine;

public class CollideUtils
{
    public static Vector3 _minValue = new Vector3(float.MinValue, float.MinValue, float.MinValue);

    public static Dictionary<string, Vector3[]> _seperatingAxes = new Dictionary<string, Vector3[]>();

    public static Dictionary<string, List<Vector2[]>> _limitObb = new Dictionary<string, List<Vector2[]>>();

    //----- AABB ----- start

    /// <summary>
    /// AABB���
    /// </summary>
    /// <param name="data1">AABB</param>
    /// <param name="data2">AABB</param>
    /// <returns></returns>
    public static bool CollisionAABB(AABBData data1, AABBData data2)
    {
        //��Χ��1����Сֵ�Ȱ�Χ��2�����ֵ���� �� ��Χ��1�����ֵ�Ȱ�Χ��2����Сֵ��С ����ײ
        if (data1.max.x < data2.min.x || data1.max.y < data2.min.y || data1.max.z < data2.min.z ||
            data1.min.x > data2.max.x || data1.min.y > data2.max.y || data1.min.z > data2.max.z)
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
    /// SAT��������ײ���֮OBB���
    /// </summary>
    /// <param name="data1">OBB</param>
    /// <param name="data2">OBB</param>
    /// <returns></returns>
    public static bool CollisionOBB(OBBData data1, OBBData data2)
    {
        //������OBB��Χ��֮������������ķ�ƽ���� ��9��
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
                //��һ�����ཻ���˳�
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
    /// ����ͶӰ�Ƿ��ཻ
    /// </summary>
    /// <param name="vertexs1"></param>
    /// <param name="vertexs2"></param>
    /// <param name="axis"></param>
    /// <returns></returns>
    public static bool NotInteractiveOBB(Vector3[] vertexs1, Vector3[] vertexs2, Vector3 axis, out Vector2[] limit)
    {
        limit = new Vector2[2];
        //����OBB��Χ���ڷ������ϵ�ͶӰ����ֵ
        limit[0] = GetProjectionLimit(vertexs1, axis);
        limit[1] = GetProjectionLimit(vertexs2, axis);
        //������Χ�м���ֵ���ཻ������ײ
        bool res = limit[0].x > limit[1].y || limit[1].x > limit[0].y;
        return res;
    }

    /// <summary>
    /// ���㶥��ͶӰ����ֵ
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
    /// ��������
    /// </summary>
    /// <param name="data1">��</param>
    /// <param name="data2">��</param>
    /// <returns></returns>
    public static bool CollisionCircle(CircleData data1, CircleData data2)
    {
        //��������뾶��
        float totalRadius = Mathf.Pow(data1.radius + data2.radius, 2);
        //����������֮��ľ���
        float distance = (data1.position - data2.position).sqrMagnitude;
        //����С�ڵ��ڰ뾶������ײ
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
    /// ����AABB���
    /// </summary>
    /// <param name="data1">��</param>
    /// <param name="data2">AABB</param>
    /// <returns></returns>
    public static bool CollisionCircle2AABB(CircleData data1, AABBData data2, out Vector3 point)
    {
        //��������
        Vector3 center = data1.position;
        Vector3 nearP = GetClosestPoint(data1.position, data2);
        //�������������ĵľ���
        float distance = (nearP - center).sqrMagnitude;
        float radius = Mathf.Pow(data1.radius, 2);
        //����С�ڰ뾶����ײ
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
    /// ���һ�㵽AABB�����
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
    /// ����OBB���
    /// </summary>
    /// <param name="data1">��</param>
    /// <param name="data2">OBB</param>
    /// <returns></returns>
    public static bool CollisionCircle2OBB(CircleData data1, OBBData data2, out Vector3 point)
    {
        //�������
        Vector3 nearP = GetClosestPoint(data1.position, data2);
        //��AABB���ԭ����ͬ
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
    /// ��ȡһ�㵽OBB�������
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetClosestPoint(Vector3 point, OBBData data2, bool isWorldPosition = true)
    {
        Vector3 nearP = data2.position;
        //��������OBB���ĵľ������� ��OBB����ָ������
        Vector3 center1 = point;
        Vector3 center2 = data2.position;
        Vector3 dist = center1 - center2;

        float[] size = new float[3] { data2.halfSize.x, data2.halfSize.y, data2.halfSize.z };
        Vector3[] axes = data2.axes;

        for (int i = 0; i < 3; i++)
        {
            //�������������OBB�������ͶӰ���� ������������OBB����ϵ�еĶ�Ӧ������ĳ���
            float distance = Vector3.Dot(dist, axes[i]);
            distance = Mathf.Clamp(distance, -size[i], size[i]);
            //��ԭ����������
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
    /// ���ߺ�����
    /// </summary>
    public static bool CollisionRay2Circle(RayData data1, CircleData data2, out Vector3 point)
    {

        Vector3 centerDis = data2.position - data1.origin;
        Vector3 forward = data1.forward;

        float projection = Vector3.Dot(centerDis, forward);
        float r2 = Mathf.Pow(data2.radius, 2);
        float f = Mathf.Pow(projection, 2) + r2 - centerDis.sqrMagnitude;

        //�����෴
        bool checkforward = projection < 0;
        //���߹���
        bool checkDistance = centerDis.sqrMagnitude > Mathf.Pow(data1.length + data2.radius, 2);
        //������������ڲ�
        bool checkNotInside = centerDis.sqrMagnitude > r2;
        //���ཻ
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
        //�ж��Ƿ���AABB��
        bool checkNotInside = CheckInsideAABB(data1.origin, data2);
        //�жϷ������
        bool checkForawd = Vector3.Dot(data2.position - data1.origin, data1.forward) < 0;
        if (checkNotInside && checkForawd)
        {
            point = _minValue;
            return false;
        }

        //�ж��Ƿ��ཻ
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
    /// ���һ�����Ƿ���AABB��
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
        //�жϲ���OBB��
        Vector3 centerDis = data1.origin - data2.position;
        bool checkNotInside = CheckInsideObb(centerDis, data2);
        //�жϷ������
        bool checkFoward = Vector3.Dot(data2.position - data1.origin, data1.forward) < 0;
        if (checkNotInside && checkFoward)
        {
            point = _minValue;
            return false;
        }

        //�ж��Ƿ��ཻ
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
    /// ���һ�����Ƿ���OBB��
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

    //----- ��ȡ��ײ���� -----

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
                    //��һ�����ཻ���˳�
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
            if (overlap > 0)
            {

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

        len = minOverlap * 2;
        //len = minOverlap;
        Vector3 dis = data1.position - data2.position;
        float amount = normal.x * dis.x + normal.y * dis.y + normal.z * dis.z;
        if (amount < 0)
        {
            normal = -normal;
        }
        if (len > 0.5f)
        {
            ConsoleUtils.Log("����");
        }
        return normal;
    }

    //TODO GJK���

    //���ߺ���

    public static void Swap(ref float one, ref float two)
    {
        float temp;
        temp = one;
        one = two;
        two = temp;
    }
}
