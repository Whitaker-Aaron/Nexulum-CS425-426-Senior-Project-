using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public class projectileManager : MonoBehaviour
{
    public static projectileManager Instance;
    public GameObject projPrefab, projPrefab2, turretPrefab, dronePrefab, tankPrefab, mageProjOne;
    private GameObject poolObj;
    public int poolSize = 25;
    public int poolSize2 = 15;
    public int turretSize = 15;
    public int tankSize = 4;
    public int mageSizeOne = 5;

    protected Dictionary<string, Queue<GameObject>> allPools;

    //private static Queue<GameObject> pool;
    //private static Queue<GameObject> pool2;


    private void Awake()
    {
        //initializePool();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void initializePool()
    {
        Instance = this;
        allPools = new Dictionary<string, Queue<GameObject>>();
        DontDestroyOnLoad(gameObject);

        poolObj = Instantiate(new GameObject("poolObjects"));
        poolObj.transform.parent = this.transform;

        createNewPool("bulletPool", projPrefab, poolSize);
        createNewPool("pistolPool", projPrefab2, poolSize2);
        createNewPool("turretPool", turretPrefab, turretSize);
        createNewPool("dronePool", dronePrefab, turretSize);
        createNewPool("tankPool", tankPrefab, tankSize);
        createNewPool("enemyMagePoolOne", mageProjOne, mageSizeOne);
    }

    public virtual void createNewPool(string poolName, GameObject prefab, int size)
    {
        if (!allPools.ContainsKey(poolName))
        {
            allPools[poolName] = new Queue<GameObject>();

            for (int i = 0; i < size; i++)
            {
                GameObject temp = Instantiate(prefab);
                temp.transform.parent = poolObj.transform;
                //DontDestroyOnLoad(temp);
                allPools[poolName].Enqueue(temp);
                temp.SetActive(false);
            }
        }
        else
        {
            Debug.Log("pool with name: " + poolName + " already exists");
        }
    }

    GameObject checkPoolPrefab(string poolName)
    {
        GameObject temp = null;
        switch (poolName)
        {
            case "bulletPool":
                temp = projPrefab;
                break;
            case "pistolPool":
                temp = projPrefab2; break;

        }
        return temp;
    }

    public GameObject getProjectile(string poolName, Vector3 position, Quaternion rotation)
    {
        //print("Getting proj");
        //print(pool.Count);

        if(allPools[poolName].Count > 0)
        {
            GameObject proj = allPools[poolName].Dequeue();
            //if (proj == null)
              //  print("proj null");
              proj.GetComponent<projectile>().setName(poolName);
            proj.transform.position = position;
            proj.transform.rotation = rotation;
            proj.SetActive(true);
            return proj;
        }
        else
        {
            GameObject proj = Instantiate(projPrefab, position, rotation);
            return proj;
        }
    }

    public void updateProjectileDamage(string pool, int damageInc)
    {
        Debug.Log("Updating projectiles in pool: " + pool + " for damage: " + damageInc);
        Queue<GameObject> bulletQ;
        if(!allPools.ContainsKey(pool))
        {
            return;
        }
        allPools.TryGetValue(pool, out bulletQ);// [pool];
        foreach(GameObject bullet in bulletQ)
        {
            bullet.GetComponent<projectile>().damage = damageInc;
        }
    }

    public void returnProjectile(string poolName, GameObject projectile)
    {
        projectile.SetActive(false);
        allPools[poolName].Enqueue(projectile);
    }

}
