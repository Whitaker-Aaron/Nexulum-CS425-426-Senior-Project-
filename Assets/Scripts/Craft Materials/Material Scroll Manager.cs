using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MaterialScrollManager : MonoBehaviour
{
    [SerializeField] ScrollView scrollView;
    [SerializeField] GameObject content;
    [SerializeField] GameObject testContent;
    // Start is called before the first frame update
    void Start()
    {
        UpdateScroll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateScroll()
    {
        //Test code
        var scrollObject = testContent.GetComponent<MaterialScrollObject>();
        scrollObject.description.text = "Sapphire Bar";
        //scrollObject.description.color = new Color(56, 56, 56, 20.0f);
        scrollObject.quantity.text = "x" + "3";
        //scrollObject.quantity.color = new Color(56, 56, 56, 20.0f);
        Instantiate(testContent).transform.SetParent(content.transform);
        

        //color = 0.5f;
        //testContent.transform.parent = content.transform;
    }
}
