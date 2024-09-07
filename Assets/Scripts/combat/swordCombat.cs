using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordCombat : MonoBehaviour
{
    public int damage = 50;

    //rune ability combat mechanic
    public bool isFire = false;
    public int fireDmg = 10;
    public float fireTime = 5f;
    public float fireDmgInterval = 1f;

    // Start is called before the first frame update
    void Start()
    {
        isFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if(isFire)
            {
                other.GetComponent<EnemyFrame>().StartCoroutine(other.GetComponent<EnemyFrame>().dmgOverTime(fireDmg, fireTime, fireDmgInterval));
            }
            other.GetComponent<EnemyFrame>().takeDamage(damage);
            return;
        }

    }

    
}
