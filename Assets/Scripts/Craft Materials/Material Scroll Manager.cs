using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MaterialScrollManager : MonoBehaviour
{
    [SerializeField] ScrollView scrollView;
    [SerializeField] GameObject content;
    [SerializeField] GameObject testContent;
    [SerializeField] Texture testTexture;

    List<GameObject> currentMaterials = new List<GameObject>();
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
        //scrollObject.description.text = "Sapphire Bar";
        //scrollObject.description.color = new Color(56, 56, 56, 20.0f);
        //scrollObject.quantity.text = "x" + "3";
        //scrollObject.imageRef.texture = testTexture;
        //scrollObject.quantity.color = new Color(56, 56, 56, 20.0f);
        //Instantiate(testContent).transform.SetParent(content.transform);

        scrollObject = testContent.GetComponent<MaterialScrollObject>();
        scrollObject.description.text = "Sapphire";
        scrollObject.imageRef.texture = testTexture;
        //scrollObject.description.color = new Color(56, 56, 56, 20.0f);
        scrollObject.quantity.text = "x" + "2";
        //scrollObject.quantity.color = new Color(56, 56, 56, 20.0f);
        GameObject newScrollMaterial = Instantiate(testContent);
        newScrollMaterial.transform.SetParent(content.transform);
        //.transform.SetParent(content.transform);

        currentMaterials.Add(newScrollMaterial);
        currentMaterials[0].GetComponent<MaterialScrollObject>().quantity.text = "x" + "4";

        //color = 0.5f;
        //testContent.transform.parent = content.transform;
    }

    private void OnDestroy()
    {
        var scrollObject = testContent.GetComponent<MaterialScrollObject>();
        scrollObject.description.text = "empty";
        scrollObject.quantity.text = "x0";
    }
}
