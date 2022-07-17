using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControlDice : AbilityDice
{
    float knockback = 4f;

    public void Bash(Character target)
    {
        Character c = GetComponentInParent<Character>();
        Vector2 unit = (new Vector2(target.transform.position.x, target.transform.position.y) - new Vector2(c.transform.position.x, c.transform.position.y)).normalized;
        target.Damage((c.physicalAttack+c.Buffed) / 4);
        StartCoroutine(BashMove(target, c.transform.position+new Vector3(unit.x, unit.y, 0)*knockback));
    }

    public void SuperBash(Character target)
    {
        Character c = GetComponentInParent<Character>();
        Vector2 unit = (new Vector2(target.transform.position.x, target.transform.position.y) - new Vector2(c.transform.position.x, c.transform.position.y)).normalized;
        target.Damage(c.physicalAttack+c.Buffed);
        StartCoroutine(BashMove(target, c.transform.position+new Vector3(unit.x, unit.y, 0)*Mathf.FloorToInt(knockback*1.5f)));
    }

    public void Miss(Character target)
    {
        target.Damage(0);
    }

    public void Trip(Character target)
    {
        Character c = GetComponentInParent<Character>();
        Vector2 unit = (new Vector2(target.transform.position.x, target.transform.position.y) - new Vector2(c.transform.position.x, c.transform.position.y)).normalized;
        target.Damage((c.physicalAttack+c.Buffed) / 8);
        c.Damage((c.physicalAttack+c.Buffed) / 8);
        StartCoroutine(BashMove(target, c.transform.position+new Vector3(unit.x, unit.y, 0)*knockback));
    }

    IEnumerator BashMove(Character target, Vector3 targetPosition)
    {
        while (Vector3.Distance(target.transform.position, targetPosition) > target.epsilon)
        {
            //player is not done moving yet

            //currentAlly.transform.position = Vector3.Lerp(currentAlly.transform.position, targetPosition, Time.deltaTime * moveSpeed);
            target.transform.position = Vector3.Lerp(target.transform.position, targetPosition, Time.deltaTime * target.moveSpeed);

            yield return null;
        }
        target.transform.position = targetPosition;
    }


}