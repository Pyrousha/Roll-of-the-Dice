using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGrunt : Character
{
    public override void DoTurn()
    {
        // TODO: limit attack range
        targetPosition = GetClosestPlayerCharacter().transform.position;
        int abilityIndex = Mathf.FloorToInt(Random.value * deck.Length);
        //deck[abilityIndex].DoAction(target);
        DoAbility(deck[abilityIndex]);
    }
}