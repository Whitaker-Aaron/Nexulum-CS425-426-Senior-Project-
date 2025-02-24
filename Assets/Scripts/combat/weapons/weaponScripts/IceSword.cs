using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSword : swordCombat
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override IEnumerator activateAttack(Transform attackPoint, float radius, LayerMask layer, bool isHeavy, float time, int count)
    {
        yield return new WaitForSeconds(time);
        GameObject proj = projectileManager.Instance.getProjectile("iceSwordProjPool", attackPoint.position, attackPoint.rotation);
        print("activating sword attack " + Time.time);
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
                if (audioManager == null)
                {
                    audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
                }
                if (uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
                audioManager.PlaySFX("SwordCollide");
                if (isHeavy)
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
                //collider.GetComponent<EnemyFrame>().takeDamage(damage, GameObject.FindGameObjectWithTag("Player").transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Sword);
            }
        }


        yield break;
    }
}
