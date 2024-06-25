using Cysharp.Threading.Tasks;

namespace Modules.AssetManagement
{
    public interface IAssetProvider
    {
        UniTask<T> Load<T>(string address) where T : class;
        void CleanUp();
    }
}