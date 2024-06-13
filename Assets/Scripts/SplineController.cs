using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineController : MonoBehaviour
{
    public static SplineController Instance;
    private SplineContainer _splineContainer;
    private float _percentage;

    public bool IsVehicleMovingOnSpline { get; set; }

    public float GetPercentage() => _percentage;
    public void SetSplineContainer(SplineContainer splineContainer) => _splineContainer = splineContainer;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void GetNearestPoint(Vector3 pos,out Vector3 nearest, out float percentage)
    {
        var native = new NativeSpline(_splineContainer.Spline, _splineContainer.transform.localToWorldMatrix);
        var dist = SplineUtility.GetNearestPoint(native, pos,out var n,  out var p);
        
        nearest = n;
        percentage = p;
    }

   
}
