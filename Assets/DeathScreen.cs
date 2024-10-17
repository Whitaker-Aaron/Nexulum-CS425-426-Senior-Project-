using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class DeathScreen : MonoBehaviour
{
    [SerializeField] Text firstText;
    [SerializeField] Text secondText;
    [SerializeField] Text thirdText;

    RawImage material1;
    RawImage material2;
    RawImage material3;

    GameObject mat1Obj;
    GameObject mat2Obj;
    GameObject mat3Obj;

    private void Start()
    {
        mat1Obj = GameObject.Find("firstMat");
        mat2Obj = GameObject.Find("secondMat");
        mat3Obj = GameObject.Find("thirdMat");

       material1 = mat1Obj.GetComponent<RawImage>();
       material2 =  mat2Obj.GetComponent<RawImage>();
       material3 =  mat3Obj.GetComponent<RawImage>();

       


        var color1 = firstText.color;
       color1.a = 0.0f;
       firstText.color = color1;

       var color2 = secondText.color;
       color2.a = 0.0f;
       secondText.color = color2;

       var color3 = thirdText.color;
       color3.a = 0.0f;
       thirdText.color = color3;
    }

    public IEnumerator AnimateDeath()
    {
        Texture[] matText = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>().ReturnFirstThreeMatTextures();
        Debug.Log(matText[0]);
        material1.texture = matText[2];
        Color col = material1.color;
        col.a = 0.0f;
        material1.color = col;

        material2.texture = matText[1];
        col = material2.color;
        col.a = 0.0f;
        material2.color = col;

        material3.texture = matText[0];
        col = material1.color;
        col.a = 0.0f;
        material3.color = col;



        StartCoroutine(IncreaseOpacity(firstText, .6f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(IncreaseOpacity(secondText, .6f));
        if (material1.texture != null) StartCoroutine(IncreaseMaterialOpacity(material1, 0.6f));
        if (material2.texture != null) StartCoroutine(IncreaseMaterialOpacity(material2, 0.6f));
        if (material3.texture != null) StartCoroutine(IncreaseMaterialOpacity(material3, 0.6f));

        //StartCoroutine(IncreaseOpacity(thirdText, 1.0f));5

    }

    public void ResetObjScales()
    {
        mat1Obj.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        mat2Obj.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        mat3Obj.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
    }

    public IEnumerator IncreaseOpacity(Text text, float rate)
    {
        //Debug.Log("Inside DeathScreen increase opacity");
        
        while (text.color.a < 1.0f)
        {
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
            StartCoroutine(IncreaseOpacity(thirdText, rate));
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

        switch (image.name)
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
        }
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
