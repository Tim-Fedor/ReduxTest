using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI
{
    public class WinScreenView : MonoBehaviour
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Transform _content;
        
        [Header("Template for block stats")]
        [SerializeField] private GameObject _template;
        [SerializeField] private TMP_Text _templateText;

        public event Action OnNextLevelTap;

        private void Awake()
        {
            _nextLevelButton.onClick.AddListener(NextLevelClick);
        }
        
        private void OnDestroy()
        {
            _nextLevelButton.onClick.RemoveListener(NextLevelClick);
        }

        private void NextLevelClick()
        {
            OnNextLevelTap?.Invoke();
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