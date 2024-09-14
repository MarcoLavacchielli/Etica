using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Movement
{

    float _jumpForce;
    float _playerSpeed;
    float _normalSpeed;
    public float _ContF;
    public float _ContA = -1;
    Animator _anim;
    Rigidbody _myRigidBody;
    Transform body;
    CapsuleCollider _collider;
    //public Action artificialBoost;
    //private float count;
    private bool _running = false;
    private float _animSpeed;



    public Movement(Rigidbody rb, float jf, float playerSpeed, Transform trans, Animator stan, CapsuleCollider collider)
    {
        _myRigidBody = rb;
        _jumpForce = jf;
        _playerSpeed = playerSpeed;
        body = trans;
        _anim = stan;
        _collider = collider;
    }

    public void OnAwake()
    {
        //artificialBoost = BootBoost;
        _normalSpeed = _playerSpeed;
        _running = false;
        _animSpeed = 0.5f;
    }

    public void Move(Vector3 inputs)
    {
        PlayerRotation(inputs);
        body.LookAt(body.position + inputs);

        _anim.SetFloat("Speed", inputs.magnitude * _animSpeed);

        _myRigidBody.MovePosition(body.position + inputs * (_playerSpeed * Time.fixedDeltaTime));
    }

    public void Run()
    {
        if (_running == false)
        {
            _running = true;
            _animSpeed = 1.0f; // Ajusta la velocidad para correr
            _playerSpeed = _playerSpeed * 2f;
            Debug.Log(_playerSpeed);
        }
        else
        {
            _running = false;
            _playerSpeed = _normalSpeed;
            _animSpeed = 0.5f; // Ajusta la velocidad para caminar
        }

    }

    void PlayerRotation(Vector3 inputs)
    {
        if (inputs.normalized == Vector3.zero) return;

        Quaternion rotation = Quaternion.LookRotation(inputs);
        body.rotation = Quaternion.Slerp(body.rotation, rotation, Time.deltaTime * 10f);
        //else _entity.Body.rotation = Quaternion.Slerp(_entity.Body.rotation, rotation, 10f * Time.deltaTime);
    }


}
