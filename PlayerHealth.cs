using Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : AttackCollisionTrigger
{

    public float playerHealth;
    public float enemyDamage;
    public bool isDead;
    public Text healthStatus;
    Animator anim;

    void Start()
    {
        healthStatus.text = "HEALTH: " + playerHealth;
        anim = GetComponent<Animator>();
    }

    protected override void TriggerEvent()
    {
        Debug.Log("Playerhealth TriggerEvent called");
        if (enemyDamage == 0)
        {
            Debug.LogError("Enemy damage must be set!");
        }
        if (!isDead)
        {
            if (playerHealth > 0)
            {
                playerHealth -= enemyDamage;
                healthStatus.text = "HEALTH: " + playerHealth;
                Debug.Log(transform + " Object Health = " + playerHealth);
            }
            else
            {
                Debug.Log("Object Destroyed!");
                isDead = true;

                playerDead();
            }
        }
    }

    void playerDead()
    {
        healthStatus.text = "YOU HAVE DIED... AGAIN.";
        healthStatus.color = UnityEngine.Color.red;
        GetComponent<PlayerMovement>().enabled = false;
        anim.SetBool("isDead", true);
        GameObject camera = this.transform.Find("Main Camera").gameObject;
        camera.GetComponent<MouseLook>().enabled = false;
    }
}
