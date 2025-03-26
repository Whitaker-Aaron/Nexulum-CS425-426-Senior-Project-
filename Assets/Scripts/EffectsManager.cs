using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager instance;


    [SerializedDictionary("effectName", "Prefab")]
    public SerializedDictionary<string, GameObject> prefabs;

    //private static Queue<GameObject> bulletHitPool;
    //public GameObject bulletHitPrefab, tankHitPrefab, mageHitPrefab, rocketHitPrefab, rocketFirePrefab, swordShotPrefab, swordShotIcePrefab, earthGrenadePrefab;
    //public GameObject caStart, caLoop, caEnd, faStart, faLoop, faEnd;
    //public GameObject earthShieldPrefab, esStart, esEnd, bsStart, bsLoop, bsEnd;
    //public GameObject cauStart, cauLoop, cauEnd, fauStart, fauLoop, fauEnd;
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
        

        createNewPool("bulletHitPool",getPrefab("bulletHit"), bulletPoolSize);
        createNewPool("archerHitPool", getPrefab("archerHit"), bulletPoolSize);
        createNewPool("pistolFlash", getPrefab("pistolFlash"), 4);
        createNewPool("rifleFlash", getPrefab("rifleFlash"), 12);
        createNewPool("revolverFlash", getPrefab("revolverFlash"), 8);
        createNewPool("subMachineFlash", getPrefab("subMachineFlash"), 12);
        createNewPool("subMachineHitPool", getPrefab("subMachineHit"), 20);
        createNewPool("tankHitPool", getPrefab("tankHit"), tankPoolSize);
        createNewPool("mageHitOne", getPrefab("mageHit"), magePoolSize);
        createNewPool("caPool", getPrefab("caStart"), 3);
        createNewPool("faPool", getPrefab("faStart"), 3);
        createNewPool("rocketHit", getPrefab("rocketHit"), 3);
        createNewPool("rocketFireCircle", getPrefab("rocketHitFire"), rocketFireSize);
        createNewPool("swordShotHit", getPrefab("swordShot"), swordShotSize);
        createNewPool("swordShotIce", getPrefab("swordShotIce"), swordShotSize);
        createNewPool("bubbleShield", getPrefab("bsStart"), 3);
        createNewPool("earthShield", getPrefab("esStart"), 3);
        createNewPool("grenade", getPrefab("grenade"), 4);
        createNewPool("earthGrenade", getPrefab("grenadeEarth"), 4);
        createNewPool("swordShotExplodeHit", getPrefab("swordShotExplode"), swordShotSize);
        createNewPool("archerHitPool", getPrefab("archerHit"), 4);
        createNewPool("swordHit", getPrefab("swordHit"), 5);
        createNewPool("swordHeavyHit", getPrefab("swordHeavyHit"), 5);
        createNewPool("iceSwordProjHit", getPrefab("iceSwordProjHit"), 10);
        createNewPool("playerDash", getPrefab("playerDash"), 3);
        //createNewPool("fireRuneEffect", getPrefab("fireRuneEffect"), 3);
        //createNewPool("iceRuneEffect", getPrefab("iceRuneEffect"), 3);
        //createNewPool("earthRuneEffect", getPrefab("earthRuneEffect"), 3);
    }


    private GameObject getPrefab(string name)
    {
        print("string name is: " + name);
        if (prefabs.ContainsKey(name))
        {
            // Return the corresponding prefab GameObject
            return prefabs[name];
        }
        else
        {
            Debug.LogWarning($"Prefab with name {name} not found in the dictionary.");
            return null;
        }
    }


    public virtual void createNewPool(string poolName,GameObject prefab, int size)
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
                        //if(poolName == "earthShield")
                            //temp.transform.position += new Vector3(0,1,0);
                        temp.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                        temp.transform.position += new Vector3(0, 1, 0);
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
                        GameObject temp = Instantiate(getPrefab("caLoop"));
                        temp.transform.parent = poolObj.transform;
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                    }
                    else if(poolName == "faPool")
                    {
                        GameObject temp = Instantiate(getPrefab("faLoop"));
                        temp.transform.parent = poolObj.transform;
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                    }
                    else if (poolName == "bubbleShield")
                    {
                        GameObject temp = Instantiate(getPrefab("bsLoop"));
                        temp.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                        temp.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                        temp.transform.position += new Vector3(0, 1, 0);
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                        //temp.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform, false);
                    }
                    else // earthShield
                    {
                        GameObject temp = Instantiate(getPrefab("esLoop"));
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
                        GameObject temp = Instantiate(getPrefab("caEnd"));
                        temp.transform.parent = poolObj.transform;
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                    }
                    else if (poolName == "faPool")
                    {
                        GameObject temp = Instantiate(getPrefab("faEnd"));
                        temp.transform.parent = poolObj.transform;
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                    }
                    else if (poolName == "bubbleShield")
                    {
                        GameObject temp = Instantiate(getPrefab("bsEnd"));
                        temp.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                        temp.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                        temp.transform.position += new Vector3(0, 1, 0);
                        //DontDestroyOnLoad(temp.gameObject);
                        allPools[poolName].Enqueue(temp);
                        temp.SetActive(false);
                        //temp.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform, false);
                    }
                    else
                    {
                        GameObject temp = Instantiate(getPrefab("esEnd"));
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

    private GameObject checkPoolPrefab(string poolName)
    {
        GameObject temp = null;
        if (allPools.ContainsKey(name))
        {
            switch(poolName)
            {
                case "bulletHitPool":
                    temp = getPrefab("bulletHit");
                    break;
                case "tankHitPool":
                    temp = getPrefab("tankHit");
                    break;
                case "mageHitOne":
                    temp = getPrefab("mageHit");
                    break;
                case "caPool":
                    temp = getPrefab("caStart");
                    break;
                case "faPool":
                    temp = getPrefab("faStart");
                    break;
                case "rocketHit":
                    temp = getPrefab("rocketHit");
                    break;
                case "rocketFireCircle":
                    temp = getPrefab("rocketHitFire");
                    break;
                case "swordShotHit":
                    temp = getPrefab("swordShot");
                    break;
                case "swordShotIceHit":
                    temp = getPrefab("swordShotIce");
                    break;
                case "bubbleShield":
                    temp = getPrefab("bsStart");
                    break;
                case "earthShield":
                    temp = getPrefab("esStart");
                    break;
                case "grenade":
                    temp = getPrefab("grenade");
                    break;
                case "earthGrenade":
                    temp = getPrefab("grenadeEarth");
                    break;
            }
            return temp;
        }
        else
        {
            //Debug.LogWarning($"Prefab with name {name} not found in the dictionary.");
            return null;
        }
    }


    public void getFromPool(string poolName, Vector3 position, Quaternion rotation, bool isParented, bool ignorePlayerLoc)
    {
        if (allPools[poolName].Count > 0)
        {
            GameObject obj = allPools[poolName].Dequeue();
            //if (poolName != "bubbleShield" && poolName != "earthShield")
            //obj.transform.position = position;
            //else
            //{
            //obj.transform.position = position;
            //obj.transform.rotation = rotation;
            //}
            if (isParented)
            {
                obj.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                if (ignorePlayerLoc)
                {
                    obj.transform.rotation = rotation;
                    obj.transform.localPosition = Vector3.zero;
                }
                else
                {
                    obj.transform.position = position;
                    obj.transform.rotation = rotation;
                }
            }
            else
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
            }

            //obj.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
            //if(poolName == "bubbleShield" || poolName == "earthShield")
            //{
            //obj.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
            //}
            obj.SetActive(true);
            obj.GetComponent<ParticleSystem>().Play();

            if (poolName != "bubbleShield" && poolName != "earthShield")
                StartCoroutine(returnToPool(poolName, obj.GetComponent<ParticleSystem>().main.duration + .03f, obj, isParented));
            else
            {
                print("poolCount = " + allPools[poolName].Count);
                if (allPools[poolName].Count == 1)
                {
                    StartCoroutine(returnToPool(poolName, classAbilties.instance.bubbleTime, obj, isParented));
                }
                else
                    StartCoroutine(returnToPool(poolName, obj.GetComponent<ParticleSystem>().main.duration + .01f, obj, isParented));
            }
        }
        else
        {
            GameObject newObj = Instantiate(checkPoolPrefab(poolName));
            newObj.GetComponent<ParticleSystem>().Play();
            StartCoroutine(returnToPool(poolName, newObj.GetComponent<ParticleSystem>().main.duration + .03f, newObj, isParented));

        }
    }

    IEnumerator returnToPool(string poolName, float time, GameObject effect, bool isParented)
    {
        yield return new WaitForSeconds(time);
        effect.SetActive(false);
        if (!isParented)
            effect.transform.position = Vector3.zero;
        allPools[poolName].Enqueue(effect);
        yield break;
    }

    public void replacePoolEffects(string poolName, int upgradeCount)
    {
        if (allPools.ContainsKey(poolName))
        {
            Queue<GameObject> tempQ;
            allPools.TryGetValue(poolName, out tempQ);

            int count = tempQ.Count;

            

            if(poolName == "caPool" || poolName == "faPool" || poolName == "earthShield" || poolName == "bubbleShield")
            {
                GameObject tempObj = null;
                GameObject oldObj = null;
                switch (upgradeCount)
                {
                    case 1:
                        switch (poolName)
                        {
                            case "caPool":
                                oldObj = tempQ.Dequeue();
                                tempObj = Instantiate(getPrefab("cauStart"), oldObj.transform.position, oldObj.transform.rotation);
                                tempObj.transform.parent = oldObj.transform.parent;
                                Destroy(oldObj);
                                tempQ.Enqueue(tempObj);
                                tempObj.SetActive(false);
                                oldObj = tempQ.Dequeue();
                                tempObj = Instantiate(getPrefab("cauLoop"), oldObj.transform.position, oldObj.transform.rotation);
                                tempObj.transform.parent = oldObj.transform.parent;
                                Destroy(oldObj);
                                tempQ.Enqueue(tempObj);
                                tempObj.SetActive(false);
                                oldObj = tempQ.Dequeue();
                                tempObj = Instantiate(getPrefab("cauEnd"), oldObj.transform.position, oldObj.transform.rotation);
                                tempObj.transform.parent = oldObj.transform.parent;
                                Destroy(oldObj);
                                tempQ.Enqueue(tempObj);
                                tempObj.SetActive(false);
                                break;
                            case "faPool":
                                oldObj = tempQ.Dequeue();
                                tempObj = Instantiate(getPrefab("fauStart"), oldObj.transform.position, oldObj.transform.rotation);
                                tempObj.transform.parent = oldObj.transform.parent;
                                Destroy(oldObj);
                                tempQ.Enqueue(tempObj);
                                tempObj.SetActive(false);
                                oldObj = tempQ.Dequeue();
                                tempObj = Instantiate(getPrefab("fauLoop"), oldObj.transform.position, oldObj.transform.rotation);
                                tempObj.transform.parent = oldObj.transform.parent;
                                Destroy(oldObj);
                                tempQ.Enqueue(tempObj);
                                tempObj.SetActive(false);
                                oldObj = tempQ.Dequeue();
                                tempObj = Instantiate(getPrefab("fauEnd"), oldObj.transform.position, oldObj.transform.rotation);
                                tempObj.transform.parent = oldObj.transform.parent;
                                Destroy(oldObj);
                                tempQ.Enqueue(tempObj);
                                tempObj.SetActive(false);
                                break;
                            case "earthShield":
                                oldObj = tempQ.Dequeue();
                                tempObj = Instantiate(getPrefab("esuStart"), oldObj.transform.position, oldObj.transform.rotation);
                                tempObj.transform.parent = oldObj.transform.parent;
                                Destroy(oldObj);
                                tempQ.Enqueue(tempObj);
                                tempObj.SetActive(false);
                                oldObj = tempQ.Dequeue();
                                tempObj = Instantiate(getPrefab("esuLoop"), oldObj.transform.position, oldObj.transform.rotation);
                                tempObj.transform.parent = oldObj.transform.parent;
                                Destroy(oldObj);
                                tempQ.Enqueue(tempObj);
                                tempObj.SetActive(false);
                                oldObj = tempQ.Dequeue();
                                tempObj = Instantiate(getPrefab("esuEnd"), oldObj.transform.position, oldObj.transform.rotation);
                                tempObj.transform.parent = oldObj.transform.parent;
                                Destroy(oldObj);
                                tempQ.Enqueue(tempObj);
                                tempObj.SetActive(false);
                                break;
                            case "bubbleShield":
                                oldObj = tempQ.Dequeue();
                                tempObj = Instantiate(getPrefab("bsuStart"), oldObj.transform.position, oldObj.transform.rotation);
                                tempObj.transform.parent = oldObj.transform.parent;
                                Destroy(oldObj);
                                tempQ.Enqueue(tempObj);
                                tempObj.SetActive(false);
                                oldObj = tempQ.Dequeue();
                                tempObj = Instantiate(getPrefab("bsuLoop"), oldObj.transform.position, oldObj.transform.rotation);
                                tempObj.transform.parent = oldObj.transform.parent;
                                Destroy(oldObj);
                                tempQ.Enqueue(tempObj);
                                tempObj.SetActive(false);
                                oldObj = tempQ.Dequeue();
                                tempObj = Instantiate(getPrefab("bsuEnd"), oldObj.transform.position, oldObj.transform.rotation);
                                tempObj.transform.parent = oldObj.transform.parent;
                                Destroy(oldObj);
                                tempQ.Enqueue(tempObj);
                                tempObj.SetActive(false);
                                break;

                        }
                        break;
                }
                
            }
            else
            {
                for(int i = 0; i < count; i++)
                {
                    GameObject oldObj = tempQ.Dequeue();  

                    GameObject temp = Instantiate(checkPoolPrefab(poolName), oldObj.transform.position, oldObj.transform.rotation);
                    temp.transform.parent = oldObj.transform.parent;  

                    Destroy(oldObj);  

                    tempQ.Enqueue(temp);  
                }

                allPools[poolName] = tempQ;
            }
        }
        else
        {
            Debug.Log("No pool exists for that name");
            return;
        }
    }
}
