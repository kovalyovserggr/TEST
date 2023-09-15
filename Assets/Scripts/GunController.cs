using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class GunController : MonoBehaviour
{
    [SerializeField] private GameObject ShootingDirectionPoint0;
    [SerializeField] private GameObject ShootingDirectionPoint1;
    [SerializeField] private GameObject Barrel;
    [SerializeField] private Vector3 _barrelRestState = new Vector3(0f, 0.6679f, 0f);
    [SerializeField] private Vector3 _barrelExtremeShotState = new Vector3(0f, 0.6679f, -0.126f);
    [Range(0f, 1f)]
    [SerializeField] private float _stepBarrelAnimation = 0;
    private IGunController _controller;
    private GunHolster _gunHolster;
    private float _timeUpdateControl = 0.05f;
    private float _timeCreaatePoint = 0.01f;
    private float _powerShot = 30.0f;
    private List<GameObject> _trajectoryPoints = new List<GameObject>();

    public GameObject TrajectoryObject;
   
    private void Start()
    {
       // #if UNITY_EDITOR_WIN ||  UNITY_STANDALONE_WIN 
            _controller = new GunControllerForWin(_timeUpdateControl);
        //#endif
        //... other OS
        
        _controller.Shot += OnShot;
        _controller.NewDirection += OnNewDirection;

        PowerRegulation.ChangePower += ChangePowerShot;

        _gunHolster = GetComponent<GunHolster>();

        StartCoroutine(UpdateBarrelDirection());
        StartCoroutine(CreatePointForCtrajectory());
    }

    private void ChangePowerShot(float power)
    {
        _powerShot = power;
        UpdateTrajectory();
    }

    private IEnumerator UpdateBarrelDirection()
    {
        while(true)
        {
           _controller?.UpdateControl();
            UpdateBarrelAnimation();
            yield return new WaitForSeconds(_timeUpdateControl);
        }
    }

    private IEnumerator CreatePointForCtrajectory()
    {
        TrajectoryObject.transform.position = Vector3.zero;
        GameObject point;
        for (int i = 0; i < 90; i++)
        {
            point = Instantiate(TrajectoryObject);
            point.transform.position = new Vector3(1.0f* i, 1.0f, -5.0f);
            _trajectoryPoints.Add(point);
            yield return new WaitForSeconds(_timeCreaatePoint);
        }
        UpdateTrajectory();
    }

    private void OnNewDirection(Vector3 direction)
    {
        transform.rotation = Quaternion.Euler(direction.x, direction.y, 0);
        UpdateTrajectory();
    }

    private void OnShot()
    {
        var cartridge = _gunHolster?.GetNextCartridge();
        var position = ShootingDirectionPoint0.transform.position;
        var speed = (ShootingDirectionPoint0.transform.position - ShootingDirectionPoint1.transform.position).normalized * _powerShot;

        if (cartridge != null) 
        {
            _stepBarrelAnimation = 1;
            cartridge.StartMove(position, speed);
        }
    }

    private void UpdateBarrelAnimation()
    {
        
        if (_stepBarrelAnimation <= 0)
        {
            return;
        }
        else
        {
            _stepBarrelAnimation -= 0.1f;
        }

        Barrel.transform.localPosition = Vector3.Lerp(_barrelRestState, _barrelExtremeShotState, _stepBarrelAnimation);
    }

    private void UpdateTrajectory()
    {
        List<DataVertex> list = CalculationTrajectoria();

        for (int i = 0; i < list.Count && i < _trajectoryPoints.Count; i++)
        {
            _trajectoryPoints[i].transform.position = list[i]._position;
        }
    }
    private List<DataVertex> CalculationTrajectoria() 
    {
        List<DataVertex> dataVertex = new List<DataVertex>();
        Vector3 S = new Vector3(0f, 0f, 0f);
        Vector3 S0 = ShootingDirectionPoint0.transform.position;
        Vector3 g = new Vector3(0f, -9.8f, 0f);
        Vector3 V0 = (ShootingDirectionPoint0.transform.position - ShootingDirectionPoint1.transform.position).normalized * _powerShot;
        Vector3 V;

        for (float t = 0f; (t < 3.0f && S.z < 40.0f); t += 0.01f)
        {
            S = S0 + V0 * t + g * t * t / 2.0f;
            V = V0 + g * t;
            V0 = V - V * 0.023f;
            S0 = S;
            dataVertex.Add(new DataVertex(V0, S0));
        }

        return dataVertex;
    }

    

    private class DataVertex
    {
        public Vector3 _vector;
        public Vector3 _position;

        public DataVertex(Vector3 vector, Vector3 position)
        {
            _vector = vector;
            _position = position;
        }
    }
}


public interface IGunController
{
    /// <summary>
    /// The start of processing the shot event
    /// </summary>
    event Action Shot;

    /// <summary>
    /// The start of processing the shot event
    /// </summary>
    event Action<Vector3> NewDirection;

    /// <summary>
    ///  pointing the gun barrel at the target 
    /// </summary>
    void UpdateControl();
}
