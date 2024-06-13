using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static Action OnStart;
        public static event Action OnWin;

        private List<Vehicle> _listVehicles;
        private void OnEnable()
        {
            LevelManager.OnLevelStart += LevelStart;
            Vehicle.OnFinishPassed += FinishPassed;
        }

        private void OnDisable()
        {
            LevelManager.OnLevelStart -= LevelStart;
            Vehicle.OnFinishPassed -= FinishPassed;
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
            OnStart?.Invoke();
        }

        private void LevelStart(Level level)
        {
            _listVehicles = level.GetVehiclesList;
        }

        private void FinishPassed(Vehicle vehicle)
        {
            _listVehicles.Remove(vehicle);
            
            if(_listVehicles.Count == 0)
                OnWin?.Invoke();
        }
    }
}