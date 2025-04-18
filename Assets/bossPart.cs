using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossPart : MonoBehaviour
{
    [SerializeField]
    private GameObject boss;
    
    // Public reference to the parent boss for damage over time effects
    public GameObject parent { get { return boss; } }
    
    // UI Manager reference for damage numbers
    private UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("UIManager")?.GetComponent<UIManager>();
        
        // Make sure the boss reference is set
        if (boss == null)
        {
            boss = transform.parent.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int damage)
    {
        if (boss != null && boss.GetComponent<golemBoss>() != null)
        {
            boss.GetComponent<golemBoss>().takeDamage(damage);
        }
    }
    
    // Method to apply fire damage over time to the boss
    public IEnumerator dmgOverTime(int dmg, float statusTime, float dmgRate, EnemyFrame.DamageType dmgType)
    {
        // Forward the damage over time to the parent boss
        if (boss != null && boss.GetComponent<golemBoss>() != null)
        {
            yield return boss.GetComponent<golemBoss>().StartCoroutine(
                boss.GetComponent<golemBoss>().dmgOverTime(dmg, statusTime, dmgRate, dmgType)
            );
        }
        else
        {
            yield break;
        }
    }
}
