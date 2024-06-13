using System;
using DG.Tweening;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        transform.DOShakeRotation(.3f,15f);
    }
}