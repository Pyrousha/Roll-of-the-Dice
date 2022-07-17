using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Color damageColor;
    [SerializeField] private Color healColor;
    [SerializeField] private Color missColor;

    void Start()
    {
        //SetColor(Color.red);
    }

    public void AnimOver()
    {
        Destroy(transform.parent.gameObject);
    }

    public void SetNumber(int damageDealt)
    {
        if (damageDealt < 0)
        {
            //heal
            text.faceColor = healColor;
            text.text = "+"+(-damageDealt).ToString();
            return;
        }

        if (damageDealt > 0)
        {
            //damage
            text.faceColor = damageColor;
            text.text = "-"+damageDealt.ToString();
            return;
        }

        //Zero
        text.faceColor = missColor;
        text.text = "0";
    }
}
