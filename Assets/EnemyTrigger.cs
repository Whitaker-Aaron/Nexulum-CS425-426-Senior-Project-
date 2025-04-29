using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    bool enemiesUnlocked = false;
    public bool isBoss = false;
    public bool triggered = false;
    CameraFollow camera;
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!enemiesUnlocked && !isBoss) UnlockEnemies();
        else if (!enemiesUnlocked && !triggered && isBoss) StartCoroutine(bossPan());
        triggered = true;
        
    }

    public void UnlockEnemies()
    {
        enemiesUnlocked = true;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                if(enemies[i].GetComponent<EnemyLOS>() != null) enemies[i].GetComponent<EnemyLOS>().canTarget = true;
                if(enemies[i].GetComponent<enemyInt>() != null) enemies[i].GetComponent<enemyInt>().isActive = true;
            }
        }
        Destroy(this.gameObject);
    }

    public void EnableEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null && !enemies[i].activeSelf)
            {
                enemies[i].SetActive(true);
            }
        }
    }

    public IEnumerator bossPan()
    {
        var uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        uiManager.StartWarningIcon();
        uiManager.DisableHUD(true);
        camera.StartPan(enemies[0].transform.position, true, true, 0.05f, 7f);
        yield return new WaitForSeconds(3f);
        GameObject.Find("AudioManager").GetComponent<AudioManager>().ChangeTrack("Boss1");
        StartCoroutine(uiManager.AnimateBossName("Gollurk"));
        EnableEnemies();
        yield return new WaitForSeconds(5f);
        uiManager.EnableHUD();
        yield return new WaitForSeconds(2f);
        UnlockEnemies();
        yield break;
    }
}
