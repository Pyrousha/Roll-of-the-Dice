using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    PlayerMovement playerController;

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

    public void StartPlayerTurn()
    {
        playerController.StartPlayerTurn();
    }

    public void EndPlayerTurn()
    {
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
