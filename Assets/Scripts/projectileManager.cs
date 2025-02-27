using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public class projectileManager : MonoBehaviour
{
    public static projectileManager Instance;
    public GameObject poolContainer;
    //public Dictionary<string, Queue<GameObject>> allPools;


    [SerializeField]
    private List<ProjPoolData> poolList = new List<ProjPoolData>(); // Serialized in Unity Inspector

    private Dictionary<string, Queue<GameObject>> allPools = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> poolPrefabs = new Dictionary<string, GameObject>(); // For easy prefab access

    //private GameObject poolContainer;


    //public GameObject projPrefab, projPrefab2, turretPrefab, dronePrefab, tankPrefab, mageProjOne, swordShotPrefab, swordShotIcePrefab, revolverPrefab;
    //private GameObject poolObj;
    //public int poolSize = 25;
    //public int poolSize2 = 15;
    //public int turretSize = 15;
    //public int tankSize = 4;
    //public int mageSizeOne = 5;
    //public int swordShotSize = 15;

    //protected Dictionary<string, Queue<GameObject>> allPools;

    //private static Queue<GameObject> pool;
    //private static Queue<GameObject> pool2;


    private void Awake()
    {
        //initializePool();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        poolContainer = new GameObject("ProjectilePools");
        DontDestroyOnLoad(poolContainer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void initializePool()
    {
        allPools.Clear();
        poolPrefabs.Clear();

        foreach (var entry in poolList)
        {
            if (entry.prefab == null)
            {
                Debug.LogError($"Prefab for {entry.poolName} is missing!");
                continue;
            }

            poolPrefabs[entry.poolName] = entry.prefab;
            createNewPool(entry.poolName, entry.prefab, entry.size);
        }
        //allPools = new Dictionary<string, Queue<GameObject>>();
        //poolPrefabs = new Dictionary<string, (GameObject, int)>();
        /*
        DontDestroyOnLoad(gameObject);

        poolContainer = Instantiate(new GameObject("poolObjects"));
        poolContainer.transform.parent = this.transform;

        foreach(var entry in poolPrefabs)
        {
            string poolName = entry.Key;
            GameObject prefab = entry.Value.prefab;
            int size = entry.Value.size;
            createNewPool(poolName, prefab, size);

        }
        */

        /*createNewPool("bulletPool", projPrefab, poolSize);
        createNewPool("pistolPool", projPrefab2, poolSize2);
        createNewPool("revolverPool", revolverPrefab, poolSize2);
        createNewPool("turretPool", turretPrefab, turretSize);
        createNewPool("dronePool", dronePrefab, turretSize);
        createNewPool("tankPool", tankPrefab, tankSize);
        createNewPool("enemyMagePoolOne", mageProjOne, mageSizeOne);
        createNewPool("swordShotPool", swordShotPrefab, swordShotSize);
        createNewPool("swordShotIcePool", swordShotIcePrefab, swordShotSize);
        */
    }

    public virtual void createNewPool(string poolName, GameObject prefab, int size)
    {
        // Check if the pool already exists
        if (allPools.ContainsKey(poolName))
        {
            Debug.Log("Pool with name: " + poolName + " already exists");
            return; // Exit if the pool already exists
        }

        // Create a new queue for the pool
        allPools[poolName] = new Queue<GameObject>();

        // Determine the prefab to use
        GameObject selectedPrefab = prefab;

        if (prefab == null)
        {
            // If prefab is null, get the first prefab from the poolPrefabs dictionary
            if (poolPrefabs.Count > 0)
            {
                // Get the first prefab from the dictionary
                selectedPrefab = poolPrefabs.Values.First();
                Debug.LogWarning("Prefab was null. Using first prefab: " + selectedPrefab.name);
            }
            else
            {
                Debug.LogError("No prefabs available in poolPrefabs dictionary to use as default.");
                return; // Exit if no prefabs are available
            }
        }

        // Create the pool using the selected prefab
        for (int i = 0; i < size; i++)
        {
            GameObject temp = Instantiate(selectedPrefab);
            temp.transform.parent = poolContainer.transform;
            temp.SetActive(false);
            allPools[poolName].Enqueue(temp);
        }
    }

    //GameObject checkPoolPrefab(string poolName)
    //{
    //  GameObject temp = null;
    //if(poolPrefabs.ContainsKey(poolName))
    //   temp = poolPrefabs[poolName];
    //return temp;
    //}

    public GameObject getProjectile(string poolName, Vector3 position, Quaternion rotation)
    {
        if (!allPools.ContainsKey(poolName))
        {
            Debug.LogError($"Projectile pool '{poolName}' not found!");
            return null;
        }
        //print("Getting proj");
        //print(pool.Count);

        //print("getting proj from " + poolName);

        /*
        if (allPools[poolName].Count > 0)
        {
            GameObject proj = allPools[poolName].Dequeue();
            //if (proj == null)
              //  print("proj null");
            if(poolName != "swordShotPool" && poolName != "swordShotIcePool")
                proj.GetComponent<projectile>().setName(poolName);
            else
                proj.GetComponent<swordShot>().setName(poolName);
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
        */


        GameObject proj;
        if (allPools[poolName].Count > 0)
        {
            print("Getting bullet");
            //GameObject proj;
            proj = allPools[poolName].Dequeue();
            proj.transform.position = position;
            proj.transform.rotation = rotation;
            proj.SetActive(true);

            return proj;
        }
        else if (poolPrefabs.TryGetValue(poolName, out GameObject prefab))
        {
            proj = Instantiate(prefab, position, rotation); 
            return proj;
        }
        else
        {
            Debug.Log("No prefab for bullet found");
            return null;
        }
            



    }

    public void updateProjectileDamage(string pool, int damageInc)
    {
        if (!allPools.TryGetValue(pool, out Queue<GameObject> bulletQ))
        {
            Debug.LogWarning($"Pool '{pool}' not found. Cannot update projectile damage.");
            return;
        }

        Debug.Log($"Updating projectiles in pool: {pool} for damage: {damageInc}");

        foreach (GameObject bullet in bulletQ)
        {
            if (bullet != null)
            {
                bullet.GetComponent<projectile>().damage = damageInc;
            }
        }
    }

    public void returnProjectile(string poolName, GameObject projectile)
    {
        projectile.SetActive(false);
        allPools[poolName].Enqueue(projectile);
    }

    public Dictionary<string, Queue<GameObject>> GetAllPools()
    {
        return allPools;
    }


}
