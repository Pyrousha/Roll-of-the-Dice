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

    public void EndPlayerTurn()
    {
        foreach(Character character in GetComponentsInChildren<Character>())
        {
            if(character.isPlayerCharacter) continue;
            character.DoTurn();
        }
    }
}
