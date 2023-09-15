using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class СollisionPlane : MonoBehaviour
{
    public Texture2D TexturePlane;
    [SerializeField] private float _radiusCartridge = 0.2f;
    [SerializeField] private GameObject _point0;
    [SerializeField] private GameObject _e1;
    [SerializeField] private GameObject _e2;
    [SerializeField] private GameObject _e3;
    [SerializeField] private FilterMode _filterMode = FilterMode.Bilinear;
    [SerializeField] private int _textureWidth = 333;
    [SerializeField] private int _textureHeight = 130;
    [SerializeField] private  float _deltaEntry = 0.5f;
    [SerializeField] private  float _coefficientElasticity = 0.9f;
    private Matrix4x4 _localToWorld;
    private Matrix4x4 _worldToLocal;

    void Start()
    {
       _localToWorld = new Matrix4x4(_e1.transform.position - _point0.transform.position,
                                      _e2.transform.position - _point0.transform.position,
                                      _e3.transform.position - _point0.transform.position,
                                      new Vector4(1, 1, 1, 1) );

        _worldToLocal = _localToWorld.inverse;

        TexturePlane = new Texture2D(_textureWidth,_textureHeight);
        TexturePlane.filterMode = _filterMode; 
        GetComponent<Renderer>().material.mainTexture = TexturePlane;
    }

    public bool СollisionСalculations(Vector3 point, out Vector3 pointToLocal)
    {
        pointToLocal = _worldToLocal * point - _worldToLocal * _point0.transform.position;
     
        return pointToLocal.x > -_deltaEntry && 
               pointToLocal.z > -_deltaEntry && 
               pointToLocal.x < _deltaEntry && 
               pointToLocal.z < _deltaEntry && 
               pointToLocal.y < _radiusCartridge;
    }

    public Vector3 BlowCalculations(Vector3 speed)
    { 
        speed = _worldToLocal * speed;
        speed = new Vector3(speed.x, - speed.y * _coefficientElasticity, speed.z);
        speed = _localToWorld * speed;

        return speed;
    }

    public Vector2 GetPixelPositionLastBlaw(Vector3 position)
    {
        float[] matrix2x2 = new float[] { _textureWidth, 0.0f, 0.0f, -135.0f};
        Vector2 offset = new Vector2( _textureWidth / 2, _textureHeight / 2);
        Vector2 v = new Vector2( matrix2x2[0] * position.x + matrix2x2[1] * position.z + offset.x,
                            matrix2x2[2] * position.x + matrix2x2[3] * position.z + offset.y);
        return v;
    }


}
