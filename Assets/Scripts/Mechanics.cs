using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanics : MonoBehaviour
{
    public const float TimeUpdate = .02f;
    public List<ÑollisionPlane> _collisionPlanes = new List<ÑollisionPlane>();
    [SerializeField] private Color _colorBlaw = new Color(0.0f, 0.0f, 0.0f); 
    [SerializeField] private Vector3 _acceleration = new Vector3(0,-9.8f,0);
    [SerializeField] private float _radiusTrueSpace = 30.0f;
    [SerializeField] private int _limitCollision = 2;
    [SerializeField] private float _coefficientAirFriction = 0.01f;
    private List<IMechanicalMovement> _mechanicalMovements = new List<IMechanicalMovement>();

    public void AddNewBody(IMechanicalMovement mechanicalMovement)
    {
        _mechanicalMovements.Add(mechanicalMovement);
    }
    public void AddPlane(ÑollisionPlane plane)
    {
        _collisionPlanes.Add(plane);
    }

    private void Start()
    {
       StartCoroutine(UpdateMovement());
    }

    private IEnumerator UpdateMovement()
    {
        while (true)
        {
            foreach(IMechanicalMovement move in _mechanicalMovements)
            {
                CalculationTrajectories(move);

                foreach(ÑollisionPlane plane in _collisionPlanes)
                {
                    CalculationCollisionWithPlane(move, plane);
                }
            } 
            
            yield return new WaitForSeconds(TimeUpdate);
        }
    }

    private void CalculationTrajectories(IMechanicalMovement move) 
    {
        if ((move != null) && (move.Status == MovementState.move))
        {
            var newPosition = move.Position + move.Speed * TimeUpdate + _acceleration * TimeUpdate * TimeUpdate / 2.0f; // vectorially: Snew = S + V*t + (gt^2)/2
            var newSpeed = move.Speed + _acceleration * TimeUpdate; // vectorially: Vnew = V + g*t;

            move.Position = newPosition;
            move.Speed = newSpeed - newSpeed *_coefficientAirFriction;

            if (newPosition.magnitude > _radiusTrueSpace)
            {
                move.StopMove();
            }
        }        
    }

    private void CalculationCollisionWithPlane(IMechanicalMovement movi, ÑollisionPlane plane)
    {
        if (plane.ÑollisionÑalculations(movi.Position,out Vector3 localPosition) == true)
        {
            movi.Speed = plane.BlowCalculations(movi.Speed);
            movi.CountCollision++;
           
            if (movi.CountCollision >= _limitCollision)
            {
                if (plane.TexturePlane != null)
                {
                    MarkingBlastPlañe(movi.Position, plane.GetPixelPositionLastBlaw(localPosition), plane.TexturePlane);
                }

                movi.StopMove();
            }
        }
    }

    public void MarkingBlastPlañe(Vector3 position, Vector2 pixelPosition,Texture2D texture)
    {
        PaintBlawPlase(pixelPosition, texture);
        ShowBlastOnDistroyPlace(position);
    }

    private void PaintBlawPlase(Vector2 position, Texture2D texture)
    {
        float WidthTexture = 333, HeightTexture = 130;

        if(texture == null)
            texture = new Texture2D(512, 130);

        if ((texture.width != WidthTexture) || (texture.height != HeightTexture))
            texture.Resize((int) WidthTexture,(int) HeightTexture);

        int x = 0, y = 0;
        float step = 0.0314f;
        for (int j = 1; j < 17; j++)
        {
            if (j == 9)
                step = 0.17f;
            for (float i = 0; i < Mathf.PI * 2; i += step)
                {   
                    x = (int) ((WidthTexture - position.x) + j * Mathf.Cos(i));
                    y = (int) (position.y + j * Mathf.Sin(i));
                    if ((x > 0 && y > 0) && (x < WidthTexture && y < HeightTexture) )
                    {
                        texture.SetPixel(x, y, _colorBlaw);
                    }
                }
        }
       
        texture.Apply();
    }

    private void ShowBlastOnDistroyPlace(Vector3 position)
    {
        PoolParticalSystems.BlastActivation(position);
    }

}
