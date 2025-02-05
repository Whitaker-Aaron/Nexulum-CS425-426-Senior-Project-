using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class enemyProjectileDamage : MonoBehaviour
{
    public static enemyProjectileDamage instance;

    [SerializedDictionary("enemyName", "damage(int)")]
    public SerializedDictionary<string, int> damages;
// Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getDamage(string name)
    {
        print("string name is: " + name);
        if (damages.ContainsKey(name))
        {
            // Return the corresponding prefab GameObject
            return damages[name];
        }
        else
        {
            Debug.LogWarning($"Prefab with name {name} not found in the dictionary.");
            return 0;
        }
    }
}
