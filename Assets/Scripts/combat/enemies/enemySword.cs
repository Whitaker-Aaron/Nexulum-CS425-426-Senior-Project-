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
            print("earthBool: " + classAbilties.instance.earthBool + "  aura: " + classAbilties.instance.bubble + "  enemyInstance: " + enemyInstance);
            if (classAbilties.instance.earthBool == true && classAbilties.instance.bubble == true && enemyInstance != null)
            {
                print("reflecting enemy damage back");
                enemyInstance.GetComponent<EnemyFrame>().takeDamage(damage, -Vector3.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Sword);
                return;
            }
            else
            {
                print("hitting player");
                other.GetComponent<CharacterBase>().takeDamage(damage);
            }
        }
    }
}
