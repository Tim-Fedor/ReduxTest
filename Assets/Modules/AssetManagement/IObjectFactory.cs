using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.AssetManagement
{
    public interface IObjectFactory : IDisposable
    {
        public UniTask<GameObject> InstantiateAsync(string address, Transform parent);
        public UniTask<GameObject> InstantiateAsync(string address, Vector3 position, Quaternion rotation, Transform parent);
        public void CleanUp();
    }
}