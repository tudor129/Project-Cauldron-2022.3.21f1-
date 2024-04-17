using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDetector : MonoBehaviour
{
    GameObject _deadEnemy;
    void OnTriggerEnter(Collider other)
    {
        Health enemyHealth = other.GetComponent<Health>();
        if (enemyHealth != null && enemyHealth.IsDead())
        {
            Debug.Log("Detected a dead enemy: " + other.gameObject.name);
            _deadEnemy = other.gameObject;
        }
       
    }

    void OnTriggerExit(Collider other)
    {
        Health enemyHealth = other.GetComponent<Health>();
        if (enemyHealth != null && enemyHealth.IsDead())
        {
            Debug.Log("A dead enemy left the detection area: " + other.gameObject.name);
        }
    }

    public GameObject GetDeadEnemy()
    {
        return _deadEnemy;
    }
}
