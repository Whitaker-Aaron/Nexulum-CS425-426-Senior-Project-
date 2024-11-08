using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager instance;

    //private static Queue<GameObject> bulletHitPool;
    public GameObject bulletHitPrefab, tankHitPrefab, mageHitPrefab, rocketHitPrefab, rocketFirePrefab, swordShotPrefab, swordShotIcePrefab;
    public GameObject caStart, caLoop, caEnd, faStart, faLoop, faEnd;
    private GameObject poolObj;
    public int bulletPoolSize = 10;
    public int tankPoolSize = 3;
    public int magePoolSize = 4;
    public int rocketFireSize = 3;
    public int swordShotSize = 10;
    //string poolName = null;


    protected Dictionary<string, Queue<GameObject>> allPools;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void initialize()
    {
        instance = this;

        allPools = new Dictionary<string, Queue<GameObject>>();

        //bulletHitPool = new Queue<GameObject>();
        poolObj = Instantiate(new GameObject("EffectsPool"));
        poolObj.transform.parent = gameObject.transform;
        

        createNewPool("bulletHitPool", bulletHitPrefab, bulletPoolSize);
        createNewPool("tankHitPool", tankHitPrefab, tankPoolSize);
        createNewPool("mageHitOne", mageHitPrefab, magePoolSize);
        createNewPool("caPool", caStart, 3);
        createNewPool("faPool", faStart, 3);
        createNewPool("rocketHit", rocketHitPrefab, 3);
        createNewPool("rocketFireCircle", rocketFirePrefab, rocketFireSize);
        createNewPool("swordShotHit", swordShotPrefab, swordShotSize);
        createNewPool("swordShotIceHit", swordShotIcePrefab, swordShotSize);
    }


    public virtual void createNewPool(string poolName, GameObject prefab, int size)
    {
        if(poolName == "caPool" || poolName == "faPool")
        {
            allPools[poolName] = new Queue<GameObject>();

            for(int i = 0; i < size; i++)
            {
                if(i==0)
                {
                    GameObject temp = Instantiate(prefab);
                    temp.transform.parent = poolObj.transform;
                    //DontDestroyOnLoad(temp.gameObject);
                    allPools[poolName].Enqueue(temp);
                    temp.SetActive(false);
                }
                else if(i==1)
                {
                    if (poolName == "caPool")
                    {
                        GameObject temp = Instantiate(caLoop);
                        temp.transform.parent = poolObj.transform;
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                    }
                    else
                    {
                        GameObject temp = Instantiate(faLoop);
                        temp.transform.parent = poolObj.transform;
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                    }
                }
                else
                {
                    if(poolName == "caPool")
                    {
                        GameObject temp = Instantiate(caEnd);
                        temp.transform.parent = poolObj.transform;
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                    }
                    else
                    {
                        GameObject temp = Instantiate(faEnd);
                        temp.transform.parent = poolObj.transform;
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                    }
                    
                }
            }
            return;
        }

        if(!allPools.ContainsKey(poolName))
        {
            allPools[poolName] = new Queue<GameObject>();

            for (int i = 0; i < size; i++)
            {
                GameObject temp = Instantiate(prefab);
                temp.transform.parent = poolObj.transform;
                //DontDestroyOnLoad(temp.gameObject);
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
        switch(poolName)
        {
            case "bulletHitPool":
                temp = bulletHitPrefab;
                break;

        }
        return temp;
    }
    

    public void getFromPool(string poolName, Vector3 position)
    {
        if (allPools[poolName].Count > 0)
        {
            GameObject obj = allPools[poolName].Dequeue();
            obj.transform.position = position;
            obj.SetActive(true);
            obj.GetComponent<ParticleSystem>().Play();
            StartCoroutine(returnToPool(poolName, obj.GetComponent<ParticleSystem>().main.duration + .03f, obj));
        }
        else
        {
            GameObject newObj = Instantiate(checkPoolPrefab(poolName));
            newObj.GetComponent<ParticleSystem>().Play();
            StartCoroutine(returnToPool(poolName, newObj.GetComponent<ParticleSystem>().main.duration + .03f, newObj));
        }
    }

    IEnumerator returnToPool(string poolName, float time, GameObject effect)
    {
        yield return new WaitForSeconds(time);
        effect.SetActive(false);
        effect.transform.position = Vector3.zero;
        allPools[poolName].Enqueue(effect);
        yield break;
    }
}
