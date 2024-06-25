using System;
using System.Collections.Generic;
using Modules.AssetManagement;
using Modules.PlatformGeneration;
using Modules.PlayerTapController;
using Modules.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettings _settings;
    [SerializeField] private Transform _UICanvas;
    [SerializeField] private Camera _mainCamera;
    
    [SerializeField] private PlatformManager _platformManager;
    [SerializeField] private Waypoints _waypoints;
    
    private WinScreenView _winScreen;
    private LoseScreenView _loseScreenView;
    private IPlayer _player;
    
    private readonly IObjectFactory _objectFactory = new ObjectFactory();

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _platformManager.Initialized += OnPathCreated;
    }

    private async void CreatePlayer()
    { 
        var playerObject = await _objectFactory.InstantiateAsync(_settings.PlayerModelName, Vector3.zero, Quaternion.identity, null);
        _player = playerObject.GetComponent<IPlayer>();
        _mainCamera.transform.SetParent(playerObject.transform);

        _player.SetupDefaultValues(_settings.PlayerDefaultLives, _settings.PlayerDefaultSpeed);
        
        _player.PlayerDied += LoseGame;
        _player.PlayerFinishPath += WinGame;
        
        _player.InitializePath(_waypoints);
    }

    private void OnPathCreated()
    {
        _platformManager.Initialized -= OnPathCreated;

        CreatePlayer();
    }

    private void OnDestroy()
    {
        _player.PlayerDied -= LoseGame;
        _player.PlayerFinishPath -= WinGame;
    }

    private async void LoseGame()
    {
        Time.timeScale = 0;

        var loseScreen = await _objectFactory.InstantiateAsync(_settings.LoseScreenModelName, _UICanvas);        
        _loseScreenView = loseScreen.GetComponent<LoseScreenView>();
        
        var stats = CalculateStatisticsOfCurrentGame();
        _loseScreenView.SetBlocksStatistics(stats);
        _loseScreenView.gameObject.SetActive(true);
        _loseScreenView.OnTryAgainTap += ReloadGame;
        _loseScreenView.OnReviveTap += ReviveAction;
    }

    private void ReviveAction()
    {
        _loseScreenView.OnTryAgainTap -= ReloadGame;
        _loseScreenView.OnReviveTap -= ReviveAction;
        Destroy(_loseScreenView.gameObject);
        
        _player.ApplySpeedChange(_player.Speed / 2, 2f);
        _player.ApplyHealthChange(3);
        _player.GoToNextWaypoint();
        
        _loseScreenView.gameObject.SetActive(false);
        
        Time.timeScale = 1;
    }

    private async void WinGame()
    {
        Time.timeScale = 0;
        
        var winScreen = await _objectFactory.InstantiateAsync(_settings.WinScreenModelName, _UICanvas);        
        _winScreen = winScreen.GetComponent<WinScreenView>();
        
        var stats = CalculateStatisticsOfCurrentGame();
        _winScreen.SetBlocksStatistics(stats);
        _winScreen.gameObject.SetActive(true);
        _winScreen.OnNextLevelTap += ReloadGame;
    }

    private Dictionary<string, int> CalculateStatisticsOfCurrentGame()
    {
        var allDangerPassedBlocks = _platformManager.GetAllDangerPlatformPassedBy(_player.GetCurrentWaypoint());
        Dictionary<string, int> blocksInfo = new Dictionary<string, int>();

        for (int i = 0; i < allDangerPassedBlocks.Length; i++)
        {
            if (blocksInfo.ContainsKey(allDangerPassedBlocks[i].ViewName))
            {
                blocksInfo[allDangerPassedBlocks[i].ViewName]++;
            }
            else
            {
                blocksInfo.Add(allDangerPassedBlocks[i].ViewName, 1);
            }
        }
        
        return blocksInfo;
        
    }

    private void ReloadGame()
    {
        _objectFactory.CleanUp();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
