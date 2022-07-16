using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCard : MonoBehaviour
{
    public Sprite icon;
    
    public virtual void DoAction(Character target) {}
}