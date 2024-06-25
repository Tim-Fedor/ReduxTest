using Cysharp.Threading.Tasks;
using Modules.AssetManagement;
using UnityEngine;

namespace Modules.BoostGeneration
{
    public class BoostManager : MonoBehaviour
    {
        [SerializeField] private BoostData[] _allBoosts;
        
        private readonly IObjectFactory _objectFactory = new ObjectFactory();

        public async void TryToCreateABoost(Vector3 position)
        {
            float boostChance = Random.Range(0, 100);
            
            for (int i = 0; i < _allBoosts.Length; i++)
            {
                boostChance -= _allBoosts[i].ChanceToSpawn;
                if (boostChance < 0)
                {
                    var boostGO = await SpawnBoost(_allBoosts[i], position);
                    boostGO.GetComponent<ABoosterBehaviour>().SetupValues(_allBoosts[i].FloatValue, _allBoosts[i].IntValue);
                    return;
                }
            }
            
        }

        private async UniTask<GameObject> SpawnBoost(BoostData boostToCreate, Vector3 pos)
        {
            var result = await _objectFactory.InstantiateAsync(boostToCreate.BoostModelName, pos, Quaternion.identity, transform);
            return result;
        }
        
        private void OnDestroy()
        {
            _objectFactory.CleanUp();
        }
    }
    
}