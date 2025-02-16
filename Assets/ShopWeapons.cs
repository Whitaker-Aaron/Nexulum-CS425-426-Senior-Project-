using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWeapons : MonoBehaviour
{
    [SerializeField] List<WeaponBase> weaponsInStore = new List<WeaponBase>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<WeaponBase> getWeapons()
    {
        return weaponsInStore;
    }

    public void removeWeapon(WeaponBase weaponToRemove)
    {
        for (int i = 0; i < weaponsInStore.Count; i++)
        {
            if (weaponsInStore[i].weaponName == weaponToRemove.weaponName)
            {
                weaponsInStore.RemoveAt(i);
                break;
            }
        }
    }
}
