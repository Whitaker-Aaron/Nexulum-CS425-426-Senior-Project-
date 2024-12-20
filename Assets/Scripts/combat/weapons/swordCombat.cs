using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordCombat : MonoBehaviour
{
    public int damage = 0;
    AudioManager audioManager;


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

    public void activateAttack(Transform attackPoint, float radius, LayerMask layer)
    {
        print("activating sword attack");
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, radius, layer);
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
                audioManager.PlaySFX("SwordCollide");
                //Vector3 knockBackDir = collider.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;
                //Debug.Log("Enemy knockback mag: " + knockBackDir.magnitude);
                //knockBackDir *= 1.5f;
                collider.GetComponent<EnemyFrame>().takeDamage(damage, GameObject.FindGameObjectWithTag("Player").transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Sword);
            }
        }
    }

    public void updateDamage(int dmg)
    {
        Debug.Log("Sword damaged updated to: " + dmg);
        damage = dmg;
    }

    public void activateFire(bool activate)
    {
        //isFire = activate;
    }


    
}
