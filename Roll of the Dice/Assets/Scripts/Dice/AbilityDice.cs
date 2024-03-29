using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilityDice : MonoBehaviour
{
    [System.Serializable]
    public struct DiceAction
    {
        public string name;
        public int numOfSides;
        public Sprite icon;
        public UnityEvent<Character> action;
    }

    [SerializeField]
    public DiceAction[] DiceSides;

    int totalSides;

    // Start is called before the first frame update
    void Start()
    {
        totalSides = 0;
        foreach(DiceAction da in DiceSides)
        {
            totalSides += da.numOfSides;
        }
    }

    public DiceAction RollAction()
    {
        int sideIndex = Mathf.FloorToInt(Random.value * totalSides);
        int actionIndex = 0;
        while(true)
        {
            sideIndex -= DiceSides[actionIndex].numOfSides;
            if(sideIndex < 0) break;
            actionIndex++;
        }
        return DiceSides[actionIndex];
    }

    public void DoAction(DiceAction action, Character target)
    {
        action.action?.Invoke(target);
    }
}
