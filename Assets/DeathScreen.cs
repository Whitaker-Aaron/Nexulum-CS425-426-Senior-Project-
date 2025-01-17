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
    string ogFirstText = "";
    string ogSecondText = "";
    string ogThirdText = "";
    [SerializeField] List<RawImage> materialImages;
    [SerializeField] List<GameObject> strokes;
    [SerializeField] List<GameObject> itemCounts;
    [SerializeField] public List<DialogueObject> onDeathDialogues;
    [SerializeField] GameObject grid;
    [SerializeField] GameObject materialsRef;
    [SerializeField] GameObject itemsGrid;
    UIManager uiManager;

    private void Start()
    { 

        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        ogFirstText = firstText.text;
        ogSecondText = secondText.text;
        ogThirdText =  thirdText.text;
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
            if (curMaterials[i] == null) break;
            validTextures++;
            materialImages[i].texture = curMaterials[i].materialTexture;
        }


        if (uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        yield return new WaitForSeconds(0.2f);
        var color1 = firstText.color;
        color1.a = 1.0f;
        firstText.color = color1;
        yield return StartCoroutine(uiManager.AnimateTypewriterDeathscreen(firstText, firstText.text, "|", .065f));
        color1 = firstText.color;
        color1.a = 0.0f;
        firstText.color = color1;

        yield return new WaitForSeconds(0.2f);
        var color2 = secondText.color;
        color2.a = 1.0f;
        secondText.color = color2;
        yield return StartCoroutine(uiManager.AnimateTypewriterDeathscreen(secondText, secondText.text, "|", .065f));
        color2 = secondText.color;
        color2.a = 0.0f;
        secondText.color = color2;


        yield return new WaitForSeconds(0.2f);
        grid.SetActive(true);
        StartCoroutine(animateGrid());
        var color3 = thirdText.color;
        color3.a = 1.0f;
        thirdText.color = color3;
        StartCoroutine(uiManager.AnimateTypewriterDeathscreen(thirdText, thirdText.text, "|", .065f));
        
 
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < validTextures; i++)
        {
            StartCoroutine(CountdownStroke(i));
            InitializeText(i, curMaterials[i].currentAmount);
            yield return StartCoroutine(IncreaseMaterialOpacity(materialImages[i], 2.6f));
        }
        yield break;

    }

    public IEnumerator animateGrid()
    {
        var desiredPos = new Vector3(593, itemsGrid.transform.localPosition.y, itemsGrid.transform.localPosition.z);
        while(itemsGrid.transform.localPosition.x != 593)
        {
            itemsGrid.transform.localPosition = Vector3.Lerp(itemsGrid.transform.localPosition, desiredPos, Time.deltaTime * 7f);
            if(Mathf.Abs(itemsGrid.transform.localPosition.magnitude -  desiredPos.magnitude) < 0.1) itemsGrid.transform.localPosition = desiredPos;
            yield return null;
        }
        yield break;
    }

    public void ResetObjScales()
    {
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
        itemsGrid.transform.localPosition = new Vector3(1500, itemsGrid.transform.localPosition.y, itemsGrid.transform.localPosition.z);
        firstText.text = ogFirstText;
        secondText.text = ogSecondText;
        thirdText.text = ogThirdText;

        var color3 = thirdText.color;
        color3.a = 0.0f;
        thirdText.color = color3;
        //firstText
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
