using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class GunHolster : MonoBehaviour
{
    [SerializeField] private GameObject PrefabCatridge;
    [SerializeField] private GameObject MechanicsObject;

    private Cartridge[] _cartridges;
    private const int _countCartridges = 30;
    private int _currentCartridgesIndex = 0;
    private float _timeCreateCatridge = 0.02f;

    public Cartridge GetNextCartridge()
    {
        if (_currentCartridgesIndex == _countCartridges)
        {
            _currentCartridgesIndex = 0;
        }

        return _cartridges[_currentCartridgesIndex++];
    }

    // Start is called before the first frame update
    private void Start()
    {
        _cartridges = new Cartridge[_countCartridges];
        StartCoroutine(ChargeGunHolster()); 
    }
    
    private IEnumerator ChargeGunHolster()
    {
        var mechanics = MechanicsObject?.GetComponent<Mechanics>();
        

        for (int i = 0; i < _cartridges.Length; i++)
        {
            GameObject newCartridge = Instantiate(PrefabCatridge);
            _cartridges[i] = newCartridge.GetComponent<Cartridge>();

            mechanics?.AddNewBody(_cartridges[i]);
            _cartridges[i].hidePosition = new Vector3(i + 1 , 1.0f, -1.0f);
            _cartridges[i].Position = _cartridges[i].hidePosition;

            yield return new WaitForSeconds(_timeCreateCatridge);
        }
    }
} 
