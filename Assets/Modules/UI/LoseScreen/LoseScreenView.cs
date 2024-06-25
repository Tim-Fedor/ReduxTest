using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI
{
    public class LoseScreenView : MonoBehaviour
    {
        [SerializeField] private Button _tryAgainButton;
        [SerializeField] private Button _reviveButton;
        
        [SerializeField] private Transform _content;
        
        [Header("Template for block stats")]
        [SerializeField] private GameObject _template;
        [SerializeField] private TMP_Text _templateText;
        
        public event Action OnTryAgainTap;
        public event Action OnReviveTap;
            
        private void Awake()
        {
            _tryAgainButton.onClick.AddListener(TryAgainAction);
            _reviveButton.onClick.AddListener(OnReviveAction);
        }

        private void OnDestroy()
        {
            _tryAgainButton.onClick.RemoveListener(TryAgainAction);
            _reviveButton.onClick.RemoveListener(OnReviveAction);
        }
        
        private void TryAgainAction()
        {
            OnTryAgainTap?.Invoke();
        }
        
        private void OnReviveAction()
        {
            OnReviveTap?.Invoke();
        }
        
        public void SetBlocksStatistics(Dictionary<string, int> stats)
        {
            foreach(KeyValuePair<string, int> kvp in stats)
            {
                _templateText.text = $"You passed {kvp.Value} blocks of {kvp.Key}!";
                Instantiate(_template, _content).SetActive(true);
            }
        }
        
        
    }
}