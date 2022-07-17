using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberAndFXCanvas : Singleton<DamageNumberAndFXCanvas>
{
    [SerializeField] private GameObject damageNumberPrefab;
    [SerializeField] private GameObject damageFXPrefab;
    [SerializeField] private GameObject healFXPrefab;
    [SerializeField] private GameObject missFXPrefab;

    public void SpawnDamageNumber(Transform characterHit, int damageDone)
    {
        Vector3 position = characterHit.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
        position.z = 0;
        Transform dmgNumberObj = Instantiate(damageNumberPrefab, transform.position, Quaternion.Euler(0,0,0)).transform;
        
        dmgNumberObj.SetParent(transform, false);
        dmgNumberObj.localPosition = position;

        dmgNumberObj.GetChild(0).GetComponent<DamageNumber>().SetNumber(damageDone);

        if(damageDone > 0)
        {
            //spawn damage fx
            Instantiate(damageFXPrefab, characterHit.position + new Vector3(0,0,0.5f), Quaternion.Euler(0,0,0));
            return;
        }

        if(damageDone < 0)
        {
            //spawn heal fx
            Instantiate(healFXPrefab, characterHit.position + new Vector3(0,0,0.5f), Quaternion.Euler(0,0,0));
            return;
        }

        //spawn miss fx
        Instantiate(missFXPrefab, characterHit.position + new Vector3(0,0,0.5f), Quaternion.Euler(0,0,0));
    }
}
