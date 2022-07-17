using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public GameObject UIFade;
    public GameObject battleResult;
    public GameObject newItems;
    public UnityEngine.UI.Slider healthSlider;

    public PlayerMovement playerController;
    bool battleOver = false;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerMovement>();
        StartBattle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartBattle()
    {
        playerController.StartPlayerTurn();
    }

    public Character[] GetAllPlayerCharacters()
    {
        List<Character> playerCharacters = new List<Character>();
        foreach(Character c in GetComponentsInChildren<Character>())
        {
            if(c.isPlayerCharacter) playerCharacters.Add(c);
        }
        return playerCharacters.ToArray();
    }

    public Character[] GetAllAICharacters()
    {
        List<Character> playerCharacters = new List<Character>();
        foreach(Character c in GetComponentsInChildren<Character>())
        {
            if(!c.isPlayerCharacter) playerCharacters.Add(c);
        }
        return playerCharacters.ToArray();
    }

    public void NotifyDeath(Character sender)
    {
        if(sender.isPlayerCharacter)
        {
            if(GetAllPlayerCharacters().Length == 0) 
            {
                EndBattle();
                //StartCoroutine(ShowLoss());
                ShowLoss();
            }
        }
        else
        {
            if(GetAllAICharacters().Length == 0)
            {
                EndBattle();
                //StartCoroutine(ShowWin());
                ShowWin();
            }
        }
    }

    void EndBattle()
    {
        battleOver = true;
        playerController.playerTurn = false;
        playerController.enabled = false;
        playerController.waitingAlly?.iconSprite.gameObject.SetActive(false);
        foreach(Character c in GetComponentsInChildren<Character>())
        {
            c.moveRangeObj?.SetActive(false);
            c.hitRangeObj?.SetActive(false);
        }
    }

    void ShowLoss()
    {
        UIFade.SetActive(true);
        battleResult.GetComponentInChildren<TextMeshProUGUI>().SetText("Defeat");
        battleResult.SetActive(true);
    }

    void ShowWin()
    {
        UIFade.SetActive(true);
        battleResult.GetComponentInChildren<TextMeshProUGUI>().SetText("Victory!");
        battleResult.SetActive(true);
    }

    /*
    IEnumerator ShowLoss()
    {
        UnityEngine.UI.Image fadeImg = UIFade.GetComponent<UnityEngine.UI.Image>();
        Color transparent = fadeImg.color;
        transparent.a = 0f;
        fadeImg.color = transparent;
        UIFade.SetActive(true);
        float time = 0f;
        while(time < 2f)
        {
            yield return new WaitForSeconds(1f/30f);
            transparent.a = transparent.a + 100f/(2f/60f);
            fadeImg.color = transparent;
        }
    }

    IEnumerator ShowWin()
    {
        
    }
    */

    public void StartPlayerTurn()
    {
        playerController.StartPlayerTurn();
    }

    public void EndPlayerTurn()
    {
        if(battleOver) return;
        /*
        foreach(Character character in GetComponentsInChildren<Character>())
        {
            if(character.isPlayerCharacter) continue;
            character.DoTurn();
            // TODO: check if all player characters are dead
        }
        playerController.StartPlayerTurn();
        */
        // TODO: check if all enemies are dead
        Character[] allAIs = GetAllAICharacters();
        if(allAIs.Length == 1) allAIs[0].DoTurn(null);
        else
        {
            Character[] chain = new Character[allAIs.Length-1];
            System.Array.Copy(allAIs, 1, chain, 0, allAIs.Length-1);
            allAIs[0].DoTurn(chain);
        }
    }
}
