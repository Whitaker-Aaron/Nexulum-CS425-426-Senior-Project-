using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject CheckpointText;
    GameObject currentCheckpointText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator AnimateCheckpointReached()
    {
        Debug.Log("Animating Checkpoing text");
        currentCheckpointText = Instantiate(CheckpointText);
        currentCheckpointText.transform.SetParent(mainCanvas.transform, false);
        currentCheckpointText.transform.localScale = new Vector3(6.0f, 6.0f, 6.0f);
        var rate = new Vector3(20f, 20f, 20f);
        while(currentCheckpointText.transform.localScale.x >= 1.0f)
        {
           Debug.Log(currentCheckpointText.transform.localScale);
           currentCheckpointText.transform.localScale -= (rate * Time.deltaTime);
           yield return null;
        }
        yield return new WaitForSeconds(3.0f);
        yield return StartCoroutine(ReduceTextOpacity(currentCheckpointText, 1.00f));
        Destroy(currentCheckpointText);
        yield break;
    }

    private IEnumerator ReduceTextOpacity(GameObject text, float rate)
    {
        var reference = text.GetComponent<Text>();
        while (reference.color.a >= 0.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a -= rate * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
        yield break;
    }

}
