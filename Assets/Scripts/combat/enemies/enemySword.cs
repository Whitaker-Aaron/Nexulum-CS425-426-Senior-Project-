using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySword : MonoBehaviour
{
    int damage;
    public bool isAttacking = false;
    [SerializeField] Transform mainSkeletonTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateAttack(bool choice, int dmg)
    {
        damage = dmg;
        isAttacking = choice;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && isAttacking)
        {
            Vector3 knockBackDir = other.transform.position - mainSkeletonTransform.position;
            other.GetComponent<CharacterBase>().takeDamage(damage, knockBackDir);
        }
    }
}
