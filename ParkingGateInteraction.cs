using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingGateInteraction : AttackCollisionTrigger
{
    public GameObject gate;
    public float keypadHealth;
    public float playerDamageVal;
    public bool isDestroyed;
    Animator anim;
    protected override void TriggerEvent()
    {
        Debug.Log("DestroyableObject TriggerEvent called");
        if (playerDamageVal == 0)
        {
            Debug.Log("Need to set PlayerDamageVal!");
            return;
        }

        if (!isDestroyed)
        {
            if (keypadHealth > 0)
            {
                keypadHealth -= playerDamageVal;
                Debug.Log(transform + " Object Health = " + keypadHealth);
            }
            else
            {
                Debug.Log("Object Destroyed!");
                isDestroyed = true;

                activateGate();
            }
        }
    }

    private void activateGate()
    {
        anim = gate.GetComponent<Animator>();
        anim.SetBool("isOpening", true);
        GameObject sparks = this.transform.Find("Spark").gameObject;
        sparks.SetActive(true);
    }

}
