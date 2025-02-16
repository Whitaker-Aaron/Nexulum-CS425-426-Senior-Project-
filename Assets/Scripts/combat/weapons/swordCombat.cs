using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordCombat : MonoBehaviour
{
    public int damage = 0;
    public int heavyDamage = 0;
    AudioManager audioManager;
    UIManager uiManager;


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

    public void activateAttack(Transform attackPoint, float radius, LayerMask layer, bool isHeavy)
    {
        print("activating sword attack");
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, radius, layer);
        GetDamage();
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag == "Enemy")
            {
                //if (isFire && collider.GetComponent<EnemyFrame>().dmgOverTimeActivated != true)
                //{
                // collider.GetComponent<EnemyFrame>().StartCoroutine(collider.GetComponent<EnemyFrame>().dmgOverTime(fireDmg, fireTime, fireDmgInterval));
                //}
                if(audioManager == null)
                {
                    audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
                }
                if(uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
                audioManager.PlaySFX("SwordCollide");
                if(isHeavy)
                {
                    uiManager.DisplayDamageNum(collider.gameObject.transform, heavyDamage);
                    collider.GetComponent<EnemyFrame>().takeDamage(heavyDamage, GameObject.FindGameObjectWithTag("Player").transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Sword);
                }
                else
                {
                    uiManager.DisplayDamageNum(collider.gameObject.transform, damage);
                    collider.GetComponent<EnemyFrame>().takeDamage(damage, GameObject.FindGameObjectWithTag("Player").transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Sword);
                }
                
                //Vector3 knockBackDir = collider.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;
                //Debug.Log("Enemy knockback mag: " + knockBackDir.magnitude);
                //knockBackDir *= 1.5f;
                collider.GetComponent<EnemyFrame>().takeDamage(damage, GameObject.FindGameObjectWithTag("Player").transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Sword);
            }
        }
    }

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
