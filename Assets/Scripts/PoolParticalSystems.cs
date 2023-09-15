using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolParticalSystems : MonoBehaviour
{
    [SerializeField] private ParticleSystem _prefabParticalSystem;
    [SerializeField] private static int _maxCountParticalSystem = 100;

    private static List<ParticleSystem> _poolParticalSystem = new List<ParticleSystem>();
    private float _timeCreateParticalSystem = 0.2f;
    private static int _indexCurrentSystem = 0;

    public static void BlastActivation(Vector3 position)
    {
        _indexCurrentSystem ++;

        if (_indexCurrentSystem >= _maxCountParticalSystem)
        {
            _indexCurrentSystem = 0;
        }

        if(_poolParticalSystem.Count > _indexCurrentSystem)
        {
            _poolParticalSystem[_indexCurrentSystem].transform.position = position;
            _poolParticalSystem[_indexCurrentSystem]?.Play(); 
        }
    }

    private void Start()
    {
        StartCoroutine(CreatePoolParticalSystems());
    }

    private IEnumerator CreatePoolParticalSystems()
    {
        for(int i = 0 ; i < _maxCountParticalSystem; i++)
        {
            ParticleSystem particleSystem = Instantiate(_prefabParticalSystem);
            particleSystem.Stop();
            _poolParticalSystem.Add(particleSystem);
            yield return new WaitForSeconds(_timeCreateParticalSystem);
        }
    }
}
