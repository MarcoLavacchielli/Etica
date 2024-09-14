using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control
{
    public Transform Body;
    private string _horizontalAxis = "Horizontal";
    private string _verticalAxis = "Vertical";
    private Vector3 _inputVector;

    Movement _movement;

    public Control(Movement m, Transform t)
    {
        _movement = m;
        Body = t;
    }

    public void NormalControls()
    {
        _inputVector.x = Input.GetAxis(_horizontalAxis);
        _inputVector.z = Input.GetAxis(_verticalAxis);

        _inputVector = new Vector3(_inputVector.x, 0f, _inputVector.z);
        //_movement.Move(moveDirection);

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            _movement.Run();
        }

    }

    public Vector3 GetMovementInputs()
    {
        return _inputVector;
    }

}
