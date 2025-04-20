using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : swordCombat
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
        print("activating sword attack " + Time.time);
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, radius, layer);
        GetDamage();
        
        // Flag to track if we've already hit a boss in this attack
        bool bossHit = false;
        
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
                    EffectsManager.instance.getFromPool("swordHeavyHit", new Vector3(collider.transform.position.x - gameObject.transform.position.x * 1.2f, .75f, collider.transform.position.z - gameObject.transform.position.x * 1.2f), Quaternion.identity, false, true);
                }
                else
                {
                    uiManager.DisplayDamageNum(collider.gameObject.transform, damage);
                    collider.GetComponent<EnemyFrame>().takeDamage(damage, GameObject.FindGameObjectWithTag("Player").transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Sword);
                    EffectsManager.instance.getFromPool("swordHit", new Vector3(collider.transform.position.x - gameObject.transform.position.x * 1.2f, .75f, collider.transform.position.z - gameObject.transform.position.x * 1.2f), Quaternion.identity, false, true);
                }

                //Vector3 knockBackDir = collider.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;
                //Debug.Log("Enemy knockback mag: " + knockBackDir.magnitude);
                //knockBackDir *= 1.5f;
                //collider.GetComponent<EnemyFrame>().takeDamage(damage, GameObject.FindGameObjectWithTag("Player").transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Sword);
            }
            else if (collider.gameObject.tag == "bossPart" && !bossHit)
            {
                // Only process the first boss part hit
                bossHit = true;
                
                if (audioManager == null)
                {
                    audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
                }
                if (uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
                audioManager.PlaySFX("SwordCollide");
                
                if (isHeavy)
                {
                    uiManager.DisplayDamageNum(collider.gameObject.transform, heavyDamage, 75f);
                    collider.GetComponent<bossPart>().takeDamage(heavyDamage);
                    EffectsManager.instance.getFromPool("swordHeavyHit", new Vector3(collider.transform.position.x - gameObject.transform.position.x * 1.2f, .75f, collider.transform.position.z - gameObject.transform.position.x * 1.2f), Quaternion.identity, false, true);
                }
                else
                {
                    uiManager.DisplayDamageNum(collider.gameObject.transform, damage, 75f);
                    collider.GetComponent<bossPart>().takeDamage(damage);
                    EffectsManager.instance.getFromPool("swordHit", new Vector3(collider.transform.position.x - gameObject.transform.position.x * 1.2f, .75f, collider.transform.position.z - gameObject.transform.position.x * 1.2f), Quaternion.identity, false, true);
                }
            }
        }
        yield break;
    }
}
