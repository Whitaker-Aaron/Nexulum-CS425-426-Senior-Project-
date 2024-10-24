using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class MaterialScrollManager : MonoBehaviour
{
    [SerializeField] ScrollView scrollView;
    [SerializeField] GameObject content;
    [SerializeField] GameObject scrollContent;
    [SerializeField] GameObject materialGradient;

    [SerializeField] Texture placeholder;

    private IEnumerator coroutine;

    MaterialsInventory materialInventory;

    List<GameObject> currentMaterials = new List<GameObject>();
    MaterialScrollObject scrollObject;
    // Start is called before the first frame update
    void Start()
    {
        materialInventory = GameObject.Find("MaterialsInventory").GetComponent<MaterialsInventory>();

        scrollObject = scrollContent.GetComponent<MaterialScrollObject>();

        scrollObject.description.text = "empty";
        scrollObject.imageRef.texture = placeholder;
        scrollObject.quantityInt = 1;
        scrollObject.quantity.text = "x" + scrollObject.quantityInt.ToString();

        GameObject newScrollMaterial = Instantiate(scrollContent);
        newScrollMaterial.transform.SetParent(content.transform, false);
        currentMaterials.Add(newScrollMaterial);
        currentMaterials.RemoveAt(0);
        //Destroy(newScrollMaterial);
        //UpdateScroll();
        var child = content.transform.GetChild(0);
        Debug.Log(child);
        child.gameObject.SetActive(false);

        Debug.Log("Inside the MSM's Start()");

        currentMaterials.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log(currentMaterials.Count);
        if(currentMaterials.Count <= 0)
        {
            materialGradient.SetActive(false);
        }
        else
        {
            materialGradient.SetActive(true);
        }
    }

    public void UpdateScroll(Texture materialTexture, string materialName)
    {
        CheckForNull();

        int existingIndex = -1; 
        bool exists = false; 

        scrollObject.description.text = materialName;
        scrollObject.imageRef.texture = materialTexture;
        scrollObject.quantityInt = 1;
        scrollObject.quantity.text = "x" + scrollObject.quantityInt.ToString();
        GameObject newScrollMaterial = Instantiate(scrollContent);
        newScrollMaterial.transform.SetParent(content.transform, false);

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

    }

    public Texture[] ReturnFirstThreeMatTextures()
    {
        var mat = materialInventory.GetFirstThreeMat();
        Texture[] matText = new Texture[3];
        for(int i =0; i < mat.Length; i++)
        {
            if (mat[i] != null)
            {
                Debug.Log(mat[i].materialTexture);
                matText[i] = mat[i].materialTexture;
            }
            else
            {
                matText[i] = null;
            }
            
        }
        return matText;
    }

    public void AddToMaterialsInventory(CraftMaterial material, int amount)
    {
        Debug.Log("Material getting added to inventory: " + material.materialName);
        materialInventory.AddToInventory(material, amount);
    }

    public void AddToTotalMaterialsInventory(CraftMaterial material, int amount)
    {
        Debug.Log("Material getting added to total inventory: " + material.materialName);
        materialInventory.AddToTotalInventory(material, amount);
    }

    public void ClearInventory()
    {
        materialInventory.ClearInventory();
    }

    public void RemoveFromMaterialsInventory(CraftMaterial material, int amount)
    {
        Debug.Log("Material getting removed from inventory: " + material.materialName);
        materialInventory.RemoveFromInventory(material, amount);
    }

    public void RemoveFromTotalMaterialsInventory(CraftMaterial material, int amount)
    {
        materialInventory.RemoveFromTotalInventory(material, amount);
    }

    private void OnDestroy()
    {
        var scrollObject = scrollContent.GetComponent<MaterialScrollObject>();
        scrollObject.description.text = "empty";
        scrollObject.quantity.text = "x0";
    }

    public CraftMaterial[] GetMaterialInventory()
    {
        return materialInventory.GetInventory();
    }

    public CraftMaterial[] GetTotalMaterialInventory()
    {
        return materialInventory.GetTotalInventory();
    }

    public int GetMaterialInventorySize()
    {
        return materialInventory.GetCurrentInventorySize();
    }

    public int GetMaterialAmount(CraftMaterial material)
    {
       return materialInventory.GetMaterialAmount(material);
    }

    public int GetMaterialInventoryMaxSize()
    {
        return materialInventory.GetMaxInventorySize();
    }

    private void CheckForNull()
    {
        int index = 0;
        for (int i = index ; i < currentMaterials.Count; i++)
        {
            if (currentMaterials[i] == null)
            {
                currentMaterials.RemoveAt(i);
                i = i - 1;
            }
            index = i;
        }
        if (currentMaterials.Count > 0 && currentMaterials[index] == null)
        {
            currentMaterials.RemoveAt(index);
        }
        
        

    }

     private IEnumerator StartCooldown(GameObject scrollObj)
    {
      IEnumerator coroutine = ReduceOpacity(scrollObj);
        //StartCoroutine("ReduceOpacity", scrollObj);
      StartCoroutine(coroutine);
      yield return new WaitForSeconds(5);
      Debug.Log("Finished coroutine");
      if(currentMaterials.Count == 1)
        {
            currentMaterials.Clear();
        }
      Destroy(scrollObj);
      CheckForNull();
      StopCoroutine(coroutine);
            
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
