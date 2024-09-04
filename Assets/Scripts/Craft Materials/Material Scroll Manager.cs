using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MaterialScrollManager : MonoBehaviour
{
    [SerializeField] ScrollView scrollView;
    [SerializeField] GameObject content;
    [SerializeField] GameObject scrollContent;

    private IEnumerator coroutine;

    List<GameObject> currentMaterials = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //UpdateScroll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScroll(Texture materialTexture, string materialName)
    {
        CheckForNull();
        var scrollObject = scrollContent.GetComponent<MaterialScrollObject>();
        int existingIndex = -1; 
        bool exists = false; 

        scrollObject.description.text = materialName;
        scrollObject.imageRef.texture = materialTexture;
        scrollObject.quantityInt = 1;
        scrollObject.quantity.text = "x" + scrollObject.quantityInt.ToString();
        GameObject newScrollMaterial = Instantiate(scrollContent);
        newScrollMaterial.transform.SetParent(content.transform);

        var existingScroll = newScrollMaterial.GetComponent<MaterialScrollObject>();

        var desc = existingScroll.description;
        for (int i = 0; i < currentMaterials.Count; i++) {
            if (currentMaterials[i].GetComponent<MaterialScrollObject>().description.text == desc.text)
            {
                exists = true;
                existingIndex = i;
            }
        }
        if (exists)
        {
            Debug.Log("Current material found");
            
            existingScroll.quantityInt += currentMaterials[existingIndex].GetComponent<MaterialScrollObject>().quantityInt;
            existingScroll.quantity.text = "x" + existingScroll.quantityInt.ToString();
            Destroy(currentMaterials[existingIndex].gameObject);
            currentMaterials.RemoveAt(existingIndex);
        }

        currentMaterials.Add(newScrollMaterial);
        Debug.Log("Amount of materials: ");
        Debug.Log(currentMaterials.Count);
        StartCoroutine(StartCooldown(newScrollMaterial));
        Debug.Log("Starting Coroutine");
        //currentMaterials[0].GetComponent<MaterialScrollObject>().quantity.text = "x" + "4";

        //color = 0.5f;
        //testContent.transform.parent = content.transform;
    }

    private void OnDestroy()
    {
        var scrollObject = scrollContent.GetComponent<MaterialScrollObject>();
        scrollObject.description.text = "empty";
        scrollObject.quantity.text = "x0";
    }

    private void CheckForNull()
    {
        for (int i = 0; i < currentMaterials.Count; i++)
        {
            if (currentMaterials[i] == null)
            {
                currentMaterials.RemoveAt(i);
                i = i - 1;
            }
        }

    }

     private IEnumerator StartCooldown(GameObject scrollObj)
    {
      StartCoroutine("ReduceOpacity", scrollObj);
      yield return new WaitForSeconds(5);
      Debug.Log("Finished coroutine");
      Destroy(scrollObj);
      CheckForNull();
      StopCoroutine("ReduceOpacity");
            
    }

    private IEnumerator ReduceOpacity(GameObject scrollObj)
    {
        var reference = scrollObj.GetComponent<MaterialScrollObject>();
        while (true && reference != null)
        {
            Color imgColor = reference.imageRef.color;
            imgColor.a -= 0.20f * Time.deltaTime;
            reference.imageRef.color = imgColor;

            Color desColor = reference.description.color;
            desColor.a -= 0.20f * Time.deltaTime;
            reference.description.color = desColor;

            Color quanColor = reference.quantity.color;
            quanColor.a -= 0.20f * Time.deltaTime;
            reference.quantity.color = quanColor;

            yield return null;
        }
        yield break;
    }
}