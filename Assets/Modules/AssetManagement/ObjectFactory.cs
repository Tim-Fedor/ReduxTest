using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.AssetManagement
{
    public class ObjectFactory : IObjectFactory
    {
        private IAssetProvider _assetProvider = new AssetProvider();

        public async UniTask<GameObject> InstantiateAsync(string address, Transform parent)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(address);
            GameObject go = InstantiatePrefab(prefab, parent);
            return go;
        }

        public async UniTask<GameObject> InstantiateAsync(string address, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(address);
            GameObject go = InstantiatePrefab(prefab, position, rotation, parent);
            return go;
        }
        
        private GameObject InstantiatePrefab(GameObject prefab, Transform parent)
        {
            return UnityEngine.Object.Instantiate(prefab, parent);
        }

        private GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            return UnityEngine.Object.Instantiate(prefab, position, rotation, parent);
        }

        public void CleanUp()
        {
            _assetProvider.CleanUp();
        }

        public void Dispose()
        {
            CleanUp();
        }
        
    }
}
