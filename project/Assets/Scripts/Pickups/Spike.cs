using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enemy;

public class Spike : MonoBehaviour
{	
    private IEnumerator OnCollisionEnter(Collision other)
    {
        //print(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            yield return other.gameObject.GetComponent<PlayerHealth>().ChangeHpWithKnockback(-1, this.gameObject.transform);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().ChangeEnemyHp(-1);
        }
    }
}
