using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playertypeKnight : MonoBehaviour
{

    public Animator anim;
    public float cooldown = 1f;
    public float nextAttackTime = .3f;
    private static int noOfClicks = 0;
    private float lastClickedTime = 0;
    public float maxComboDelay = .7f;
    public float animTime = 0.5f;
    public float animTimeTwo = 0.5f;
    public float animTimeThree = 0.99f;
    public float differenceTime = .02f;
    public float animDiff = 1.2f;

    //Sword instantiations
    public GameObject[] swords;
    public Transform hand;


    public PlayerController playerCon; 

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        playerCon = gameObject.GetComponent<PlayerController>();
        GameObject currentSword = swords[0];
        Instantiate(currentSword, hand);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        if(Time.time > lastClickedTime + nextAttackTime && playerCon.isAttacking == false)
        {
            if(Input.GetMouseButtonDown(0))
            {
                //print("click: " + noOfClicks);
                lastClickedTime = Time.time;
                noOfClicks++;
                if (noOfClicks >= 1)
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("attackTwo") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .99f)
                    {
                        noOfClicks = 0;
                        return;
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("attackThree") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < animTimeThree)
                    {
                        noOfClicks = 0;
                        return;
                    }
                    //anim.SetTrigger("Attack");
                    anim.SetBool("attack1", true);
                    StartCoroutine(wait(animTime));
                    anim.Play("attackOne");
                    //nextAttackTime = anim.GetCurrentAnimatorStateInfo(0).length - differenceTime;
                    //print("Anim: " + anim.GetBool("attack1"));
                }
                noOfClicks = Mathf.Clamp(noOfClicks, 1, 3);

                if (noOfClicks >= 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > animTime && anim.GetCurrentAnimatorStateInfo(0).IsName("attackOne"))
                {
                    anim.SetBool("attack2", true);
                    anim.SetBool("attack1", false);
                    anim.Play("attackTwo");
                    StartCoroutine(wait(animTimeTwo));
                }

                if (noOfClicks >= 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > animTimeTwo && anim.GetCurrentAnimatorStateInfo(0).IsName("attackTwo"))
                {
                    nextAttackTime += differenceTime;
                    noOfClicks = 0;
                    anim.SetBool("attack3", true);
                    anim.SetBool("attack2", false);
                    anim.SetBool("attack1", false);
                    anim.Play("attackThree");
                    StartCoroutine(waitThree(animTimeThree + animDiff));
                    anim.SetBool("attack3", false);
                    nextAttackTime -= differenceTime;
                    
                }
                else
                {
                    
                    if (anim.GetBool("attack2") == true)
                    {
                        anim.SetBool("attack2", false);
                        anim.SetBool("attack1", false);
                    }
                    else
                    {
                        anim.ResetTrigger("Attack");
                        anim.SetBool("attack1", false);
                    }
                }
            }
        }
    }

    IEnumerator wait(float animationTime)
    {
        playerCon.isAttacking = true;
        yield return new WaitForSeconds(animationTime);
        playerCon.isAttacking = false;
        yield break;
    }

    IEnumerator waitThree(float animationTime)
    {
        playerCon.isAttacking = true;
        yield return new WaitForSeconds(animationTime);
        playerCon.isAttacking = false;
        yield break;
    }


    public void onClickRun()
    {
        lastClickedTime = Time.time;
        //noOfClicks++;
        if(noOfClicks >= 1)
        {
            anim.SetTrigger("Attack");
            anim.SetBool("attack1", true);
        }
        //noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        if(noOfClicks >= 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
        {
            anim.SetBool("attack2", true);
        }

        if (noOfClicks >= 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("attack2"))
        {
            anim.SetBool("attack3", true);
            anim.SetBool("attack3", false);
        }
        else
        {
            if(anim.GetBool("attack2") == true)
            {
                anim.SetBool("attack2", false);
                anim.SetBool("attack1", false);
            }
            else
            {
                anim.ResetTrigger("Attack");
                anim.SetBool("attack1", false);
            }
        }
    }
}
