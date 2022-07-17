using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberAndFXCanvas : Singleton<DamageNumberAndFXCanvas>
{
    [SerializeField] private GameObject damageNumberPrefab;
    [SerializeField] private GameObject damageFXPrefab;
    [SerializeField] private GameObject healFXPrefab;
    [SerializeField] private GameObject missFXPerfab;

    public void SpawnDamageNumber(Transform characterHit, int damageDone)
    {
        Vector3 position = characterHit.position;
        position.z = 0;
        Transform dmgNumberObj = Instantiate(damageNumberPrefab, transform.position, Quaternion.Euler(0,0,0)).transform;
        
        dmgNumberObj.parent = transform;
        dmgNumberObj.localPosition = position;

        dmgNumberObj.GetChild(0).GetComponent<DamageNumber>().SetNumber(damageDone);
    }
}
