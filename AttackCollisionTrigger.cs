using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackCollisionTrigger : MonoBehaviour
{
    private bool _AttackInsideTrigger = false;

    private void Awake()
    {
        /*if (!GetComponent<BoxCollider>().isTrigger)
        {
            Debug.LogError("Ensure the object's box collider is set to trigger");
            enabled = false;
            return;
        } */
    }

    // Update is called once per frame
    void Update()
    {
        if (_AttackInsideTrigger)
        {
            Debug.Log("Attempting to trigger event");
            TriggerEvent();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(transform + " OnTriggerEnter triggered");
        Debug.Log(collider.tag);
        if ((collider.CompareTag("MeleeAttack") && transform.tag != "Player")|| collider.CompareTag("Projectile"))
        {
            _AttackInsideTrigger = true;
            Debug.Log(_AttackInsideTrigger);
        }
        else
        {
            return;
        }
    }

    protected abstract void TriggerEvent();

    private void OnTriggerExit(Collider collider)
    {
        if ((collider.CompareTag("MeleeAttack") && transform.tag != "Player") || collider.CompareTag("Projectile"))
        {
            _AttackInsideTrigger = false;
        }
        else
        {
            return;
        }
    }
}
