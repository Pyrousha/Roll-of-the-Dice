using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool isPlayerCharacter;

    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int health;

    public int physicalAttack;
    public int magicAttack;
    public int defense;
    public float critMultiplier = 2f;
    public float clumsyPenalty = 3f;

    public Animator anim;

    public GameObject moveRangeObj;
    public GameObject hitRangeObj;
    public GameObject equipment;

    public void Damage(int hp) {
        health -= hp;
        if(health <= 0) Die();
    }

    public void Die() {
        // TODO
        anim.SetTrigger("Die");
        gameObject.SetActive(false);
    }

    public AbilityDice[] GetEquippedDice() {
        return equipment.GetComponentsInChildren<AbilityDice>();
    }

    public void DoTurn() {} // for AI use
}  
