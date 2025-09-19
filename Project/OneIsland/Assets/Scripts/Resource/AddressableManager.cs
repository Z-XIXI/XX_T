using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class AddressableManager : MonoBehaviour
{
    // ����ģʽ
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

    // ͬ��������Դ�����Ƽ�������ĳЩ����¿�����Ҫ��
    public T LoadAssetSync<T>(string address)
    {
        var handle = Addressables.LoadAssetAsync<T>(address);
        handle.WaitForCompletion(); // ����ֱ���������

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            T result = handle.Result;
            // ע�⣺���ﲻ�ͷ�handle����Ϊ������Ҫ������Դ����
            return result;
        }
        else
        {
            Debug.LogError($"Failed to load asset at address: {address}");
            Addressables.Release(handle);
            return default;
        }
    }

    // �첽������Դ���Ƽ���
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

    // ʵ������Ϸ�����첽��
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

    // �ͷ���Դ
    public void ReleaseAsset<T>(T asset)
    {
        Addressables.Release(asset);
    }

    // ����ʵ��������Ϸ����
    public void ReleaseInstance(GameObject instance)
    {
        Addressables.ReleaseInstance(instance);
    }

    // �����Դ�Ƿ����
    public async Task<bool> CheckIfAssetExists(string address)
    {
        var handle = Addressables.LoadResourceLocationsAsync(address);
        await handle.Task;

        bool exists = handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null && handle.Result.Count > 0;
        Addressables.Release(handle);
        return exists;
    }
}