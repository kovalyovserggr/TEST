using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControllerForWin :IGunController
{
    public event Action Shot;
    public event Action<Vector3> NewDirection;

    private const float _maxAngle = 30.0f;
    private float _deltaTime;
    private float _angleY = .0f, _angleX = .0f, _deltaAngelInSecond = 10.0f;
    private bool _isAngleChanged = false;

    public GunControllerForWin(float deltaTime) 
    {
        _deltaTime = deltaTime;
    }

    public void UpdateControl()
    {
        if (Input.GetKey(KeyCode.J) && -_maxAngle < _angleY)
        {
            _angleY -= _deltaTime * _deltaAngelInSecond;
            _isAngleChanged = true;
        }

        if (Input.GetKey(KeyCode.L) && _maxAngle > _angleY)
        {
            _angleY += _deltaTime * _deltaAngelInSecond;
            _isAngleChanged = true;
        }

        if (Input.GetKey(KeyCode.I) && -_maxAngle < _angleX)
        {
            _angleX -= _deltaTime * _deltaAngelInSecond;
            _isAngleChanged = true;
        }

        if (Input.GetKey(KeyCode.K) && _maxAngle > _angleX)
        {
            _angleX += _deltaTime * _deltaAngelInSecond;
            _isAngleChanged = true;
        }
        
        if (_isAngleChanged == true)
        {
            _isAngleChanged = false;
            NewDirection?.Invoke(new Vector3(_angleX, _angleY,.0f));
        }

        if (Input.GetKey(KeyCode.F) == true)
        {
            Shot?.Invoke();
        }  
    }
}
