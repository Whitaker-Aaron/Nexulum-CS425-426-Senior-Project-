using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using System.Xml.Schema;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject CheckpointText;
    [SerializeField] GameObject bossText;
    [SerializeField] GameObject levelUpText;
    [SerializeField] GameObject attackUpText;
    [SerializeField] GameObject expText;
    float ogExpTextXPos;
    [SerializeField] GameObject mainHUD;
    [SerializeField] GameObject knightHUD;
    [SerializeField] GameObject engineerHUD;
    [SerializeField] GameObject gunnerHUD;
    [SerializeField] GameObject topHUD;
    [SerializeField] GameObject bottomHUD;
    [SerializeField] GameObject topRedBorder;
    [SerializeField] GameObject bottomRedBorder;
    [SerializeField] GameObject thankYouScreen;
    [SerializeField] GameObject warningObject;

    [SerializeField] GameObject ability1;
    [SerializeField] GameObject ability2;
    [SerializeField] GameObject ability3;
    [SerializeField] GameObject ability_spell1;
    [SerializeField] GameObject ability_spell2;
    [SerializeField] GameObject ability_spell3;
    [SerializeField] GameObject Spell_Text;
    [SerializeField] GameObject Abilities_Text;
    [SerializeField] GameObject dashSmear;
    [SerializeField] GameObject criticalText;
    [SerializeField] GameObject criticalTextBorder;
    [SerializeField] GameObject warningScreen;
    [SerializeField] GameObject florentineUI;
    

    [SerializeField] GameObject damageNumPrefab;
    [SerializeField] GameObject viewItemPrefab;
    [SerializeField] GameObject chestDepositUI;
    [SerializeField] GameObject enemiesRemainingUI;

    [SerializeField] GameObject talk_portrait;
    [SerializeField] GameObject dialogue_box;
    [SerializeField] GameObject advance_textbox_obj;

    [SerializeField] GameObject abilitiesUI;
    [SerializeField] GameObject spellsUI;
    [SerializeField] GameObject greyedOutSwapUI;
    [SerializeField] GameObject spellAbility1;
    [SerializeField] GameObject spellAbility2;
    [SerializeField] GameObject spellAbility3;
    [SerializeField] GameObject classAbility1;
    [SerializeField] GameObject classAbility2;
    [SerializeField] GameObject classAbility3;

    [SerializeField] GameObject tempGradients;
    [SerializeField] GameObject viewItemGradient;

    [SerializeField] GameObject tutorialObject;

    
    Coroutine currentCriticalOpacity;
    Coroutine currentCriticalBorderOpacity;
    Coroutine currentTransitionTypewriter;
    IEnumerator currentDialogueBox;
    Coroutine currentDialogueBoxAnimation;
    Coroutine currentCheckpointAnimator;
    Coroutine currentExpText;
    Coroutine currentBorderStretch;
    Coroutine currentFlorentineAnimator;

    Slider currentAbilitySlider;

    [SerializeField] Slider ExperienceBar;
    GameObject currentCheckpointText;
    GameObject currentBossText;
    GameObject curViewItem;
    Queue<GameObject> currentSmears = new Queue<GameObject>();
    Queue<GameObject> currentDamageNums = new Queue<GameObject>();
    Queue<string> dialogueText = new Queue<string>();
    CharacterBase character;
    public static ResetDelegateTemplate.ResetDelegate curViewReset;
    AudioManager audioManager;
    bool advanceTextbox = false;
    bool advanceLeadChar = false;
    bool viewItemActive = false;
    bool abilityUIActive = true;

    Vector3 initialEnemyRemainingUIPos;
    Vector3 initialTutorialPagePos;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        knightHUD.SetActive(false);
        gunnerHUD.SetActive(false);
        engineerHUD.SetActive(false);
        initialTutorialPagePos = tutorialObject.transform.Find("Tutorial").gameObject.transform.localPosition;
        ogExpTextXPos = expText.transform.localPosition.x;

        initialEnemyRemainingUIPos = enemiesRemainingUI.transform.position;
    }

    private void Update()
    {
        if (currentDamageNums.Count > 0)
        {
            foreach (var num in currentDamageNums)
            {
                //num.transform.position = new Vector3(num.transform.position.x, num.transform.position.y, num.transform.position.z);
            }
        }
        if(viewItemActive && curViewItem != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(curViewItem.GetComponent<ViewItemPrefab>().backButton);
        }
    }

    public void OnDeath()
    {
        DeactivateEnemiesRemainingUI();
    }

    public void LoadTutorial(TutorialObject tutorialToLoad, GameObject trigger)
    {
        var tutorial = tutorialObject.GetComponent<TutorialPage>();
        tutorial.tutorial = tutorialToLoad;
        tutorial.trigger = trigger;
        tutorial.destroyOnExit = false;
        tutorialObject.SetActive(true);
        var page = tutorialObject.transform.Find("Tutorial").gameObject;
        page.transform.localPosition = initialTutorialPagePos;
        startTutorialAnimate(page);
    }

    public void startTutorialAnimate(GameObject page)
    {
        if (character.isDying) return;
        StartCoroutine(animateTutorialPage(page));
    }

    public void StartWarningIcon()
    {
        StartCoroutine(animateWarningIcon());
    }

    public IEnumerator animateWarningIcon()
    {
        
        warningObject.SetActive(true);
        int counter = 0;
        while (counter < 3)
        {
            
            StartCoroutine(IncreaseImageOpacity(warningObject.transform.Find("GuildLogo").gameObject, 2f, true));
            StartCoroutine(IncreaseTextOpacity(warningObject.transform.Find("Text1").gameObject, 2f, true));
            yield return StartCoroutine(IncreaseTextOpacity(warningObject.transform.Find("Text2").gameObject, 2f, true));
            audioManager.PlaySFX("Alarm");
            StartCoroutine(DecreaseImageOpacity(warningObject.transform.Find("GuildLogo").gameObject, 2f));
            StartCoroutine(ReduceTextOpacity(warningObject.transform.Find("Text1").gameObject, 2f));
            yield return StartCoroutine(ReduceTextOpacity(warningObject.transform.Find("Text2").gameObject, 2f));
            counter++;
            yield return null;
        }
        warningObject.SetActive(false);
        yield break;
    }

    public IEnumerator animateThankYouScreen(GameObject thankYouPage)
    {
  
        Vector3 desiredPos = new Vector3(-400, thankYouPage.transform.localPosition.y, thankYouPage.transform.localPosition.z);
        bool animFinished = false;
        while (!animFinished)
        {
            if (thankYouPage == null) yield break;
            if (thankYouPage != null) thankYouPage.transform.localPosition = Vector3.Lerp(thankYouPage.transform.localPosition, desiredPos, (10f * Time.unscaledDeltaTime));
            /*if(page.transform.localPosition.x == -400.0f)
            {
                page.transform.localPosition = desiredPos;
                animFinished = true;
            }*/
            //Debug.Log(page.transform.position);
            //Debug.Log(page.transform.localPosition);
            yield return null;
        }
        yield break;

    }

    public IEnumerator animateTutorialPage(GameObject page)
    {
        Vector3 desiredPos = new Vector3(-400, page.transform.localPosition.y, page.transform.localPosition.z);
        Debug.Log(page);
        Debug.Log(page.transform.position);
        Debug.Log(page.transform.localPosition);
        //page.transform.localPosition != desiredPos
        bool animFinished = false;
        while (!animFinished)
        {
            if (page == null) yield break;
            if(page != null) page.transform.localPosition = Vector3.Lerp(page.transform.localPosition, desiredPos, (10f * Time.unscaledDeltaTime));
            /*if(page.transform.localPosition.x == -400.0f)
            {
                page.transform.localPosition = desiredPos;
                animFinished = true;
            }*/
            //Debug.Log(page.transform.position);
            //Debug.Log(page.transform.localPosition);
            yield return null;
        }
        yield break;
    }

    public IEnumerator animateEnemyRemainingUI()
    {
        enemiesRemainingUI.transform.position = initialEnemyRemainingUIPos;
        Vector3 desiredPos = new Vector3(enemiesRemainingUI.transform.position.x, 932, enemiesRemainingUI.transform.position.z);
        while(enemiesRemainingUI.transform.position != desiredPos)
        {
            //Vector3 newPos = new Vector3(enemiesRemainingUI.transform.position.x, enemiesRemainingUI.transform.position.y - (000f*Time.deltaTime),
            //    enemiesRemainingUI.transform.position.z);
            enemiesRemainingUI.transform.position = Vector3.Lerp(enemiesRemainingUI.transform.position, desiredPos, 10f * Time.deltaTime);
            if (Mathf.Abs(enemiesRemainingUI.transform.position.y - 932) <= 2) enemiesRemainingUI.transform.position = desiredPos;
            yield return null;
        }
        yield break;
    }

    public void EnableHUD()
    {
        tempGradients.SetActive(false);
        topHUD.SetActive(true);
        bottomHUD.SetActive(true);
        dialogue_box.SetActive(true);
    }

    public void EnableDialogueBox()
    {
        dialogue_box.SetActive(true);
    }

    public void DisableHUD(bool keepGradients=false)
    {
        if(keepGradients) tempGradients.SetActive(true);
        GameObject.Find("TopHUD").SetActive(false);
        GameObject.Find("BottomHUD").SetActive(false);
        GameObject.Find("DialogueBox").SetActive(false);
    }

    public void DeactivateEnemiesRemainingUI()
    {
        enemiesRemainingUI.SetActive(false);
    }

    public void ActivateEnemiesRemainingUI(int numEnemies)
    {
        enemiesRemainingUI.SetActive(true);
        if (numEnemies > 0)
        {
            Debug.Log("NUM ENEMIES: " + numEnemies.ToString());
            //enemiesRemainingUI.SetActive(true);
            //GameObject.Find("EnemiesRemainingUI").transform.Find("EnemyRemainingText").GetComponent<TMP_Text>().text = "x" + numEnemies;
            //GameObject.Find("EnemiesRemainingUI").transform.Find("EnemyRemainingShadowText").GetComponent<TMP_Text>().text = "x" + numEnemies;
            enemiesRemainingUI.transform.Find("EnemyRemainingText").GetComponent<TMP_Text>().text = "x" + numEnemies.ToString();
            enemiesRemainingUI.transform.Find("EnemyRemainingShadowText").GetComponent<TMP_Text>().text = "x" + numEnemies.ToString();
            StartCoroutine(animateEnemyRemainingUI());
        }
    }

    public bool UpdateEnemiesRemainingUI(int numEnemies)
    {
        if (!enemiesRemainingUI.activeSelf) return false;
        if (numEnemies > 0)
        {
            Debug.Log("NUM ENEMIES: " + numEnemies.ToString());
            //enemiesRemainingUI.SetActive(true);
            //GameObject.Find("EnemiesRemainingUI").transform.Find("EnemyRemainingText").GetComponent<TMP_Text>().text = "x" + numEnemies;
            //GameObject.Find("EnemiesRemainingUI").transform.Find("EnemyRemainingShadowText").GetComponent<TMP_Text>().text = "x" + numEnemies;
            enemiesRemainingUI.transform.Find("EnemyRemainingText").GetComponent<TMP_Text>().text = "x" + numEnemies.ToString();
            enemiesRemainingUI.transform.Find("EnemyRemainingShadowText").GetComponent<TMP_Text>().text = "x" + numEnemies.ToString();
        }
        else DeactivateEnemiesRemainingUI();

        return true;

    }

    public void ShowCriticalText()
    {
        criticalText.SetActive(true);
        criticalTextBorder.SetActive(true);
        StartCoroutine(AnimateCriticalText());
        StartCoroutine(AnimateCriticalTextBorder());
    }

    public void HideCriticalText()
    {
        StopCoroutine(AnimateCriticalText());
        StopCoroutine(AnimateCriticalTextBorder());
        StopCoroutine(currentCriticalOpacity);
        StopCoroutine(currentCriticalBorderOpacity);
        criticalText.SetActive(false);
        criticalTextBorder.SetActive(false);

    }

    public void DisplayViewItem(ViewItemPrefab.ViewType type, ResetDelegateTemplate.ResetDelegate method, WeaponBase? weapon = null, Rune? rune = null, PlayerItem? item = null)
    {
        viewItemActive = true;
        viewItemGradient.SetActive(true);
        viewItemGradient.transform.SetAsLastSibling();
        var viewItem = viewItemPrefab.GetComponent<ViewItemPrefab>();
        curViewReset = method;
        switch (type)
        {
            case ViewItemPrefab.ViewType.Weapon:
                if(weapon != null)
                {
                    viewItem.classTypeUI.SetActive(true);
                    viewItem.runeTypeUI.SetActive(false);
                    switch (weapon.weaponClassType)
                    {
                        case WeaponBase.weaponClassTypes.Knight:
                            viewItem.classTypeUI.transform.Find("KnightClass").gameObject.SetActive(true);
                            viewItem.classTypeUI.transform.Find("GunnerClass").gameObject.SetActive(false);
                            viewItem.classTypeUI.transform.Find("EngineerClass").gameObject.SetActive(false);
                            break;
                        case WeaponBase.weaponClassTypes.Gunner:
                            viewItem.classTypeUI.transform.Find("KnightClass").gameObject.SetActive(false);
                            viewItem.classTypeUI.transform.Find("GunnerClass").gameObject.SetActive(true);
                            viewItem.classTypeUI.transform.Find("EngineerClass").gameObject.SetActive(false);
                            break;
                        case WeaponBase.weaponClassTypes.Engineer:
                            viewItem.classTypeUI.transform.Find("KnightClass").gameObject.SetActive(false);
                            viewItem.classTypeUI.transform.Find("GunnerClass").gameObject.SetActive(false);
                            viewItem.classTypeUI.transform.Find("EngineerClass").gameObject.SetActive(true);
                            break;
                    }
                    viewItem.viewItemName.GetComponent<TMP_Text>().text = weapon.weaponName;
                    viewItem.viewOptionDescription.GetComponent<TMP_Text>().text = weapon.weaponDescription;
                    if (weapon.weaponTexture != null) viewItem.viewTexture.GetComponent<RawImage>().texture = weapon.weaponTexture;
                    viewItem.viewOptionEffect.SetActive(false);
                    viewItem.viewOptionDamage.SetActive(true);
                    viewItem.viewOptionDamage.transform.Find("DamageDescription").gameObject.GetComponent<TMP_Text>().text = "+ " + weapon.weaponAttack + " ATK";
                }
                break;
            case ViewItemPrefab.ViewType.Item:
                if (item != null)
                {
                    viewItem.classTypeUI.SetActive(false);
                    viewItem.runeTypeUI.SetActive(false);
                    viewItem.viewItemName.GetComponent<TMP_Text>().text = item.itemName;
                    viewItem.viewOptionDescription.GetComponent<TMP_Text>().text = item.itemDescription;
                    viewItem.viewOptionEffect.SetActive(false);
                    viewItem.viewOptionDamage.SetActive(false);
                    if (item.itemTexture != null) viewItem.viewTexture.GetComponent<RawImage>().texture = item.itemTexture;
                }
                break;
            case ViewItemPrefab.ViewType.Rune:
                if (rune != null)
                {
                    viewItem.classTypeUI.SetActive(false);
                    viewItem.runeTypeUI.SetActive(true);
                    switch (rune.runeType)
                    {
                        case Rune.RuneType.Class:
                            viewItem.runeTypeUI.transform.Find("ClassUI").gameObject.SetActive(true);
                            viewItem.runeTypeUI.transform.Find("BuffUI").gameObject.SetActive(false);
                            viewItem.runeTypeUI.transform.Find("SpellUI").gameObject.SetActive(false);
                            break;
                        case Rune.RuneType.Spell:
                            viewItem.runeTypeUI.transform.Find("ClassUI").gameObject.SetActive(false);
                            viewItem.runeTypeUI.transform.Find("BuffUI").gameObject.SetActive(false);
                            viewItem.runeTypeUI.transform.Find("SpellUI").gameObject.SetActive(true);
                            break;
                        case Rune.RuneType.Buff:
                            viewItem.runeTypeUI.transform.Find("ClassUI").gameObject.SetActive(false);
                            viewItem.runeTypeUI.transform.Find("BuffUI").gameObject.SetActive(true);
                            viewItem.runeTypeUI.transform.Find("SpellUI").gameObject.SetActive(false);
                            break;
                    }
                    viewItem.viewItemName.GetComponent<TMP_Text>().text = rune.runeName;
                    viewItem.viewOptionDescription.GetComponent<TMP_Text>().text = rune.runeDescription;
                    if (rune.runeTexture != null) viewItem.viewTexture.GetComponent<RawImage>().texture = rune.runeTexture;
                    viewItem.viewOptionEffect.SetActive(true);
                    viewItem.viewOptionDamage.SetActive(false);
                    viewItem.viewOptionEffect.transform.Find("EffectDescription").gameObject.GetComponent<TMP_Text>().text = "+ " + rune.runeEffect;
                }
                break;
        }
        curViewItem = Instantiate(viewItemPrefab);
        var ui = GameObject.Find("Canvas");
        curViewItem.transform.SetParent(ui.transform, false);

    }

    public void HideViewItem()
    {
        viewItemGradient.SetActive(false);
        if (viewItemActive)
        {
            if (curViewReset != null) curViewReset();
            viewItemActive = false;
            Destroy(curViewItem);
            curViewReset = null;
        }

    }

    public void UpdateFlorentine(int amount, string dir="None")
    {
        //var text = florentineUI.GetComponent<TMP_Text>();
        //text.text = amount.ToString();
        if (currentFlorentineAnimator != null) StopCoroutine(currentFlorentineAnimator);
        currentFlorentineAnimator = StartCoroutine(AnimateFlorentine(amount, dir));
    }

    public IEnumerator AnimateFlorentine(int amount, string dir)
    {
        bool finished = false;
        var text = florentineUI.GetComponent<TMP_Text>();
        while (!finished)
        {
            var textInt = int.Parse(text.text);
            if(dir == "Up")
            {
                if (textInt >= amount)
                {
                    text.text = amount.ToString();
                    finished = true;
                    break;
                }
                var rate = (int)(200 * Time.deltaTime);
                text.text = (textInt + rate).ToString();
            }
            else if(dir == "Down")
            {
                if (textInt <= amount)
                {
                    text.text = amount.ToString();
                    finished = true;
                    break;
                }
                var rate = (int)(200 * Time.deltaTime);
                text.text = (textInt - rate).ToString();
            }
            else if(dir == "None"){
                text.text = amount.ToString();
                finished = true;
                break;
            }
            
            yield return null;
            
        }
        yield break;
    }

    public void DisplayDamageNum(Transform enemyTransform, float damage, float textSize = 40f, float rate = 2f)
    {
        if (character.isDying) return;
        var mainText = damageNumPrefab.GetComponent<TMP_Text>();
        mainText.text = damage.ToString();
        mainText.fontSize = textSize;
        var backdropText = damageNumPrefab.transform.Find("EnemyDamageNumberBackdrop").GetComponent<TMP_Text>();
        backdropText.text = damage.ToString();
        backdropText.fontSize = textSize;
        GameObject numRef = Instantiate(damageNumPrefab);
        var ui = GameObject.Find("Canvas");
        numRef.transform.SetParent(ui.transform, false);
        var screenPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(enemyTransform.position);
        numRef.transform.position = new Vector3(screenPos.x + Random.Range(-25.0f, 25.0f), screenPos.y + Random.Range(0.0f, 120.0f), screenPos.z);
        StartCoroutine(AnimateDamageNum(numRef, enemyTransform, rate));
        //currentDamageNums.Enqueue(numRef);
    }

    public void DisplayChestDepositUI()
    {

    }

    public void StartAnimateTypewriterScreenTransition(TMP_Text tmp_text, string text_to_animate, string leadingChar, float rate)
    {
        if (currentTransitionTypewriter != null) StopCoroutine(currentTransitionTypewriter);
        currentTransitionTypewriter = StartCoroutine(AnimateTypewriterScreenTransition(tmp_text, text_to_animate, leadingChar, rate));
    }

    public IEnumerator AnimateTypewriterScreenTransition(TMP_Text tmp_text, string text_to_animate, string leadingChar = "", float rate = 0.25f)
    {

        tmp_text.text = "";
        foreach (char c in text_to_animate)
        {
            if (tmp_text.text.Length > 0)
            {
                tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            }
            tmp_text.text += c;
            tmp_text.text += leadingChar;
            audioManager.PlaySFX("KeyTap");
            yield return new WaitForSeconds(rate);
        }
        int counter = 0;
        while (leadingChar != "" && counter < 2)
        {
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            yield return new WaitForSeconds(rate * 2);
            counter++;
            tmp_text.text += leadingChar;
            yield return new WaitForSeconds(rate * 2);
            //yield return null;
        }
        foreach (char c in text_to_animate)
        {
            if (tmp_text.text.Length > 0)
            {
                tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            }
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - 1);
            tmp_text.text += leadingChar;
            audioManager.PlaySFX("KeyTap");
            yield return new WaitForSeconds(rate);
        }
        counter = 0;
        /*while (leadingChar != "" && counter < 2)
        {
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            yield return new WaitForSeconds(rate * 2);
            counter++;
            tmp_text.text += leadingChar;
            yield return new WaitForSeconds(rate * 2);
            //yield return null;
        }*/
        if (leadingChar != "") tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
    }

    public IEnumerator LoadDialogueBox(DialogueObject dialogueObject)
    {
        if (currentDialogueBox != null) UnloadDialogue();
        if(dialogueText.Count <= 0) audioManager.PlaySFX("DialogueStart");
        foreach (var text in dialogueObject.dialogueList)
        {
            dialogueText.Enqueue(text);
        }
        if(currentDialogueBoxAnimation != null) StopCoroutine(currentDialogueBoxAnimation);
        currentDialogueBoxAnimation = StartCoroutine(AnimateDialogueBoxMovement("left"));
        currentDialogueBox = AnimateTypewriterDialogue(GameObject.Find("DialogueText").GetComponent<TMP_Text>(), dialogueObject.leadingChar, dialogueObject.textRate, dialogueObject.stopPlayer);
        StartCoroutine(currentDialogueBox);
        //Debug.Log("Returned from dialogue coroutine");
        //dialogueObject.dialogueFinished = true;
        yield break;
    }

    public IEnumerator AwaitLoadDialogueBox(DialogueObject dialogueObject)
    {
        if (currentDialogueBox != null) UnloadDialogue();
        foreach (var text in dialogueObject.dialogueList)
        {
            dialogueText.Enqueue(text);
        }
        advanceTextbox = false;
        currentDialogueBoxAnimation = StartCoroutine(AnimateDialogueBoxMovement("left"));
        currentDialogueBox = AnimateTypewriterDialogue(GameObject.Find("DialogueText").GetComponent<TMP_Text>(), dialogueObject.leadingChar, dialogueObject.textRate, dialogueObject.stopPlayer);
        yield return StartCoroutine(currentDialogueBox);
        //Debug.Log("Returned from dialogue coroutine");
        //dialogueObject.dialogueFinished = true;
        yield break;
    }

    public void UnloadDialogue()
    {
        if(currentDialogueBox != null) StopCoroutine(currentDialogueBox);
        if(currentDialogueBoxAnimation != null) StopCoroutine(currentDialogueBoxAnimation);
        GameObject static_portrait = GameObject.Find("Portrait_Static");
        talk_portrait.SetActive(false);
        static_portrait.SetActive(true);
        dialogueText.Clear();
        GameObject.Find("DialogueText").GetComponent<TMP_Text>().text = "";
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (character.inDialogueBox && context.performed)
        {
            advanceTextbox = true;
        }
    }

    public IEnumerator DialogueBoxTimeout()
    {

        //yield return new WaitForSeconds(0.2f);
        if(dialogueText.Count > 0)
        {
            yield break;
        }
        else
        {
            if (currentDialogueBoxAnimation != null) StopCoroutine(currentDialogueBoxAnimation);
           currentDialogueBoxAnimation = StartCoroutine(AnimateDialogueBoxMovement("right"));
        }
        yield break;
    }

    public IEnumerator AnimateDialogueBoxMovement(string direction)
    {
        Vector3 desiredPos = dialogue_box.transform.localPosition;
        if (direction == "left")
        {
            desiredPos = new Vector3(413, dialogue_box.transform.localPosition.y, dialogue_box.transform.localPosition.z);
            
        }
        else if(direction == "right")
        {
            desiredPos = new Vector3(1510, dialogue_box.transform.localPosition.y, dialogue_box.transform.localPosition.z);
        }
        while (dialogue_box.transform.localPosition != desiredPos)
        {
            if (Mathf.Abs(dialogue_box.transform.localPosition.magnitude - desiredPos.magnitude) <= 0.5f) dialogue_box.transform.localPosition = desiredPos;
            dialogue_box.transform.localPosition = Vector3.Lerp(dialogue_box.transform.localPosition, desiredPos, 5.5f * Time.deltaTime);
            yield return null;
        }
        yield break;
    }

    public IEnumerator AwaitDialogueBoxLeadChar()
    {
        yield return new WaitForSeconds(0.5f);
        advanceLeadChar = true;
        yield return null;
    }

    public void startBorderStretch()
    {
        if (currentBorderStretch != null) StopCoroutine(currentBorderStretch);
        currentBorderStretch = StartCoroutine(StretchBorderUI());
    }

    public void stopBorderStretch()
    {
        if (currentBorderStretch != null) StopCoroutine(currentBorderStretch);
        currentBorderStretch = StartCoroutine(UnstretchBorderUI());
    }

    public IEnumerator StretchBorderUI()
    {
     
        var topGrad = GameObject.Find("TopGradient").GetComponent<RectTransform>();
        var topGrad2 = GameObject.Find("TopGradient2").GetComponent<RectTransform>();
        var botGrad = GameObject.Find("BottomGradient").GetComponent<RectTransform>();
        var botGrad2 = GameObject.Find("BottomGradient2").GetComponent<RectTransform>();
        float desiredAmount = topGrad.sizeDelta.y + 300;
        
        while(topGrad.sizeDelta.y < desiredAmount)
        {
            topGrad.sizeDelta = new Vector2(topGrad.sizeDelta.x, topGrad.sizeDelta.y + 1250f*Time.deltaTime);
            topGrad2.sizeDelta = new Vector2(topGrad2.sizeDelta.x, topGrad2.sizeDelta.y + 1250f * Time.deltaTime);
            botGrad.sizeDelta = new Vector2(botGrad.sizeDelta.x, botGrad.sizeDelta.y + 1250f * Time.deltaTime);
            botGrad2.sizeDelta = new Vector2(botGrad2.sizeDelta.x, botGrad2.sizeDelta.y + 1250f * Time.deltaTime);
            if(Mathf.Abs(topGrad.sizeDelta.y - desiredAmount) <= 25)
            {
                topGrad.sizeDelta = new Vector2(topGrad.sizeDelta.x, desiredAmount);
                topGrad2.sizeDelta = new Vector2(topGrad2.sizeDelta.x, desiredAmount);
                botGrad.sizeDelta = new Vector2(botGrad.sizeDelta.x, desiredAmount);
                botGrad2.sizeDelta = new Vector2(botGrad2.sizeDelta.x, desiredAmount);
            }
            yield return null;
        }
        yield break;
    }

    public IEnumerator UnstretchBorderUI()
    {
        var topGrad = GameObject.Find("TopGradient").GetComponent<RectTransform>();
        var topGrad2 = GameObject.Find("TopGradient2").GetComponent<RectTransform>();
        var botGrad = GameObject.Find("BottomGradient").GetComponent<RectTransform>();
        var botGrad2 = GameObject.Find("BottomGradient2").GetComponent<RectTransform>();
        float desiredAmount = 275.9484f;

        while (topGrad.sizeDelta.y > desiredAmount)
        {
            topGrad.sizeDelta = new Vector2(topGrad.sizeDelta.x, topGrad.sizeDelta.y - 1250f * Time.deltaTime);
            topGrad2.sizeDelta = new Vector2(topGrad2.sizeDelta.x, topGrad2.sizeDelta.y - 1250f * Time.deltaTime);
            botGrad.sizeDelta = new Vector2(botGrad.sizeDelta.x, botGrad.sizeDelta.y - 1250f * Time.deltaTime);
            botGrad2.sizeDelta = new Vector2(botGrad2.sizeDelta.x, botGrad2.sizeDelta.y - 1250f * Time.deltaTime);
            if (Mathf.Abs(topGrad.sizeDelta.y - desiredAmount) <= 35)
            {
                topGrad.sizeDelta = new Vector2(topGrad.sizeDelta.x, desiredAmount);
                topGrad2.sizeDelta = new Vector2(topGrad2.sizeDelta.x, desiredAmount);
                botGrad.sizeDelta = new Vector2(botGrad.sizeDelta.x, desiredAmount);
                botGrad2.sizeDelta = new Vector2(botGrad2.sizeDelta.x, desiredAmount);
            }
            yield return null;
        }
        yield break;
    }

    public IEnumerator AnimateTypewriterDialogue(TMP_Text tmp_text, string leadingChar = "", float rate = 0.25f, bool freezePlayer = false)
    {
        if (freezePlayer)
        {
            character.inDialogueBox = true;
            character.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        }
        tmp_text.text = "";
        GameObject static_portrait = GameObject.Find("Portrait_Static");
        talk_portrait.SetActive(false);
        //advance_textbox_obj.SetActive(false);
        while (dialogueText.Count > 0)
        {
            var text_to_aniamte = dialogueText.Dequeue();
            advance_textbox_obj.SetActive(false);
            foreach (char c in text_to_aniamte)
            {
                if (freezePlayer && advanceTextbox)
                {
                    tmp_text.text = text_to_aniamte;
                    //tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
                    tmp_text.text += leadingChar;
                    advanceTextbox = false;
                    break;
                }
                if (c == ' ') talk_portrait.SetActive(false);
                else talk_portrait.SetActive(true);
                if (tmp_text.text.Length > 0)
                {
                    tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
                }
                tmp_text.text += c;
                tmp_text.text += leadingChar;
                audioManager.PlaySFX("DialogueKey");
                yield return new WaitForSeconds(rate);
            }
            int counter = 0;
            int counterLimit = 3;
            talk_portrait.SetActive(false);
            if(freezePlayer) advance_textbox_obj.SetActive(true);
            while (counter < counterLimit && !advanceTextbox)
            {
                advanceLeadChar = false;
                if (advanceTextbox) break;
                tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
                StartCoroutine(AwaitDialogueBoxLeadChar());
                while (!advanceLeadChar)
                {
                    if (advanceTextbox) break;
                    yield return null;
                }
                advanceLeadChar = false;
                if(!freezePlayer) counter++;
                tmp_text.text += leadingChar;
                if (advanceTextbox) break;
                StartCoroutine(AwaitDialogueBoxLeadChar());
                while (!advanceLeadChar)
                {
                    if (advanceTextbox) break;
                    yield return null;
                }
                yield return null;
            }
            advanceTextbox = false;
            //if (!freezePlayer) yield return new WaitForSeconds(0.75f);

            if(dialogueText.Count > 0) tmp_text.text = "";
        }
        if (freezePlayer)
        {
            character.inDialogueBox = false;
            character.GetMasterInput().GetComponent<masterInput>().resumePlayerInput();
        }
        StartCoroutine(DialogueBoxTimeout());
        Debug.Log("Finished dialogue box");
        yield break;
        //if (leadingChar != "") tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
    }

    public IEnumerator AnimateTypewriterDeathscreen(TMP_Text tmp_text, string text_to_animate, string leadingChar = "", float rate = 0.25f)
    {
        tmp_text.text = "";
        foreach (char c in text_to_animate)
        {
            if (tmp_text.text.Length > 0)
            {
                tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            }
            tmp_text.text += c;
            tmp_text.text += leadingChar;
            audioManager.PlaySFX("KeyTap");
            yield return new WaitForSeconds(rate);
        }
        int counter = 0;
        while (leadingChar != "" && counter < 2)
        {
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            yield return new WaitForSeconds(0.5f);
            counter++;
            tmp_text.text += leadingChar;
            yield return new WaitForSeconds(0.5f);
            //yield return null;
        }
        foreach (char c in text_to_animate)
        {
            if (tmp_text.text.Length > 0)
            {
                tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            }
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - 1);
            tmp_text.text += leadingChar;
            audioManager.PlaySFX("KeyTap");
            yield return new WaitForSeconds(rate);
        }
        counter = 0;
        /*while (leadingChar != "" && counter < 2)
        {
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            yield return new WaitForSeconds(rate * 2);
            counter++;
            tmp_text.text += leadingChar;
            yield return new WaitForSeconds(rate * 2);
            //yield return null;
        }*/
        if (leadingChar != "") tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
        yield break;
    }

    public IEnumerator AnimateTypewriterCheckpoint(TMP_Text tmp_text, string text_to_animate, string leadingChar = "", float rate = 0.25f, bool deleteText = true)
    {

        tmp_text.text = "";
        foreach (char c in text_to_animate)
        {
            if (tmp_text.text.Length > 0)
            {
                tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            }
            tmp_text.text += c;
            tmp_text.text += leadingChar;
            audioManager.PlaySFX("KeyTap");
            yield return new WaitForSeconds(rate);
        }
        int counter = 0;
        while (leadingChar != "" && counter < 2)
        {
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            yield return new WaitForSeconds(0.5f);
            counter++;
            tmp_text.text += leadingChar;
            yield return new WaitForSeconds(0.5f);
            if (!deleteText) counter = 0;
            //yield return null;
        }
        
        foreach (char c in text_to_animate)
        {
            if (tmp_text.text.Length > 0)
            {
                tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            }
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - 1);
            tmp_text.text += leadingChar;
            audioManager.PlaySFX("KeyTap");
            yield return new WaitForSeconds(rate);
        }
        counter = 0;
        /*while (leadingChar != "" && counter < 2)
        {
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            yield return new WaitForSeconds(rate * 2);
            counter++;
            tmp_text.text += leadingChar;
            yield return new WaitForSeconds(rate * 2);
            //yield return null;
        }*/
        if (leadingChar != "") tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
    }

    public IEnumerator AnimateDamageNum(GameObject num, Transform enemyTrans, float rate)
    {
        var text = num.GetComponent<TMP_Text>();
        var textBackdrop = num.transform.GetChild(0).GetComponent<TMP_Text>();
        //textBackdrop.color 
        float xOffset = Random.Range(-100.0f, 0.0f);
        float yOffset = Random.Range(0.0f, 120.0f);
        Vector3 screenPos = Vector3.zero;
        
        while (text.color.a > 0)
        {
            xOffset += 100f * Time.deltaTime;
            if(enemyTrans != null) screenPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(enemyTrans.position);
            num.transform.position = new Vector3((screenPos.x + xOffset), (screenPos.y + yOffset), screenPos.z);
            Color txtColor = text.color;
            Color txtBackdropColor = textBackdrop.color;
            txtColor.a -= rate * Time.deltaTime;
            txtBackdropColor.a -= rate * Time.deltaTime;
            text.color = txtColor;
            textBackdrop.color = txtBackdropColor;
            yield return null;
        }
        Destroy(num);
        yield break;
    }

    public IEnumerator AnimateCriticalTextBorder()
    {
        while (true)
        {
            yield return currentCriticalBorderOpacity = StartCoroutine(IncreaseTextOpacity(criticalTextBorder, 1.0f));
            yield return currentCriticalBorderOpacity = StartCoroutine(ReduceTextOpacity(criticalTextBorder, 1.0f));
        }
    }

    public void DisplayThankYouScreen()
    {
        var menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        var inputManager = GameObject.Find("InputandAnimationManager").GetComponent<masterInput>();
        var thankYouPage = Instantiate(thankYouScreen);
        thankYouPage.transform.SetParent(GameObject.Find("Canvas").gameObject.transform, false);
        menuManager.menusPaused = true;
        inputManager.pausePlayerInput();
        Time.timeScale = 0.0f;
        StartCoroutine(animateThankYouScreen(thankYouPage.transform.Find("DemoScreen").gameObject));
    }

    public IEnumerator AnimateWarningScreen()
    {
        while (true)
        {
            yield return currentCriticalBorderOpacity = StartCoroutine(IncreaseTextOpacity(warningScreen, 1.0f));
            yield return currentCriticalBorderOpacity = StartCoroutine(ReduceTextOpacity(warningScreen, 1.0f));
        }
    }

    public IEnumerator AnimateCriticalText()
    {
        
        while (true)
        {
            yield return currentCriticalOpacity = StartCoroutine(IncreaseTextOpacity(criticalText, 1.0f));
            yield return currentCriticalOpacity = StartCoroutine(ReduceTextOpacity(criticalText, 1.0f)); 
        }
        
    }

    public void Initialize()
    {
        ExperienceBar.value = character.weaponClass.totalExp;
        UpdateClass(character.weaponClass.classType, character.weaponClass.currentLvl);
        //UpdateExperienceBar(character.weaponClass.getCurrentLvlExperienceAmount(), character.weaponClass.getNextLvlExperienceAmount(), character.weaponClass.totalExp);
    }

    public IEnumerator InstantiateSmear(float angle, Vector3 pos)
    {
        yield return new WaitForSeconds(0.15f);
        var currentSmear = Instantiate(dashSmear);
        currentSmear.transform.SetParent(GameObject.Find("SplashCanvas").transform, false);
        currentSmear.transform.position = new Vector3(pos.x, character.transform.position.y + 0.15f, pos.z);
        currentSmear.transform.rotation = Quaternion.Euler(currentSmear.transform.eulerAngles.x, currentSmear.transform.eulerAngles.y, -angle);
        StartCoroutine(IncreaseImageOpacity(currentSmear, 4f));
        currentSmears.Enqueue(currentSmear);
        yield break;
    }

    public void DestroyOldestSmear(bool instant=false)
    {
        if(currentSmears.Count > 0)
        {
            if (instant)
            {
                Destroy(currentSmears.Dequeue());
            }
            else StartCoroutine(AnimateDestroyOldestSmear(currentSmears.Dequeue()));

        }   
    }
    public IEnumerator AnimateDestroyOldestSmear(GameObject smear)
    {
        yield return StartCoroutine(DecreaseImageOpacity(smear, 3.0f));
        Destroy(smear);
        
    }



    public void ActivateCooldownOnAbility(int abilityNum, bool isSpell=false)
    {
        GameObject ability = null;
        if (!isSpell)
        {
            switch (abilityNum)
            {
                case 1:
                    ability = ability1;
                    break;
                case 2:
                    ability = ability2;
                    break;
                case 3:
                    ability = ability3;
                    break;
            }
        }
        else
        {
            switch (abilityNum)
            {
                case 1:
                    ability = ability_spell1;
                    break;
                case 2:
                    ability = ability_spell2;
                    break;
                case 3:
                    ability = ability_spell3;
                    break;
            }
        }
        
        ability.transform.Find("AbilityCooldownFill").gameObject.SetActive(true);
        ability.transform.Find("AbilityBorderCooldown").gameObject.SetActive(true);
        ability.transform.Find("AbilityBorderNormal").gameObject.SetActive(false);
        ability.transform.Find("AbilityCooldownBorder").gameObject.SetActive(false);
        ability.transform.Find("AbilityCooldownBorderDeactivated").gameObject.SetActive(true);


        
        if (!isSpell)
        {
            //var reducedAlpha_local = ability.transform.Find("AbilityIcon").gameObject.GetComponent<Image>().color;
            //reducedAlpha_local.a = 0.5f;
            //ability.transform.Find("AbilityIcon").gameObject.GetComponent<Image>().color = reducedAlpha_local;
            ReduceClassAbilityAlphas(abilityNum);
        }
        else
        {
            ReduceSpellRuneAlphas(abilityNum);
        }

        var reducedAlpha = ability.transform.Find("AbilityNumber").gameObject.GetComponent<TMP_Text>().color;
        reducedAlpha.a = 0.5f;
        ability.transform.Find("AbilityNumber").gameObject.GetComponent<TMP_Text>().color = reducedAlpha;

        //reducedAlpha = ability.transform.Find("AbilityCooldownBorderDeactivated").gameObject.GetComponent<Image>().color;
        //reducedAlpha.a = 0.5f;
        //ability.transform.Find("AbilityCooldownBorderDeactivated").gameObject.GetComponent<Image>().color = reducedAlpha;
    }

    public IEnumerator AnimateCooldownSlider(Slider currentAbilitySlider, float rate)
    {
        
        while(currentAbilitySlider != null && currentAbilitySlider.value != 1)
        {
            currentAbilitySlider.value += rate * Time.deltaTime;
            if (Mathf.Abs(currentAbilitySlider.value - 1) <= 0.01) currentAbilitySlider.value = 1;
            yield return null;
        }
        yield break;
    }

    public IEnumerator StartCooldownSlider(int abilityNum, float rate, bool isSpell=false)
    {
        Slider currentAbilitySlider = null;
        if (!isSpell)
        {
            switch (abilityNum)
            {
                case 1:
                    currentAbilitySlider = ability1.transform.Find("AbilityCooldownFill").GetComponent<Slider>();
                    break;
                case 2:
                    currentAbilitySlider = ability2.transform.Find("AbilityCooldownFill").GetComponent<Slider>();
                    break;
                case 3:
                    currentAbilitySlider = ability3.transform.Find("AbilityCooldownFill").GetComponent<Slider>();
                    break;
            }
        }
        else
        {
            switch (abilityNum)
            {
                case 1:
                    currentAbilitySlider = ability_spell1.transform.Find("AbilityCooldownFill").GetComponent<Slider>();
                    break;
                case 2:
                    currentAbilitySlider = ability_spell2.transform.Find("AbilityCooldownFill").GetComponent<Slider>();
                    break;
                case 3:
                    currentAbilitySlider = ability_spell3.transform.Find("AbilityCooldownFill").GetComponent<Slider>();
                    break;
            }
        }
        
        yield return StartCoroutine(AnimateCooldownSlider(currentAbilitySlider, rate));
    }

    public void DeactivateCooldownOnAbility(int abilityNum, bool isSpell=false)
    {
        GameObject ability = null;
        if (!isSpell)
        {
            switch (abilityNum)
            {
                case 1:
                    ability = ability1;
                    break;
                case 2:
                    ability = ability2;
                    break;
                case 3:
                    ability = ability3;
                    break;
            }
        }
        else
        {
            switch (abilityNum)
            {
                case 1:
                    ability = ability_spell1;
                    break;
                case 2:
                    ability = ability_spell2;
                    break;
                case 3:
                    ability = ability_spell3;
                    break;
            }
        }
         
        ability.transform.Find("AbilityCooldownFill").gameObject.GetComponent<Slider>().value = 0.0f;
        ability.transform.Find("AbilityCooldownFill").gameObject.SetActive(false);
        ability.transform.Find("AbilityBorderCooldown").gameObject.SetActive(false);
        ability.transform.Find("AbilityBorderNormal").gameObject.SetActive(true);
        ability.transform.Find("AbilityCooldownBorder").gameObject.SetActive(true);
        ability.transform.Find("AbilityCooldownBorderDeactivated").gameObject.SetActive(false);

        if (!isSpell)
        {
            //var increasedAlpha_local = ability.transform.Find("AbilityIcon").gameObject.GetComponent<Image>().color;
            //increasedAlpha_local.a = 1.0f;
            //ability.transform.Find("AbilityIcon").gameObject.GetComponent<Image>().color = increasedAlpha_local;
            IncreaseClassAbilityAlphas(abilityNum);
        }
        else
        {
            IncreaseSpellRuneAlphas(abilityNum);
        }
        

        var increasedAlpha = ability.transform.Find("AbilityNumber").gameObject.GetComponent<TMP_Text>().color;
        increasedAlpha.a = 1.0f;
        ability.transform.Find("AbilityNumber").gameObject.GetComponent<TMP_Text>().color = increasedAlpha;

        //increasedAlpha = ability.transform.Find("AbilityCooldownBorder").gameObject.GetComponent<Image>().color;
        //increasedAlpha.a = 1.0f;
        //ability.transform.Find("AbilityCooldownBorder").gameObject.GetComponent<Image>().color = increasedAlpha;
    }
    
    public void UpdateClass(WeaponBase.weaponClassTypes weaponClass, int experienceLVL, bool changingClass = false)
    {
        switch (weaponClass)
        {
            case WeaponBase.weaponClassTypes.Knight:
                knightHUD.SetActive(true);
                gunnerHUD.SetActive(false);
                engineerHUD.SetActive(false);
                
                break;
            case WeaponBase.weaponClassTypes.Gunner:
                knightHUD.SetActive(false);
                gunnerHUD.SetActive(true);
                engineerHUD.SetActive(false);
                break;
            case WeaponBase.weaponClassTypes.Engineer:
                knightHUD.SetActive(false);
                gunnerHUD.SetActive(false);
                engineerHUD.SetActive(true);
                break;
        }
        UpdateExperienceLevel(weaponClass, experienceLVL, changingClass);
        PopulateClassAbilities();
        
    }

    public void UpdateExperienceBar(float current)
    {
        
        StartCoroutine(AnimateExperienceBar(current));
        //ExperienceBar.value = current;
    }

    public IEnumerator AnimateExperienceBar(float current)
    {
        while (ExperienceBar.value < current)
        {
            ExperienceBar.value = Mathf.Lerp(ExperienceBar.value, current, 3.0f * Time.deltaTime);
            if (Mathf.Abs(ExperienceBar.value - current) < 0.5) ExperienceBar.value = current;
            if (Mathf.Abs(ExperienceBar.value - ExperienceBar.maxValue) < 0.5) yield break;
            yield return null;
        }
        Debug.Log("Bar finished in AnimateExperienceBar");
        yield break;
    }

    public void ChangeExperienceLvl(float min, float max)
    {
        ExperienceBar.minValue = min;
        ExperienceBar.maxValue = max;
    }
    

    public IEnumerator AnimateExperienceUpdate(WeaponBase.weaponClassTypes weaponClass, int experienceLVL, bool changingClass = false)
    {
        if (!changingClass)
        {
            yield return StartCoroutine(AnimateExperienceBar(character.weaponClass.getCurrentLvlExperienceAmount()));
            Debug.Log("Bar finished in AnimateExperienceUpdate");
        }
        

        switch (weaponClass)
        {
            case WeaponBase.weaponClassTypes.Knight:
                knightHUD.transform.Find("KnightLVLTextMain").GetComponent<TMP_Text>().text = experienceLVL.ToString();
                knightHUD.transform.Find("KnightLVLTextSub").GetComponent<TMP_Text>().text = experienceLVL.ToString();
                break;
            case WeaponBase.weaponClassTypes.Gunner:
                gunnerHUD.transform.Find("GunnerLVLTextMain").GetComponent<TMP_Text>().text = experienceLVL.ToString();
                gunnerHUD.transform.Find("GunnerLVLTextSub").GetComponent<TMP_Text>().text = experienceLVL.ToString();
                break;
            case WeaponBase.weaponClassTypes.Engineer:
                engineerHUD.transform.Find("EngineerLVLTextMain").GetComponent<TMP_Text>().text = experienceLVL.ToString();
                engineerHUD.transform.Find("EngineerLVLTextSub").GetComponent<TMP_Text>().text = experienceLVL.ToString();
                break;
        }
        ChangeExperienceLvl(character.weaponClass.getCurrentLvlExperienceAmount(), character.weaponClass.getNextLvlExperienceAmount());
        if (!changingClass)
        {
            yield return StartCoroutine(AnimateExperienceBar(character.weaponClass.totalExp));
        }
        else
        {
            ExperienceBar.value = character.weaponClass.totalExp;
        }
        yield break;
        //
        
    }

    public void EnableCriticalBorders()
    {
        StartCoroutine(AnimateCriticalBorders());
    }

    public void DisableCriticalBorders()
    {
        topRedBorder.SetActive(false);
        bottomRedBorder.SetActive(false);
        StopCoroutine(AnimateCriticalBorders());
    }

    public IEnumerator AnimateCriticalBorders()
    {
        topRedBorder.SetActive(true);
        bottomRedBorder.SetActive(true);
        var top = topRedBorder.GetComponent<Image>();
        var bottom = bottomRedBorder.GetComponent<Image>();

        Color imgColorTop = top.color;
        Color imgColorBottom = bottom.color;
        imgColorTop.a = 0;
        imgColorBottom.a = 0;
        top.color = imgColorTop;
        bottom.color = imgColorBottom;
        //Color 

        while(true)
        {
            while(top.color.a < 1)
            {
                Color imgColorTopA = top.color;
                Color imgColorBottomA = bottom.color;
                imgColorTopA.a += 3f * Time.deltaTime;
                imgColorBottomA.a += 3f * Time.deltaTime;
                top.color = imgColorTopA;
                bottom.color = imgColorBottomA;
                yield return null;
            }
            yield return new WaitForSeconds(0.75f);
            while (top.color.a > 0)
            {
                Color imgColorTopA = top.color;
                Color imgColorBottomA = bottom.color;
                imgColorTopA.a -= 3f * Time.deltaTime;
                imgColorBottomA.a -= 3f * Time.deltaTime;
                top.color = imgColorTopA;
                bottom.color = imgColorBottomA;
                yield return null;
            }
            yield return new WaitForSeconds(0.75f);
            yield return null;
        }
    }

    public IEnumerator greyOutSwapIcon()
    {
        greyedOutSwapUI.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        greyedOutSwapUI.SetActive(false);
    }

    public void SwitchAbilityUI()
    {
        if (abilityUIActive)
        {
            abilitiesUI.SetActive(false);
            spellsUI.SetActive(true);
            abilityUIActive = false;
            PopulateSpellRunes();
            Spell_Text.SetActive(true);
            Abilities_Text.SetActive(false);
        }
        else
        {
            abilitiesUI.SetActive(true);
            spellsUI.SetActive(false);
            abilityUIActive = true;
            PopulateClassAbilities();
            Spell_Text.SetActive(false);
            Abilities_Text.SetActive(true);
        }
        StopCoroutine(greyOutSwapIcon());
        StartCoroutine(greyOutSwapIcon());
    }

    public void ReduceSpellRuneAlphas(int index)
    {
        int count = spellAbility1.transform.childCount;
        switch (index)
        {
            case 1:
                for (int j = 0; j < count; j++)
                {
                    var child = spellAbility1.transform.GetChild(j);
                    if (child.gameObject.activeSelf)
                    {
                        var reducedAlpha = child.gameObject.GetComponent<Image>().color;
                        reducedAlpha.a = 0.5f;
                        child.gameObject.GetComponent<Image>().color = reducedAlpha;
                    }
                }
                break;
            case 2:
                for (int j = 0; j < count; j++)
                {
                    var child = spellAbility2.transform.GetChild(j);
                    if (child.gameObject.activeSelf)
                    {
                        var reducedAlpha = child.gameObject.GetComponent<Image>().color;
                        reducedAlpha.a = 0.5f;
                        child.gameObject.GetComponent<Image>().color = reducedAlpha;
                    }
                }
                break;
            case 3:
                for (int j = 0; j < count; j++)
                {
                    var child = spellAbility3.transform.GetChild(j);
                    if (child.gameObject.activeSelf)
                    {
                        var reducedAlpha = child.gameObject.GetComponent<Image>().color;
                        reducedAlpha.a = 0.5f;
                        child.gameObject.GetComponent<Image>().color = reducedAlpha;
                    }
                }
                break;
        }
        
    }

    public void ReduceClassAbilityAlphas(int index)
    {
        int count = classAbility1.transform.childCount;
        switch (index)
        {
            case 1:
                for (int j = 0; j < count; j++)
                {
                    var child = classAbility1.transform.GetChild(j);
                    if (child.gameObject.activeSelf)
                    {
                        var reducedAlpha = child.gameObject.GetComponent<Image>().color;
                        reducedAlpha.a = 0.5f;
                        child.gameObject.GetComponent<Image>().color = reducedAlpha;
                    }
                }
                break;
            case 2:
                for (int j = 0; j < count; j++)
                {
                    var child = classAbility2.transform.GetChild(j);
                    if (child.gameObject.activeSelf)
                    {
                        var reducedAlpha = child.gameObject.GetComponent<Image>().color;
                        reducedAlpha.a = 0.5f;
                        child.gameObject.GetComponent<Image>().color = reducedAlpha;
                    }
                }
                break;
            case 3:
                for (int j = 0; j < count; j++)
                {
                    var child = classAbility3.transform.GetChild(j);
                    if (child.gameObject.activeSelf)
                    {
                        var reducedAlpha = child.gameObject.GetComponent<Image>().color;
                        reducedAlpha.a = 0.5f;
                        child.gameObject.GetComponent<Image>().color = reducedAlpha;
                    }
                }
                break;
        }

    }

    public void IncreaseSpellRuneAlphas(int index)
    {
        int count = spellAbility1.transform.childCount;
        switch (index)
        {
            case 1:
                for (int j = 0; j < count; j++)
                {
                    var child = spellAbility1.transform.GetChild(j);
                    if (child.gameObject.activeSelf)
                    {
                        var reducedAlpha = child.gameObject.GetComponent<Image>().color;
                        reducedAlpha.a = 1.0f;
                        child.gameObject.GetComponent<Image>().color = reducedAlpha;
                    }
                }
                break;
            case 2:
                for (int j = 0; j < count; j++)
                {
                    var child = spellAbility2.transform.GetChild(j);
                    if (child.gameObject.activeSelf)
                    {
                        var reducedAlpha = child.gameObject.GetComponent<Image>().color;
                        reducedAlpha.a = 1.0f;
                        child.gameObject.GetComponent<Image>().color = reducedAlpha;
                    }
                }
                break;
            case 3:
                for (int j = 0; j < count; j++)
                {
                    var child = spellAbility3.transform.GetChild(j);
                    if (child.gameObject.activeSelf)
                    {
                        var reducedAlpha = child.gameObject.GetComponent<Image>().color;
                        reducedAlpha.a = 1.0f;
                        child.gameObject.GetComponent<Image>().color = reducedAlpha;
                    }
                }
                break;
        }

    }

    public void IncreaseClassAbilityAlphas(int index)
    {
        int count = spellAbility1.transform.childCount;
        switch (index)
        {
            case 1:
                for (int j = 0; j < count; j++)
                {
                    var child = classAbility1.transform.GetChild(j);
                    if (child.gameObject.activeSelf)
                    {
                        var reducedAlpha = child.gameObject.GetComponent<Image>().color;
                        reducedAlpha.a = 1.0f;
                        child.gameObject.GetComponent<Image>().color = reducedAlpha;
                    }
                }
                break;
            case 2:
                for (int j = 0; j < count; j++)
                {
                    var child = classAbility2.transform.GetChild(j);
                    if (child.gameObject.activeSelf)
                    {
                        var reducedAlpha = child.gameObject.GetComponent<Image>().color;
                        reducedAlpha.a = 1.0f;
                        child.gameObject.GetComponent<Image>().color = reducedAlpha;
                    }
                }
                break;
            case 3:
                for (int j = 0; j < count; j++)
                {
                    var child = classAbility3.transform.GetChild(j);
                    if (child.gameObject.activeSelf)
                    {
                        var reducedAlpha = child.gameObject.GetComponent<Image>().color;
                        reducedAlpha.a = 1.0f;
                        child.gameObject.GetComponent<Image>().color = reducedAlpha;
                    }
                }
                break;
        }

    }

    public void PopulateSpellRunes()
    {
        var runes = character.equippedRunes;
        for(int i =0; i < runes.Length; i++)
        {
            
            if (i == 0)
            {
                int count = spellAbility1.transform.childCount;
                
                for (int j = 0; j < count; j++)
                {
                    var child = spellAbility1.transform.GetChild(j);
                    if (runes[i] == null)
                    {
                        child.gameObject.SetActive(false);
                        continue;
                    }
                    if (child.name == runes[i].runeName) child.gameObject.SetActive(true);
                    else child.gameObject.SetActive(false);
                }
            }
            if (i == 1)
            {
                int count = spellAbility2.transform.childCount;

                for (int j = 0; j < count; j++)
                {
                    var child = spellAbility2.transform.GetChild(j);
                    if (runes[i] == null)
                    {
                        child.gameObject.SetActive(false);
                        continue;
                    }
                    if (child.name == runes[i].runeName) child.gameObject.SetActive(true);
                    else child.gameObject.SetActive(false);
                }
            }
            if (i == 2)
            {
                int count = spellAbility3.transform.childCount;

                for (int j = 0; j < count; j++)
                {
                    var child = spellAbility3.transform.GetChild(j);
                    if (runes[i] == null)
                    {
                        child.gameObject.SetActive(false);
                        continue;
                    }
                    if (child.name == runes[i].runeName) child.gameObject.SetActive(true);
                    else child.gameObject.SetActive(false);
                }
            }
        }
    }

    public void PopulateClassAbilities()
    {
        var currentClass = character.weaponClass.classType;
        string abilityIconToActivate = "";
        switch (currentClass)
        {
            case WeaponBase.weaponClassTypes.Knight:
                abilityIconToActivate = "KnightAbilityIcon";
                break;
            case WeaponBase.weaponClassTypes.Gunner:
                abilityIconToActivate = "GunnerAbilityIcon";
                break;
            case WeaponBase.weaponClassTypes.Engineer:
                abilityIconToActivate = "EngineerAbilityIcon";
                break;
        }

        int count = classAbility1.transform.childCount;
        for (int j = 0; j < count; j++)
        {
            var child = classAbility1.transform.GetChild(j);
            if (child.name == abilityIconToActivate) child.gameObject.SetActive(true);
            else child.gameObject.SetActive(false);
        }

        count = classAbility2.transform.childCount;
        for (int j = 0; j < count; j++)
        {
            var child = classAbility2.transform.GetChild(j);
            if (child.name == abilityIconToActivate) child.gameObject.SetActive(true);
            else child.gameObject.SetActive(false);
        }

        count = classAbility3.transform.childCount;
        for (int j = 0; j < count; j++)
        {
            var child = classAbility3.transform.GetChild(j);
            if (child.name == abilityIconToActivate) child.gameObject.SetActive(true);
            else child.gameObject.SetActive(false);
        }

    }

    public void UpdateExperienceLevel(WeaponBase.weaponClassTypes weaponClass, int experienceLVL, bool changingClass = false)
    {
        StartCoroutine(AnimateExperienceUpdate(weaponClass, experienceLVL, changingClass));
       
    }

    public void StartLevelUpText()
    {
        StartCoroutine(AnimateLevelUpText());
    }
    public void StartAttackUpText()
    {
        StartCoroutine(AnimateAttackUpText());
    }

    public void StartAnimateExpText(string exp)
    {
        if(currentExpText != null)
        {
            StopCoroutine(currentExpText);
            Vector3 ogPosition = new Vector3(ogExpTextXPos, expText.transform.localPosition.y, expText.transform.localPosition.z);
            expText.transform.localPosition = ogPosition;
        }
        currentExpText = StartCoroutine(AnimateExpText(exp));
    }

    public IEnumerator AnimateExpText(string exp)
    {
        expText.GetComponent<TMP_Text>().text = "+" + exp;
        Vector3 ogPosition = new Vector3(expText.transform.localPosition.x, expText.transform.localPosition.y, expText.transform.localPosition.z);
        Vector3 desPos = new Vector3(expText.transform.localPosition.x + 50f, expText.transform.localPosition.y, expText.transform.localPosition.z);
        //8.900024
        expText.transform.localPosition = ogPosition;



        bool increasingOpacity = false;
        while (expText.transform.localPosition.x < desPos.x)
        {
            if (!increasingOpacity)
            {
                StartCoroutine(IncreaseTextOpacity(expText, 2.0f));
            }
            if (Mathf.Abs(expText.transform.localPosition.x - (desPos.x)) < 0.3f)
            {
                expText.transform.localPosition = desPos;
            }
            else
            {
                expText.transform.localPosition = Vector3.Lerp(expText.transform.localPosition, desPos, 5.0f * Time.deltaTime);
            }
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(ReduceTextOpacity(expText, 1.0f));
        
        yield break;
    }

    public IEnumerator AnimateLevelUpText()
    {
        Vector3 ogPosition = levelUpText.transform.localPosition;
        levelUpText.transform.localPosition = new Vector3(levelUpText.transform.localPosition.x - 2000.0f, levelUpText.transform.localPosition.y, levelUpText.transform.localPosition.z);
        
        bool increasingOpacity = false;
        while (levelUpText.transform.localPosition.x < ogPosition.x)
        {
             if((levelUpText.transform.localPosition.x > ogPosition.x - 1000.0f) && !increasingOpacity)
            {
                StartCoroutine(IncreaseImageOpacity(levelUpText, 4.0f));
                increasingOpacity = true;
            }
             if(Mathf.Abs(levelUpText.transform.localPosition.x - ogPosition.x) < 0.3f){
                levelUpText.transform.localPosition = ogPosition;
             }
             else
             {
                levelUpText.transform.localPosition = Vector3.Lerp(levelUpText.transform.localPosition, ogPosition, 15.0f*Time.deltaTime);
             }
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(DecreaseImageOpacity(levelUpText, 1.0f));
        yield return StartCoroutine(AnimateAttackUpText());
        yield break;
    }

    public IEnumerator AnimateAttackUpText()
    {
        Vector3 ogPosition = attackUpText.transform.localPosition;
        attackUpText.transform.localPosition = new Vector3(attackUpText.transform.localPosition.x - 2000.0f, attackUpText.transform.localPosition.y, attackUpText.transform.localPosition.z);

        bool increasingOpacity = false;
        while (attackUpText.transform.localPosition.x < ogPosition.x)
        {
            if ((attackUpText.transform.localPosition.x > ogPosition.x - 1000.0f) && !increasingOpacity)
            {
                StartCoroutine(IncreaseTextOpacity(attackUpText, 4.0f));
            }
            if (Mathf.Abs(attackUpText.transform.localPosition.x - ogPosition.x) < 0.3f)
            {
                attackUpText.transform.localPosition = ogPosition;
            }
            else
            {
                attackUpText.transform.localPosition = Vector3.Lerp(attackUpText.transform.localPosition, ogPosition, 15.0f * Time.deltaTime);
            }
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(ReduceTextOpacity(attackUpText, 1.0f));
        yield break;
    }

    public IEnumerator AnimateCheckpointReached()
    {
        Debug.Log("Animating Checkpoing text");
        currentCheckpointText = Instantiate(CheckpointText);
        currentCheckpointText.transform.SetParent(mainCanvas.transform, false);
        currentCheckpointText.transform.localScale = new Vector3(6.0f, 6.0f, 6.0f);
        var rate = new Vector3(20f, 20f, 20f);
        while(currentCheckpointText.transform.localScale.x != 1.0f)
        {
           Debug.Log(currentCheckpointText.transform.localScale);
            if((currentCheckpointText.transform.localScale.x - (rate.x * Time.deltaTime)) <= 1.0f)
            {
                currentCheckpointText.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                yield return null;
            }
            else
            {
                currentCheckpointText.transform.localScale -= (rate * Time.deltaTime);
                yield return null;
            }

        }
        //yield return new WaitForSeconds(2.8f);
        yield return StartCoroutine(AnimateTypewriterCheckpoint(currentCheckpointText.GetComponent<TMP_Text>(), "Checkpoint Reached", "|", 0.125f));
        Destroy(currentCheckpointText);
        yield break;
    }

    public IEnumerator AnimateBossName(string bossName)
    {
        Debug.Log("Animating Boss Name");
        currentBossText = Instantiate(bossText);
        currentBossText.transform.SetParent(mainCanvas.transform, false);
        currentBossText.transform.localScale = new Vector3(6.0f, 6.0f, 6.0f);
        var rate = new Vector3(20f, 20f, 20f);
        while (currentBossText.transform.localScale.x != 1.0f)
        {
            Debug.Log(currentBossText.transform.localScale);
            if ((currentBossText.transform.localScale.x - (rate.x * Time.deltaTime)) <= 1.0f)
            {
                currentBossText.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                yield return null;
            }
            else
            {
                currentBossText.transform.localScale -= (rate * Time.deltaTime);
                yield return null;
            }

        }
        //yield return new WaitForSeconds(2.8f);
        yield return StartCoroutine(AnimateTypewriterCheckpoint(currentBossText.GetComponent<TMP_Text>(), bossName, "|", 0.125f));
        Destroy(currentBossText);
        yield break;
    }

    public IEnumerator DecreaseImageOpacity(GameObject image, float rate)
    {
        var reference = image.GetComponent<Image>();
        while (reference.color.a >= 0.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a -= rate * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
        yield break;
    }

    public IEnumerator IncreaseImageOpacity(GameObject image, float rate, bool setToZero=false)
    {
        var reference = image.GetComponent<Image>();
        if (setToZero)
        {
            Color imgColor = reference.color;
            imgColor.a = 0;
            reference.color = imgColor;
        }
        while (reference.color.a < 1.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a += rate * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
        yield break;
    }

    private IEnumerator ReduceTextOpacity(GameObject text, float rate)
    {
        var reference = text.GetComponent<TMP_Text>();
        while (reference.color.a >= 0.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a -= rate * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
        yield break;
    }

    private IEnumerator IncreaseTextOpacity(GameObject text, float rate, bool setToZero= false)
    {
        var reference = text.GetComponent<TMP_Text>();
        if (setToZero)
        {
            Color imgColor = reference.color;
            imgColor.a = 0;
            reference.color = imgColor;
        }
        while (reference.color.a <= 1.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a += rate * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
        yield break;
    }

}
