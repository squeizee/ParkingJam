using System;
using System.Collections;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

public class Vehicle : MonoBehaviour
{
    public static event Action<Vehicle> OnFinishPassed;
    public static event Action<Collider> OnCrash;
    
    public bool isSelected;
    private float _speed;
    private bool _isMoving;
    private Vector3 _direction;

    private Rigidbody _rigidbody;
    private SplineAnimate _splineAnimate;
    private Collider _collider;

    private Vector3 _triggerEnterPosition;

    private Vector3 GetPosition => transform.position;

    private void OnEnable()
    {
        OnCrash += ShakeOnCrash;
    }

    private void OnDisable()
    {
        OnCrash -= ShakeOnCrash;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _splineAnimate = GetComponent<SplineAnimate>();
        _collider = GetComponent<Collider>();
        
        _speed = Random.Range(20, 30);
    }

    private void FixedUpdate()
    {
        if (_isMoving)
            _rigidbody.MovePosition(GetPosition + _direction * (_speed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            SplineController.Instance.IsVehicleMovingOnSpline = false;
            OnFinishPassed?.Invoke(this);
            Destroy(gameObject,.5f);
        }

        if (!isSelected) return;

        if (other.gameObject.CompareTag("Vehicle") || other.gameObject.CompareTag("Obstacle"))
        {
            Crash(other);
            return;
        }

        if (!other.CompareTag("ParkArea")) return;

        Stop();

        var closestPoint = other.ClosestPoint(transform.position);

        SplineController.Instance.GetNearestPoint(closestPoint,out var n,out var p);
        StartCoroutine(MoveIt(n, p));
    }

    IEnumerator MoveIt(Vector3 targetPosition, float percentage)
    {
        yield return new WaitUntil(() => !SplineController.Instance.IsVehicleMovingOnSpline);

        SplineController.Instance.IsVehicleMovingOnSpline = true;
        
        transform.DOMove(targetPosition, .3f).OnComplete(() =>
        {
            _splineAnimate.StartOffset = percentage;
            _splineAnimate.MaxSpeed = _speed;
            _splineAnimate.PlayOnAwake = true;
            _splineAnimate.enabled = true;
            _splineAnimate.Play();
        });
    }
    private void MoveToTarget(Vector3 targetPosition, float percentage)
    {
        transform.DOMove(targetPosition, .3f).OnComplete(() =>
        {
            _splineAnimate.StartOffset = percentage;
            _splineAnimate.MaxSpeed = _speed;
            _splineAnimate.PlayOnAwake = true;
            _splineAnimate.enabled = true;
            _splineAnimate.Play();
        });
    }

    public void Move(Vector3 direction)
    {
        if (Mathf.Abs(Vector3.Dot(transform.forward, direction)) < .9f || _isMoving) return;

        _direction = direction;
        _isMoving = true;
    }

    private void Crash(Collider crashedCollider)
    {
        Stop();
        OnCrash?.Invoke(crashedCollider);
        
        var closestPoint = crashedCollider.ClosestPoint(GetPosition);
        var directionToExit = (GetPosition - closestPoint).normalized;

        var moveBackVector = _direction == Vector3.forward || _direction == Vector3.back
            ? new Vector3(0, 0, directionToExit.z)
            : new Vector3(directionToExit.x, 0, 0);

        transform.DOMove(transform.position + (_direction * .05f + moveBackVector), .4f);
    }

    private void Stop()
    {
        _isMoving = false;
        isSelected = false;
    }

    private void ShakeOnCrash(Collider col)
    {
        if(_collider == col)
            transform.DOShakeRotation(.3f,15f);
    }
}