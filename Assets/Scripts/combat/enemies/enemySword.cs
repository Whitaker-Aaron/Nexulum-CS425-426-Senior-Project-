using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySword : MonoBehaviour
{
    int damage;
    public bool isAttacking = false;
    [SerializeField] Transform mainSkeletonTransform;

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
        if(isAttacking) Debug.Log("Sword collision detected on " + other.tag + "while attacking");
        else if (!isAttacking) Debug.Log("Sword collision detected on " + other.tag + "while not attacking");
        if (other.tag == "Player" && isAttacking)
        {
            Vector3 knockBackDir = other.transform.position - mainSkeletonTransform.position;
            Debug.Log("Knock back direction: " + knockBackDir);
            //other.GetComponent<CharacterBase>().takeDamage(damage, knockBackDir);
            
            //print("earthBool: " + classAbilties.instance.earthBool + "  aura: " + classAbilties.instance.bubble + "  enemyInstance: " + enemyInstance);
            if (classAbilties.instance.earthBool == true && classAbilties.instance.bubble == true && enemyInstance != null)
            {
                //print("reflecting enemy damage back");
                enemyInstance.GetComponent<EnemyFrame>().takeDamage(damage, mainSkeletonTransform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Sword);
                return;
            }
            else
            {
                print("hitting player");
                other.GetComponent<CharacterBase>().takeDamage(damage, knockBackDir);
            }
        }
    }
}
