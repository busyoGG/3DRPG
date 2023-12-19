using System.Collections.Generic;
using UnityEditor;
using UnityEngine.ResourceManagement.ResourceProviders;

public class ResourceUtils
{
    /// <summary>
    /// ��Դ�����ֵ�
    /// </summary>
    private static Dictionary<string, object> _res = new Dictionary<string, object>();

    /// <summary>
    /// ���������ֵ�
    /// </summary>
    private static Dictionary<string, SceneInstance> _scene = new Dictionary<string, SceneInstance>();

    /// <summary>
    /// ������Դ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    public static void Load<T>(string url) where T : class
    {
#if UNITY_EDITOR
        object res = AssetDatabase.LoadAssetAtPath(url, typeof(T));
        ResourceCache(url, res);
#else
        AssetBundleUtils.LoadResourceAsync<T>(url, ResourceCache);
#endif
    }

    /// <summary>
    /// ��Դ���ػص�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="res"></param>
    private static void ResourceCache<T>(string url, T res)
    {
        if (!_res.ContainsKey(url))
        {
            _res.Add(url, res);
        }
    }

    /// <summary>
    /// ���س���
    /// </summary>
    /// <param name="url"></param>
    public static void LoadScene(string url)
    {
#if UNITY_EDITOR
        //SceneInstance res = UnityEngine.SceneManagement.SceneManager.GetSceneByPath(url);
        //SceneCache(url, res);
#else
        AssetBundleUtils.LoadScene(url, SceneCache);
#endif
    }

    /// <summary>
    /// ��Դ���ػص�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="res"></param>
    private static void SceneCache(string url, SceneInstance res)
    {
        if (!_scene.ContainsKey(url))
        {
            _scene.Add(url, res);
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    public static T GetRes<T>(string url) where T : class
    {
        object res;
        _res.TryGetValue(url, out res);
        if (res == null)
        {
            ConsoleUtils.Log("��Դ������:", url);
            return null;
        }
        return res as T;
    }

    /// <summary>
    /// ��ó���
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static SceneInstance GetScene(string url)
    {
        SceneInstance res;
        _scene.TryGetValue(url, out res);
        return res;
    }

    /// <summary>
    /// ��Դ�������
    /// </summary>
    public static void ResClear()
    {
        _res.Clear();
    }

    /// <summary>
    /// �����������
    /// </summary>
    public static void SceneClear()
    {
        _scene.Clear();
    }
}
