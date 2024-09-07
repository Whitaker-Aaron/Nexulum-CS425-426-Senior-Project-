using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] string weaponName;
    [SerializeField] weaponClassTypes weaponClassType;
    [SerializeField] GameObject weaponMesh;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum weaponClassTypes
    {
        Knight,
        Gunner,
        Engineer
    }
}
