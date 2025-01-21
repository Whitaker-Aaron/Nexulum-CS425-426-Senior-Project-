using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterShield : MonoBehaviour
{
    public bool checking = false;

    float rad = 0;
    float distanceBuffer = 0;
    Vector3 pos = Vector3.zero;

    int damage = 0;


    Collider[] objs;

    Collider[] objsOut;


    public void setVars(float radius, float buffer, Vector3 position, int dmg)
    {
        rad = radius;
        pos = position;
        damage = dmg;
        distanceBuffer = buffer;
    }

    public Collider[] initialCheck()
    {
        if (checking)
        {
            objs = Physics.OverlapSphere(pos, rad);

            return objs;
        }
        else
            return null;
    }

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
        if(checking)
        {
            bool temp = true;
            foreach(Collider c in objs)
            {
                if (c.gameObject != other)
                    temp = true;
                else
                {
                    temp = false;
                    break;
                }
            }

            if(temp == true)
            {

            }
        }
    }

}
