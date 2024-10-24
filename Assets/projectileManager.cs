using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class projectileManager : MonoBehaviour
{
    public static projectileManager Instance;
    public GameObject projPrefab, projPrefab2, poolObj;
    public int poolSize = 25;
    public int poolSize2 = 15;

    private static Queue<GameObject> pool;
    private static Queue<GameObject> pool2;


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
        pool = new Queue<GameObject>();
        pool2 = new Queue<GameObject>();
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(gameObject);

        poolObj = Instantiate(new GameObject("poolObjects"));
        poolObj.transform.parent = this.transform;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject proj = Instantiate(projPrefab, new Vector3(0, 999, 0), Quaternion.identity);
            DontDestroyOnLoad (proj);
            proj.transform.parent = poolObj.transform;
            pool.Enqueue(proj);
            proj.SetActive(false);
            
        }

        for (int i = 0; i < poolSize2; i++)
        {
            GameObject proj = Instantiate(projPrefab2, new Vector3(0, 999, 0), Quaternion.identity);
            DontDestroyOnLoad(proj);
            proj.transform.parent = poolObj.transform;
            pool2.Enqueue(proj);
            proj.SetActive(false);
        }
        //print(pool.Count);
    }

    public GameObject getProjectile(Vector3 position, Quaternion rotation)
    {
        //print("Getting proj");
        //print(pool.Count);

        if(pool.Count > 0)
        {
            GameObject proj = pool.Dequeue();
            //if (proj == null)
              //  print("proj null");
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

    public GameObject getProjectile2(Vector3 position, Quaternion rotation)
    {
        //print("Getting proj");
        //print(pool.Count);

        if (pool.Count > 0)
        {
            GameObject proj = pool2.Dequeue();
            //if (proj == null)
              //  print("proj null");
            proj.transform.position = position;
            proj.transform.rotation = rotation;
            proj.SetActive(true);
            return proj;
        }
        else
        {
            GameObject proj = Instantiate(projPrefab2, position, rotation);
            return proj;
        }
    }

    public void returnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        pool.Enqueue(projectile);
    }

    public void returnProjectile2(GameObject projectile)
    {
        projectile.SetActive(false);
        pool2.Enqueue(projectile);
    }
}
