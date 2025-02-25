using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class swordCombat : MonoBehaviour
{
    public int damage = 0;
    public int heavyDamage = 0;
    public int projectileDamage = 0;
    protected int count = 0;
    protected AudioManager audioManager;
    protected UIManager uiManager;
    public bool checking = false;


    //rune ability combat mechanic
    //public bool isFire = false;
    //public int fireDmg = 10;
    //public float fireTime = 5f;
    //public float fireDmgInterval = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract IEnumerator activateAttack(Transform attackPoint, float radius, LayerMask layer, bool isHeavy, float time, int count);
    

    public void GetDamage()
    {
        var playerBase = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();

        switch (playerBase.equippedWeapon.weaponClassType)
        {
            case WeaponBase.weaponClassTypes.Knight:
                damage = playerBase.knightObject.baseAttack + playerBase.equippedWeapon.weaponAttack;
                heavyDamage = playerBase.knightObject.baseAttack + playerBase.equippedWeapon.weaponHeavyAttack;
                break;
            case WeaponBase.weaponClassTypes.Gunner:
                break;
            case WeaponBase.weaponClassTypes.Engineer:
                break;
        }
    }

    public void updateDamage(int dmg)
    {
        Debug.Log("Sword damaged updated to: " + dmg);
        //damage = dmg;
    }



    
}
