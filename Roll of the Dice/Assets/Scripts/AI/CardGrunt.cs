using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGrunt : Character
{
    public override void DoTurn()
    {
        // limit attack range
        if(confused > 0) TargetConfused();
        else
        {
            Character closest = GetClosestPlayerCharacter();
            Vector2 unit = (new Vector2(transform.position.x, transform.position.y) - new Vector2(closest.transform.position.x, closest.transform.position.y)).normalized;
            Vector2 bestPos = new Vector2(closest.transform.position.x, closest.transform.position.y) + unit * minDistance;
            if(moveRangeObj.GetComponent<Collider2D>().OverlapPoint(bestPos)) targetPosition = bestPos;
            else targetPosition = moveRangeObj.GetComponent<Collider2D>().ClosestPoint(bestPos);
            targetPosition.z = closest.transform.position.z;
        }
        int abilityIndex = Mathf.FloorToInt(Random.value * deck.Length);
        //deck[abilityIndex].DoAction(target);
        DoAbility(deck[abilityIndex]);
    }
}