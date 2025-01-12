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
    [SerializeField] GameObject scrollContentWrapper;
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

        //scrollObject.descriptionMain.text = "empty";
        //scrollObject.descriptionSub.text = "empty";
        //scrollObject.imageRef.texture = placeholder;
        //scrollObject.quantityInt = 1;
        //scrollObject.quantityMain.text = "x" + scrollObject.quantityInt.ToString();
        //scrollObject.quantitySub.text = "x" + scrollObject.quantityInt.ToString();
        //scrollContentWrapper.GetComponent<MaterialScrollWrapper>().scrollObject = scrollObject;

        //GameObject newScrollMaterial = Instantiate(scrollContentWrapper);
        //newScrollMaterial.transform.SetParent(content.transform, false);
        //currentMaterials.Add(newScrollMaterial);
        //currentMaterials.RemoveAt(0);
        //Destroy(newScrollMaterial);
        //UpdateScroll();
        //var child = content.transform.GetChild(0);
        //Debug.Log(child);
        //child.gameObject.SetActive(false);

        //Debug.Log("Inside the MSM's Start()");

        currentMaterials.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log(currentMaterials.Count);
        if(currentMaterials.Count <= 0)
        {
            //materialGradient.SetActive(false);
        }
        else
        {
            //materialGradient.SetActive(true);
        }
    }

    public void UpdateScroll(Texture materialTexture, string materialName)
    {
        CheckForNull();

        int existingIndex = -1; 
        bool exists = false;
        var scrollContentWrapperMaterial = scrollContentWrapper.GetComponent<MaterialScrollWrapper>();
        scrollContentWrapperMaterial.descriptionMain.text = materialName;
        scrollContentWrapperMaterial.descriptionSub.text = materialName;
        scrollContentWrapperMaterial.imageRef.texture = materialTexture;
        scrollContentWrapperMaterial.quantityInt = 1;
        scrollContentWrapperMaterial.quantityMain.text = "x" + scrollContentWrapperMaterial.quantityInt.ToString();
        scrollContentWrapperMaterial.quantitySub.text = "x" + scrollContentWrapperMaterial.quantityInt.ToString();
        //scrollContentWrapper.GetComponent<MaterialScrollWrapper>().scrollObject = scrollObject;

        GameObject newScrollMaterial = Instantiate(scrollContentWrapper);
        //GameObject newScrollMaterial = Instantiate(scrollContent);
        newScrollMaterial.transform.SetParent(content.transform, false);

        var existingScroll = newScrollMaterial.GetComponent<MaterialScrollWrapper>();

        var desc = existingScroll.descriptionMain;
        for (int i = 0; i < currentMaterials.Count; i++) {
            if (currentMaterials[i].GetComponent<MaterialScrollWrapper>().descriptionMain.text == desc.text)
            {
                exists = true;
                existingIndex = i;
            }
        }
        if (exists)
        {
            Debug.Log("Current material found");

            currentMaterials[existingIndex].GetComponent<MaterialScrollWrapper>().quantityInt += existingScroll.quantityInt;
            currentMaterials[existingIndex].GetComponent<MaterialScrollWrapper>().quantityMain.text = "x" + currentMaterials[existingIndex].GetComponent<MaterialScrollWrapper>().quantityInt.ToString();
            currentMaterials[existingIndex].GetComponent<MaterialScrollWrapper>().quantitySub.text = "x" + currentMaterials[existingIndex].GetComponent<MaterialScrollWrapper>().quantityInt.ToString();
            ResetOpacity(currentMaterials[existingIndex]);
            Destroy(newScrollMaterial);
            //existingScroll.quantitySub.text = "x" + existingScroll.quantityInt.ToString();
            //Destroy(currentMaterials[existingIndex].gameObject);
            //currentMaterials.RemoveAt(existingIndex);
            
        }
        else
        {
            StartCoroutine(StartCooldown(newScrollMaterial, exists));
            currentMaterials.Add(newScrollMaterial);
        }

        



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
        Debug.Log("Clearing inventory from scroll manager");
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
        scrollObject.descriptionMain.text = "empty";
        scrollObject.descriptionSub.text = "empty";
        scrollObject.quantityMain.text = "x0";
        scrollObject.quantitySub.text = "x0";

        var scrollObjectWrapper = scrollContentWrapper.GetComponent<MaterialScrollWrapper>();
        scrollObjectWrapper.descriptionMain.text = "empty";
        scrollObjectWrapper.descriptionSub.text = "empty";
        scrollObjectWrapper.quantityMain.text = "x0";
        scrollObjectWrapper.quantitySub.text = "x0";
        scrollObjectWrapper.transform.GetChild(0).localPosition = new Vector3(50, scrollObjectWrapper.transform.GetChild(0).localPosition.y, scrollObjectWrapper.transform.GetChild(0).localPosition.z);
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

     private IEnumerator StartCooldown(GameObject scrollObj, bool exists)
    {
      IEnumerator coroutine = ReduceOpacity(scrollObj);
      IEnumerator coroutine2 = AnimateMaterialSlide(scrollObj);
        //StartCoroutine("ReduceOpacity", scrollObj);
      if(!exists) StartCoroutine(coroutine2);
      yield return StartCoroutine(coroutine);
      Debug.Log("Finished coroutine");
      if(currentMaterials.Count == 1)
        {
            currentMaterials.Clear();
        }
      Destroy(scrollObj);
      CheckForNull();
      StopCoroutine(coroutine);
      if(!exists) StopCoroutine(coroutine2);

    }

    private IEnumerator AnimateMaterialSlide(GameObject scrollObj)
    {
        var reference = scrollObj.GetComponent<MaterialScrollWrapper>().transform.GetChild(0);
        Vector3 desiredPos = new Vector3(350.0f, reference.localPosition.y, reference.localPosition.z);
        while(reference != null && reference.localPosition != desiredPos)
        {

            reference.localPosition = Vector3.Lerp(reference.localPosition, desiredPos, Time.deltaTime * 14f);
            if (Mathf.Abs(reference.localPosition.magnitude - desiredPos.magnitude) <= 5) reference.localPosition = desiredPos;
            yield return null;
        }
        yield break;
    }

    public void ResetOpacity(GameObject scrollObj)
    {
        var reference = scrollObj.GetComponent<MaterialScrollWrapper>();
        Color imgColor = reference.imageRef.color;
        imgColor.a = 1;
        reference.imageRef.color = imgColor;

        Color backColor = reference.background.color;
        backColor.a = 1;
        reference.background.color = backColor;

        Color desColor = reference.descriptionMain.color;
        desColor.a = 1;
        reference.descriptionMain.color = desColor;

        Color desColor2 = reference.descriptionSub.color;
        desColor2.a = 1;
        reference.descriptionSub.color = desColor2;

        Color quanColor = reference.quantityMain.color;
        quanColor.a = 1;
        reference.quantityMain.color = quanColor;

        Color quanColor2 = reference.quantitySub.color;
        quanColor2.a = 1;
        reference.quantitySub.color = quanColor2;
    }

    private IEnumerator ReduceOpacity(GameObject scrollObj)
    {
        var reference = scrollObj.GetComponent<MaterialScrollWrapper>();
        while (reference.imageRef.color.a > 0.0f && reference != null)
        {
            Color imgColor = reference.imageRef.color;
            imgColor.a -= 0.10f * Time.deltaTime;
            reference.imageRef.color = imgColor;

            Color backColor = reference.background.color;
            backColor.a -= 0.10f * Time.deltaTime;
            reference.background.color = backColor;

            Color desColor = reference.descriptionMain.color;
            desColor.a -= 0.10f * Time.deltaTime;
            reference.descriptionMain.color = desColor;

            Color desColor2 = reference.descriptionSub.color;
            desColor2.a -= 0.10f * Time.deltaTime;
            reference.descriptionSub.color = desColor2;

            Color quanColor = reference.quantityMain.color;
            quanColor.a -= 0.10f * Time.deltaTime;
            reference.quantityMain.color = quanColor;

            Color quanColor2 = reference.quantitySub.color;
            quanColor2.a -= 0.10f * Time.deltaTime;
            reference.quantitySub.color = quanColor2;

            yield return null;
        }
        yield break;
    }
}
