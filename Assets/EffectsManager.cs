using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager instance;

    //private static Queue<GameObject> bulletHitPool;
    public GameObject bulletHitPrefab, tankHitPrefab, mageHitPrefab, rocketHitPrefab, rocketFirePrefab, swordShotPrefab, swordShotIcePrefab, earthGrenadePrefab;
    public GameObject caStart, caLoop, caEnd, faStart, faLoop, faEnd;
    public GameObject earthShieldPrefab, esStart, esEnd, bsStart, bsLoop, bsEnd;
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
        createNewPool("bubbleShield", bsStart, 3);
        createNewPool("earthShield", esStart, 3);
        createNewPool("earthGrenade", earthGrenadePrefab, 4);
    }


    public virtual void createNewPool(string poolName, GameObject prefab, int size)
    {
        if(poolName == "caPool" || poolName == "faPool" || poolName == "earthShield" || poolName == "bubbleShield")
        {
            allPools[poolName] = new Queue<GameObject>();

            for(int i = 0; i < size; i++)
            {
                if(i==0)
                {
                    GameObject temp = Instantiate(prefab);
                    if (poolName == "bubbleShield" || poolName == "earthShield")
                    {
                        temp.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                        if(poolName == "earthShield")
                            temp.transform.position += new Vector3(0,1,0);
                        temp.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                    }
                    else
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
                    else if(poolName == "faPool")
                    {
                        GameObject temp = Instantiate(faLoop);
                        temp.transform.parent = poolObj.transform;
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                    }
                    else if (poolName == "bubbleShield")
                    {
                        GameObject temp = Instantiate(bsLoop);
                        temp.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                        temp.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                        //temp.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform, false);
                    }
                    else // earthShield
                    {
                        GameObject temp = Instantiate(earthShieldPrefab);
                        temp.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                        temp.transform.position += new Vector3(0, 1, 0);
                        temp.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                        //temp.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform, false);
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
                    else if (poolName == "faPool")
                    {
                        GameObject temp = Instantiate(faEnd);
                        temp.transform.parent = poolObj.transform;
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                    }
                    else if (poolName == "bubbleShield")
                    {
                        GameObject temp = Instantiate(bsEnd);
                        temp.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                        temp.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                        //temp.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform, false);
                    }
                    else
                    {
                        GameObject temp = Instantiate(esEnd);
                        temp.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                        temp.transform.position += new Vector3(0, 1, 0);
                        temp.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                        //temp.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform, false);
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
            if (poolName != "bubbleShield" && poolName != "earthShield")
                obj.transform.position = position;
            else
                obj.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
            //if(poolName == "bubbleShield" || poolName == "earthShield")
            //{
                //obj.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
            //}
            obj.SetActive(true);
            obj.GetComponent<ParticleSystem>().Play();

            if (poolName != "bubbleShield" && poolName != "earthShield")
                StartCoroutine(returnToPool(poolName, obj.GetComponent<ParticleSystem>().main.duration + .03f, obj));
            else
            {
                print("poolCount = " + allPools[poolName].Count);
                if (allPools[poolName].Count == 1)
                {
                    StartCoroutine(returnToPool(poolName, classAbilties.instance.bubbleTime, obj));
                }
                else
                    StartCoroutine(returnToPool(poolName, obj.GetComponent<ParticleSystem>().main.duration + .01f, obj));
            }
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
        if (poolName != "bubbleShield" && poolName != "earthShield")
            effect.transform.position = Vector3.zero;
        allPools[poolName].Enqueue(effect);
        yield break;
    }
}
