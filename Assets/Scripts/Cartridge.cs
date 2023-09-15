using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Cartridge : MonoBehaviour, IMechanicalMovement
{
    public Vector3 Speed { get; set;}
    public MovementState Status { get; set; }
    public Vector3 Position 
    {
        get {
            return transform.position;
        }
        set {
            transform.position = value;
        }
    }
    public int CountCollision { get; set; }
    public Vector3 hidePosition;

    private readonly float roughness = 0.5f;

    public void StartMove(Vector3 position, Vector3 speed)
    {
        Speed = speed;
        Position = position;
        Status = MovementState.move;
        CountCollision = 0;
    }

    public void StopMove()
    {
        Position = hidePosition;
        Status = MovementState.stop;
    }

    public void SetRandomSurface()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Vector3[] vertices = meshFilter.mesh.vertices;

        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices[i].x + roughness * Random.Range(-1.0f, 1.0f),
                                      vertices[i].y + roughness * Random.Range(-1.0f, 1.0f),
                                      vertices[i].z + roughness * Random.Range(-1.0f, 1.0f));
        }

        meshFilter.mesh.vertices = vertices;
    }

    private void Start()
    {
        Status = MovementState.stop;
        SetRandomSurface(); 
    }
}

public interface IMechanicalMovement
{
    Vector3 Position { get; set; }
    Vector3 Speed { get; set; }
    MovementState Status { get; set; }
    int CountCollision { get; set; }
    void StopMove();
}

public enum MovementState
{ 
    move = 0,
    stop,
}