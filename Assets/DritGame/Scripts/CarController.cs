using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public enum EAxel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct SWheel
    {
        public GameObject _wheelModel;
        //public WheelCollider _wheelCollider;
        public EAxel _axel;
    }

    [SerializeField] private List<SWheel> _wheels;

    //[SerializeField] private float _maxAcceleration;
    //[SerializeField] private float _brakeAcceleration;

    //[SerializeField] private float _turnSensivity;
    //[SerializeField] private float _maxSteerAngle;

    //[SerializeField] private Vector3 _centerOfMass;

    //private float _moveInput;
    //private float _steerInput;

    //private Rigidbody _carRb;


    //[SerializeField] private float _driftFactor;
    //private bool _isDrifting = false;



    //private void Start()
    //{
    //    _carRb = GetComponent<Rigidbody>();
    //    _carRb.centerOfMass = _centerOfMass;
    //}
    //private void Update()
    //{
    //    GetInputs();
    //    WheelsAnimation();
    //}

    //private void LateUpdate()
    //{
    //    Move();
    //    Steer();
    //    HandBrake();
    //    Drift();
    //}




    //private void GetInputs()
    //{
    //    _moveInput = Input.GetAxis("Vertical");
    //    _steerInput = Input.GetAxis("Horizontal");
    //}


    //private void Move()
    //{
    //    foreach (var wheel in _wheels)
    //    {
    //        wheel._wheelCollider.motorTorque = _moveInput * 600 * _maxAcceleration * Time.deltaTime;
    //    }
    //}


    //private void Steer()
    //{
    //    foreach (var wheel in _wheels)
    //    {
    //        if (wheel._axel == EAxel.Front)
    //        {
    //            var steerAngle = _steerInput * _turnSensivity * _maxSteerAngle;
    //            wheel._wheelCollider.steerAngle = Mathf.Lerp(wheel._wheelCollider.steerAngle, steerAngle, 0.6f);
    //        }
    //    }
    //}

    //private void HandBrake()
    //{
    //    if (Input.GetKey(KeyCode.Space))
    //    {
    //        foreach (var wheel in _wheels)
    //        {
    //            if (wheel._axel == EAxel.Rear)
    //            {
    //                wheel._wheelCollider.brakeTorque = 300 * _brakeAcceleration * Time.deltaTime;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        foreach (var wheel in _wheels)
    //        {
    //            wheel._wheelCollider.brakeTorque = 0;
    //        }
    //    }
    //}
    //private void Drift()
    //{
    //    // Check if the car is steering (regardless of forward or backward movement) and apply drift behavior
    //    if (_steerInput != 0)
    //    {
    //        foreach (var wheel in _wheels)
    //        {
    //            if (wheel._axel == EAxel.Rear)
    //            {
    //                ApplyDriftPhysics(wheel._wheelCollider);
    //            }
    //        }
    //        _isDrifting = true;
    //    }
    //    else
    //    {
    //        // If not steering, reset the physics properties
    //        foreach (var wheel in _wheels)
    //        {
    //            if (wheel._axel == EAxel.Rear)
    //            {
    //                ResetDriftPhysics(wheel._wheelCollider);
    //            }
    //        }
    //        _isDrifting = false;
    //    }
    //}
    //private void ApplyDriftPhysics(WheelCollider wheelCollider)
    //{
    //    // Simulate drift by adjusting sideways friction
    //    WheelFrictionCurve sidewaysFriction = wheelCollider.sidewaysFriction;
    //    sidewaysFriction.stiffness = _driftFactor;
    //    wheelCollider.sidewaysFriction = sidewaysFriction;
    //}

    //private void ResetDriftPhysics(WheelCollider wheelCollider)
    //{
    //    // Reset sideways friction to default to stop drift
    //    WheelFrictionCurve sidewaysFriction = wheelCollider.sidewaysFriction;
    //    sidewaysFriction.stiffness = 2.0f; // Default value
    //    wheelCollider.sidewaysFriction = sidewaysFriction;
    //}

    //private void WheelsAnimation()
    //{
    //    foreach (var wheel in _wheels)
    //    {
    //        Quaternion rot;
    //        Vector3 pos;
    //        wheel._wheelCollider.GetWorldPose(out pos, out rot);
    //        wheel._wheelModel.transform.position = pos;
    //        wheel._wheelModel.transform.rotation = rot;
    //    }
    //}






    [SerializeField] private float _moveSpeed = 50f;
    [SerializeField] private float _maxSpeed = 15;
    [SerializeField] private float _drag = 0.98f;
    [SerializeField] private float _steerAngle = 20;
    [SerializeField] private float _traction = 1;
    [SerializeField] private Transform[] _wheelModels;

    private Vector3 _moveForce;

    private void Update()
    {
        _moveForce += transform.forward * _moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += _moveForce * Time.deltaTime;

        float steerInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerInput * _moveForce.magnitude * _steerAngle * Time.deltaTime);

        _moveForce *= _drag;
        _moveForce = Vector3.ClampMagnitude(_moveForce, _maxSpeed);



        Debug.DrawRay(transform.position, _moveForce.normalized * 10, Color.red);
        Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);
        _moveForce = Vector3.Lerp(_moveForce.normalized, transform.forward, _traction * Time.deltaTime) * _moveForce.magnitude;


        WheelsAnimation();
    }

    private void WheelsAnimation()
    {
        float moveInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");

        foreach (Transform wheelModel in _wheelModels)
        {
            // Rotate the wheels based on the car's movement
            wheelModel.Rotate(Vector3.right, _moveSpeed * moveInput * Time.deltaTime, Space.Self);

            // Implement steering animation (optional)
            wheelModel.localRotation = Quaternion.Euler(wheelModel.localRotation.eulerAngles.x, steerInput * 30f, wheelModel.localRotation.eulerAngles.z);
        }
    }
}
