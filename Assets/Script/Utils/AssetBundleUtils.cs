using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class AssetBundleUtils
{
    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="url"></param>
    public static void LoadResourceAsync<T>(string url, Action<string, T> callback)
    {
        Addressables.LoadAssetAsync<T>(url).Completed += res => OnCompleted(res, url, callback);
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="url"></param>
    public static void LoadScene(string url, Action<string, SceneInstance> callback)
    {
        Addressables.LoadSceneAsync(url).Completed += res => OnCompleted(res, url, callback);
    }

    /// <summary>
    /// 资源加载完成回调
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="url"></param>
    private static void OnCompleted<T>(AsyncOperationHandle<T> handle, string url, Action<string, T> callback)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            callback.Invoke(url, handle.Result);
        }
        else
        {
            ConsoleUtils.Log("加载失败");
        }

        Addressables.Release(handle);
    }
}
