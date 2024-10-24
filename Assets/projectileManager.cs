using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class projectileManager : MonoBehaviour
{
    public static projectileManager Instance;
    public GameObject projPrefab, poolObj;
    public int poolSize = 25;

    private static Queue<GameObject> pool; 


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
        print(pool.Count);
    }

    public GameObject getProjectile(Vector3 position, Quaternion rotation)
    {
        print("Getting proj");
        print(pool.Count);
        int tempCount = 0;
        foreach(GameObject obj in pool)
        {
            if(obj != null)
            {
                tempCount++;
                print("proj: " + tempCount);
            }
            print("proj null");
        }
        tempCount = 0;
        if(pool.Count > 0)
        {
            GameObject proj = pool.Dequeue();
            if (proj == null)
                print("proj null");
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

    public void returnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        pool.Enqueue(projectile);
    }
}
