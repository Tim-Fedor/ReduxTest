using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Modules.AssetManagement;
using Modules.BoostGeneration;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Modules.PlatformGeneration
{
    public class PlatformManager : MonoBehaviour, IPlatformManager
    {
        [SerializeField] private PlatformData[] _platformsData;
        [SerializeField] private PlatformData _lastPlatform;
        [SerializeField] private Waypoints waypointSystem;
        
        [SerializeField] private int platformCount = 10;
        
        [SerializeField] private BoostManager boostManager;
        
        private Vector3 _nextPlatform;

        private Transform _cachedTransform;

        private bool _isLastPlatformWasDanger;

        private readonly IObjectFactory _objectFactory = new ObjectFactory();
        private List<PlatformObjectData> _allPlatforms = new List<PlatformObjectData>();

        public event Action Initialized;

        private void Awake()
        {
            _cachedTransform = transform;
        }

        private async void Start()
        {
            _nextPlatform = transform.position;
            
            for (int i = 0; i < platformCount; i++)
            {
                await SpawnPlatform();
            }

            SpawnPlatform(_lastPlatform);
            
            Initialized?.Invoke();
        }

        private async UniTask SpawnPlatform(PlatformData platformToSpawn = null)
        {
            if (platformToSpawn == null)
            {
                do
                {
                    platformToSpawn = _platformsData[0];
                    
                    float blockToChoose = Random.Range(0, 100f);
                    for (int i = 0; i < _platformsData.Length; i++)
                    {
                        blockToChoose -= _platformsData[i].ChanceToSpawn;
                        if (blockToChoose <= 0)
                        {
                            platformToSpawn = _platformsData[i];      
                            break;
                        }
                    }
                } 
                while (_isLastPlatformWasDanger && platformToSpawn.IsDangerBlock);

                _isLastPlatformWasDanger = platformToSpawn.IsDangerBlock;
            }

            
            var platform = await _objectFactory.InstantiateAsync(platformToSpawn.PlatformModelName, _nextPlatform, Quaternion.identity, _cachedTransform);

            if (_allPlatforms.Count > 0)
            {
                platform.transform.rotation = _allPlatforms[_allPlatforms.Count - 1].PlatformTransform.rotation;   
            }

            if (platformToSpawn.Rotation != Vector3.zero)
            {
                platform.transform.Rotate(platformToSpawn.Rotation);
            }
                

            if (!_isLastPlatformWasDanger)
                boostManager.TryToCreateABoost(_nextPlatform + Vector3.up);
            
            PlatformObjectData newPlatform = new PlatformObjectData();
            newPlatform.PlatformInfo = platformToSpawn;
            newPlatform.PlatformTransform = platform.transform;
            
            _allPlatforms.Add(newPlatform);
            
            _nextPlatform = platform.transform.position + platform.transform.forward * platformToSpawn.Length;
        }

        private void OnDestroy()
        {
            _allPlatforms.Clear();
            _objectFactory.CleanUp();
        }

        public Transform GetNextWaypoint(Transform waypoint)
        {
            return waypointSystem.GetNextWaypoint(waypoint);
        }

        public PlatformData[] GetAllDangerPlatformPassedBy(Transform waypoint)
        {
            List<PlatformData> result = new List<PlatformData>();
            int waypointIndex = waypointSystem.GetWaypointIndex(waypoint);

            for (int i = 0; i < _allPlatforms.Count; i++)
            {
                if(i > waypointIndex) break;

                if (_allPlatforms[i].PlatformInfo.IsDangerBlock)
                {
                    result.Add(_allPlatforms[i].PlatformInfo);
                }
            }
            return result.ToArray();
        }

        private class PlatformObjectData
        {
            public Transform PlatformTransform;
            public PlatformData PlatformInfo;
        }
    }
    
}