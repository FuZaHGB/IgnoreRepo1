using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditorInternal;
using UnityEngine;

public class DestroyableObject : AttackCollisionTrigger
{
    public float objectHealth;
    public float playerDamageVal;
    public bool isDestroyed;
    public bool isDecorative;

    public GameObject scenery;
    public GameObject extraScenery1;

    public GameObject door;
    
    protected override void TriggerEvent()
    {
        Debug.Log("DestoryableObject TriggerEvent called");
        if (playerDamageVal == 0)
        {
            Debug.Log("Need to set PlayerDamageVal!");
            return;
        }

        if (!isDestroyed)
        {
            if (objectHealth > 0)
            {
                objectHealth -= playerDamageVal;
                Debug.Log(transform + " Object Health = " + objectHealth);
            }
            else
            {
                Debug.Log("Object Destroyed!");
                isDestroyed = true;

                ActivateDamageDetails();
            }
        }
    }

    private void ActivateDamageDetails()
    {
        if (isDecorative) // i.e. is the generator object at the start of the level etc.
        {
            GameObject explosionEffect = this.transform.Find("Explosion_A").gameObject;
            GameObject smoke = this.transform.Find("smoke_thick").gameObject;
            GameObject sparks = this.transform.Find("Spark").gameObject;

            explosionEffect.SetActive(true);
            smoke.SetActive(true);
            sparks.SetActive(true);

            if(scenery != null)
            {
                GameObject pointlight = scenery.transform.Find("Point light").gameObject;
                pointlight.GetComponent<Light>().color = UnityEngine.Color.red;
                if (extraScenery1 != null)
                {
                    extraScenery1.GetComponent<MeshRenderer>().material.color = UnityEngine.Color.red;
                }
            }

            // For interior door in hallway
            if(door != null)
            {
                Animator m_Animator = door.GetComponent<Animator>();
                m_Animator.SetTrigger("character_nearby");
                Debug.Log("Attempted to open door");
            }

        }
        else // i.e. An Enemy Agent
        {
            Debug.Log("Enemy Defeated!");
            GetComponent<Animator>().enabled = false;
            GameObject meshCollider = this.transform.Find("MESH_Infantry").gameObject;
            Debug.Log("MESH COLLIDER: " + meshCollider);
            //meshCollider.GetComponent<MeshCollider>().enabled = false;
            Destroy(meshCollider.GetComponent<MeshCollider>());

            setRigidbodyState(false);
            setColliderState(true);
            applyExplosionForce();

            GetComponentInParent<Score>().IncrementScore();

            isDestroyed = true;
        }

        void setRigidbodyState(bool state)
        {
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = state;
            }
            GetComponent<Rigidbody>().isKinematic = !state;
        }

        void setColliderState(bool state)
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.enabled = state;
            }
            //GetComponent<Collider>().enabled = !state;
        }

        void applyExplosionForce()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 50f);

            foreach (Collider collider in colliders)
            {
                Rigidbody rigidbody = collider.GetComponent<Rigidbody>();

                if (rigidbody != null)
                {
                    rigidbody.AddExplosionForce(250f, transform.position, 50f);
                }
            }
        }
    }
}
