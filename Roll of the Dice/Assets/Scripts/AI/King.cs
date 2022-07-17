using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Character
{
    public override void DoTurn()
    {
        
        if(Confused > 0) TargetConfused();
        else
        {
            // Summon goonies in multiple stages
        }
        int abilityIndex = Mathf.FloorToInt(Random.value * deck.Length);
        DoAbility(deck[abilityIndex]);
    }
}