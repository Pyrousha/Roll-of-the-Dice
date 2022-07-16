using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public DialoguePipeline dialogue;

    public Character goon;
    public Vector3 enterPoint;

    BattleManager battleManager;

    void Start()
    {
        battleManager = GetComponent<BattleManager>();
    }

    float goonDelay = 0f;

    void Update()
    {
        if(waitForGoon)
        {
            if(goonDelay > 0f)
            {
                goonDelay -= Time.deltaTime;
                if(goonDelay <= 0f)
                {
                    waitForGoon = false;
                    dialogue.gameObject.SetActive(true);
                }
            }
            if(goon.anim.GetBool("BlankOver"))
            {
                if(Vector3.Distance(goon.transform.position, enterPoint) > goon.epsilon)
                {
                    goon.transform.position = Vector3.Lerp(goon.transform.position, enterPoint, Time.deltaTime * goon.moveSpeed);
                } else {
                    goon.transform.position = enterPoint;
                    goon.anim.SetTrigger("SpinOver");
                    goon.anim.SetBool("BlankOver", false);
                    goonDelay = 1f;
                }
            }
        }
        else if(waitForCharacter)
        {
            if(battleManager.playerController.currentAlly != null)
            {
                waitForCharacter = false;
                dialogue.gameObject.SetActive(true);
            }
        }
        else if(waitForDie)
        {
            if(battleManager.playerController.activeDiceIndex != -1)
            {
                waitForDie = false;
                dialogue.gameObject.SetActive(true);
            }
        }
        else if(waitForWin)
        {
            if(goon.health <= 0)
            {
                waitForWin = false;
                dialogue.gameObject.SetActive(true);
            }
        }
    }

    bool waitForGoon = false;

    public void EnterGoon()
    {
        goon.anim.SetTrigger("DoSpin");
        waitForGoon = true;
    }

    bool waitForCharacter = false;

    public void SelectCharacter()
    {
        waitForCharacter = true;
    }

    bool waitForDie = false;

    public void SelectDie()
    {
        waitForDie = true;
    }

    bool waitForWin = false;
    public void Win()
    {
        waitForWin = true;
    }

    public void EndTutorial()
    {
        
    }
}