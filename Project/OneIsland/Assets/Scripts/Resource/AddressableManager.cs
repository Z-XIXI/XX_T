using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class AddressableManager : MonoBehaviour
{
    // 单例模式
    private static AddressableManager _instance;
    public static AddressableManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindFirstObjectByType<AddressableManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("AddressableManager");
                    _instance = obj.AddComponent<AddressableManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 同步加载资源（不推荐，但在某些情况下可能需要）
    public T LoadAssetSync<T>(string address)
    {
        var handle = Addressables.LoadAssetAsync<T>(address);
        handle.WaitForCompletion(); // 阻塞直到加载完成

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            T result = handle.Result;
            // 注意：这里不释放handle，因为我们需要保持资源加载
            return result;
        }
        else
        {
            Debug.LogError($"Failed to load asset at address: {address}");
            Addressables.Release(handle);
            return default;
        }
    }

    // 异步加载资源（推荐）
    public async Task<T> LoadAssetAsync<T>(string address)
    {
        var handle = Addressables.LoadAssetAsync<T>(address);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }
        else
        {
            Debug.LogError($"Failed to load asset at address: {address}");
            Addressables.Release(handle);
            return default;
        }
    }

    // 实例化游戏对象（异步）
    public async Task<GameObject> InstantiateAsync(string address, Transform parent = null, bool instantiateInWorldSpace = false)
    {
        var handle = Addressables.InstantiateAsync(address, parent, instantiateInWorldSpace);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }
        else
        {
            Debug.LogError($"Failed to instantiate asset at address: {address}");
            Addressables.Release(handle);
            return null;
        }
    }

    // 释放资源
    public void ReleaseAsset<T>(T asset)
    {
        Addressables.Release(asset);
    }

    // 销毁实例化的游戏对象
    public void ReleaseInstance(GameObject instance)
    {
        Addressables.ReleaseInstance(instance);
    }

    // 检查资源是否存在
    public async Task<bool> CheckIfAssetExists(string address)
    {
        var handle = Addressables.LoadResourceLocationsAsync(address);
        await handle.Task;

        bool exists = handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null && handle.Result.Count > 0;
        Addressables.Release(handle);
        return exists;
    }
}