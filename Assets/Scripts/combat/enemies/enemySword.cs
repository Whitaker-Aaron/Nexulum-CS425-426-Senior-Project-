using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySword : MonoBehaviour
{
    int damage;
    public bool isAttacking = false;

    GameObject enemyInstance = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateAttack(bool choice, int dmg, GameObject enemy)
    {
        damage = dmg;
        isAttacking = choice;
        enemyInstance = enemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && isAttacking)
        {
            if (classAbilties.instance.earthBool == true && classAbilties.instance.checkingAura == true && enemyInstance != null)
            {
                enemyInstance.GetComponent<EnemyFrame>().takeDamage(damage, -Vector3.forward, EnemyFrame.DamageSource.Enemy, EnemyFrame.DamageType.Sword);
                return;
            }
            else
                other.GetComponent<CharacterBase>().takeDamage(damage);
        }
    }
}
