using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DeathScreen : MonoBehaviour
{
    [SerializeField] TMP_Text firstText;
    [SerializeField] TMP_Text secondText;
    [SerializeField] TMP_Text thirdText;
    [SerializeField] List<RawImage> materialImages;
    [SerializeField] List<GameObject> strokes;
    [SerializeField] List<GameObject> itemCounts;
    [SerializeField] GameObject grid;
    [SerializeField] GameObject materialsRef;
    UIManager uiManager;

    private void Start()
    { 

       /*var color1 = firstText.color;
       color1.a = 0.0f;
       firstText.color = color1;

       var color2 = secondText.color;
       color2.a = 0.0f;
       secondText.color = color2;

       var color3 = thirdText.color;
       color3.a = 0.0f;
       thirdText.color = color3;*/

        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        ResetObjScales();
    }
    public IEnumerator CountdownStroke(int index)
    {
        yield return new WaitForSeconds(1.25f);
        strokes[index].SetActive(true);
    }

    public void InitializeText(int index, int itemCount)
    {
        TMP_Text text = itemCounts[index].GetComponent<TMP_Text>();
        text.text = itemCount.ToString();
        itemCounts[index].SetActive(true);
    }

    public IEnumerator AnimateDeath()
    {
        grid.SetActive(false);
        var curMaterials = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>().GetMaterialInventory();
        var index = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>().GetMaterialInventorySize();
        int validTextures = 0;
        for (int i = 0; i < index; i++)
        {
            validTextures++;
            materialImages[i].texture = curMaterials[i].materialTexture;
        }

        /*material1 = mat1Obj.GetComponent<RawImage>();
        material2 = mat2Obj.GetComponent<RawImage>();
        material3 = mat3Obj.GetComponent<RawImage>();

        Texture[] matText = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>().ReturnFirstThreeMatTextures();
        
        material1.texture = matText[0];
        Color col = material1.color;
        col.a = 0.0f;
        material1.color = col;

        material2.texture = matText[1];
        Color col2 = material2.color;
        col2.a = 0.0f;
        material2.color = col2;

        material3.texture = matText[2];
        Color col3 = material3.color;
        col3.a = 0.0f;
        material3.color = col3;*/

        if (uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        yield return new WaitForSeconds(0.2f);
        var color1 = firstText.color;
        color1.a = 1.0f;
        firstText.color = color1;
        yield return StartCoroutine(uiManager.AnimateTypewriterDeathscreen(firstText, firstText.text, "|", .085f));
        color1 = firstText.color;
        color1.a = 0.0f;
        firstText.color = color1;

        yield return new WaitForSeconds(0.2f);
        var color2 = secondText.color;
        color2.a = 1.0f;
        secondText.color = color2;
        yield return StartCoroutine(uiManager.AnimateTypewriterDeathscreen(secondText, secondText.text, "|", .085f));
        color2 = secondText.color;
        color2.a = 0.0f;
        secondText.color = color2;

        yield return new WaitForSeconds(0.2f);
        var color3 = thirdText.color;
        color3.a = 1.0f;
        thirdText.color = color3;
        yield return StartCoroutine(uiManager.AnimateTypewriterDeathscreen(thirdText, thirdText.text, "|", .085f));
        color3 = thirdText.color;
        color3.a = 0.0f;
        thirdText.color = color3;
        grid.SetActive(true);
        for (int i = 0; i < validTextures; i++)
        {
            StartCoroutine(CountdownStroke(i));
            InitializeText(i, curMaterials[i].currentAmount);
            yield return StartCoroutine(IncreaseMaterialOpacity(materialImages[i], 2.6f));
        }

        /*if (material1.texture != null) yield return StartCoroutine(IncreaseMaterialOpacity(material1, 1.8f));
        if (material2.texture != null) yield return StartCoroutine(IncreaseMaterialOpacity(material2, 1.8f));
        if (material3.texture != null) yield return StartCoroutine(IncreaseMaterialOpacity(material3, 1.8f));*/
        yield break;

    }

    public void ResetObjScales()
    {
        //mat1Obj.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        //mat2Obj.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        //mat3Obj.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        for(int i =0; i < materialImages.Count; i++)
        {
            materialImages[i].transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            Color col = materialImages[i].color;
            col.a = 0.0f;
            materialImages[i].color = col;
        }
        for(int i = 0; i < strokes.Count; i++)
        {
            strokes[i].SetActive(false);
        }
        for(int i =0; i < itemCounts.Count; i++)
        {
            itemCounts[i].SetActive(false);
        }
    }

    public IEnumerator IncreaseOpacity(Text text, float rate)
    {
        
        
        while (text.color.a < 1.0f)
        {
            Debug.Log("Inside DeathScreen increase opacity for :" + text.name);
            if (Mathf.Abs(text.color.a - 1.0f) <= 0.05)
            {
                Color txtColor = text.color;
                txtColor.a = 1.0f;
                text.color = txtColor;
            }
            else
            {
                Color txtColor = text.color;
                txtColor.a += rate * Time.deltaTime;
                text.color = txtColor;
            }

            yield return null;
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(DecreaseOpacity(text, rate));
        yield break;
    }

    public IEnumerator DecreaseOpacity(Text text, float rate)
    {
        //Debug.Log("Inside DeathScreen increase opacity");

        while (text.color.a > 0.0f)
        {
            if (Mathf.Abs(text.color.a - 0.0f) <= 0.05)
            {
                Color txtColor = text.color;
                txtColor.a = 0.0f;
                text.color = txtColor;
            }
            else
            {
                Color txtColor = text.color;
                txtColor.a -= rate * Time.deltaTime;
                text.color = txtColor;
            }

            yield return null;
        }
        if (text.name == "secondText")
        {
            yield return new WaitForSeconds(0.5f);
            //StartCoroutine(IncreaseOpacity(thirdText, rate));
        }
            

        yield break;
    }

    public IEnumerator IncreaseMaterialOpacity(RawImage image, float rate)
    {
        while (image.color.a < 1.0f)
        {
            if (Mathf.Abs(image.color.a - 1.0f) <= 0.05)
            {
                Color txtColor = image.color;
                txtColor.a = 1.0f;
                image.color = txtColor;
            }
            else
            {
                Color txtColor = image.color;
                txtColor.a += rate * Time.deltaTime;
                image.color = txtColor;
            }
            yield return null;
        }
        Debug.Log(image.name);

        /*switch (image.name)
        {
            case "thirdMat" :
                StartCoroutine(DecreaseMaterialScale(mat3Obj, rate*2));
                break;
            case "secondMat" :
                yield return new WaitForSeconds(1f);
                StartCoroutine(DecreaseMaterialScale(mat2Obj, rate*2));
                break;
            case "firstMat":
                yield return new WaitForSeconds(2f);
                StartCoroutine(DecreaseMaterialScale(mat1Obj, rate*2));
                break;
        }*/
        yield break;
    }

    public IEnumerator DecreaseMaterialScale(GameObject matObj, float rate)
    {
        Transform tranRef = matObj.transform;
        Vector3 rateVector = new Vector3(rate, rate, rate);

        Debug.Log("Inside Decrease Material Scale");

        while (tranRef.localScale.x > 0.0f)
        {
            if (Mathf.Abs(tranRef.localScale.x - 0.0f) <= 0.05)
            {
                tranRef.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            }
            else
            {
                tranRef.localScale = new Vector3(
                    tranRef.localScale.x - (rateVector.x * Time.deltaTime), 
                    tranRef.localScale.y - (rateVector.y * Time.deltaTime), 
                    tranRef.localScale.z - (rateVector.z * Time.deltaTime));
            }

            yield return null;
        }

        yield break;
    }
}
