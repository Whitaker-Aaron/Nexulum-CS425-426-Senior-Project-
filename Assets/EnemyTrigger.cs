using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    bool enemiesUnlocked = false;
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enemiesUnlocked) UnlockEnemies();
    }

    public void UnlockEnemies()
    {
        enemiesUnlocked = true;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<EnemyLOS>().canTarget = true;
                enemies[i].GetComponent<enemyInt>().isActive = true;
            }
        }
    }
}
