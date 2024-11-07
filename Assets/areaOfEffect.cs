using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class areaOfEffect : MonoBehaviour
{

    bool check = false;
    public float checkTime = 5f;
    int enemy;


    Vector3 position;
    float radius, damageT, damageR;
    int fireDamage;

    public void startCheck(Vector3 pos, float rad, int fireDmg, float dmgTime, float dmgRate, float abilityTime)
    {
        enemy = LayerMask.GetMask("Enemy");
        position = pos;
        radius = rad;
        damageT = dmgTime;
        damageR = dmgRate;
        fireDamage = fireDmg;
        check = true;
        Destroy(gameObject, abilityTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(position, radius);
    }

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(check)
        {
            Collider[] enemies = Physics.OverlapSphere(position, radius, enemy);

            foreach (Collider c in enemies)
            {
                if(c.gameObject.GetComponent<EnemyFrame>().dmgOverTimeActivated == false)
                {
                    c.gameObject.GetComponent<EnemyFrame>().dmgOverTimeActivated = true;
                    StartCoroutine(c.GetComponent<EnemyFrame>().dmgOverTime(fireDamage, damageT, damageR));
                }
                
            }
        }
    }
}
