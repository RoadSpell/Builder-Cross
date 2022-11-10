using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private AnimatorController _animatorController;
    public float _moveSpeed;
    public float _rotateSpeed;
    [SerializeField] private BoxManager _boxManager;
    [SerializeField]  private bool _canMove = true;
    public bool CanMove
    {
        get { return _canMove; }
        set { _canMove = value; }
    }

    private Rigidbody _rigidbody;
    private Vector3 _moveVector;
    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
   
    private void FixedUpdate()
    {
        if (_canMove)
        {
            Move();
        }
         
    }

    private void Move()
    {
        
        _moveVector.x = _joystick.Horizontal * _moveSpeed * Time.deltaTime;
        _moveVector.z = _joystick.Vertical * _moveSpeed * Time.deltaTime;

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {

            Vector3 direction = Vector3.RotateTowards(transform.forward, _moveVector, _rotateSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(direction);
            if (_boxManager.GetHaveBox())
            {
                _animatorController.BoxRun();
                _moveSpeed = 5;
            }
            else
            {
                _animatorController.PlayRun();
            }
        }

        else if (_joystick.Horizontal == 0 && _joystick.Vertical == 0)
        {
            if (_boxManager.GetHaveBox())
            {
                _animatorController.BoxStand();
            }
            else
            {
                _animatorController.PlayIdle();
            }

        }
        _rigidbody.MovePosition(_rigidbody.position + _moveVector);
    }
}
