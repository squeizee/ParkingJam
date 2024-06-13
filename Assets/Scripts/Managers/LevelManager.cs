using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static event Action<Level> OnLevelStart;
        
        [SerializeField] private Transform levelParent;
        [SerializeField] private List<Level> listLevel;

        private int _activeLevelNumber;
        private Level _activeLevel;
        private void OnEnable()
        {
            GameManager.OnStart += LoadLevel;
        }

        private void OnDisable()
        { 
            GameManager.OnStart -= LoadLevel;
        }
        private void LoadLevel()
        {
            ClearLevelParent();
            
            _activeLevelNumber++;
            
            if (_activeLevelNumber > listLevel.Count)
                _activeLevelNumber = 1;
            
            _activeLevel = Instantiate(listLevel.Find(x => x.GetLevelNumber == _activeLevelNumber), levelParent);
            SplineController.Instance.SetSplineContainer(_activeLevel.GetSplineContainer);
            OnLevelStart?.Invoke(_activeLevel);
        }

        private void ClearLevelParent()
        {
            foreach (Transform child in levelParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}