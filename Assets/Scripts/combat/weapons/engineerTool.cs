using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class engineerTool : MonoBehaviour
{
    public int damage;
    //bool isAttacking = false;


    public IEnumerator activateAttack(float time, Transform attackPoint, float radius, LayerMask layer)
    {
        print("activating tool attack");
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, radius, layer);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag == "Enemy")
            {
                collider.GetComponent<EnemyFrame>().takeDamage(damage);
            }
        }
        //isAttacking = true;
        yield return new WaitForSeconds(time);
        print("Deactivating");
        //isAttacking = false;
        yield break;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
