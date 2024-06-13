using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Level : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private List<Vehicle> listVehicles;
    public int GetLevelNumber => level;
    public SplineContainer GetSplineContainer => splineContainer;

    public List<Vehicle> GetVehiclesList => listVehicles;
}