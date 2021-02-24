using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelRestartLogic : MonoBehaviour
{
    public GameObject starDestroyer;
    public Text restartText;

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<PlayerHealth>().isDead)
        {
            restartText.text = "DEAD. \n Press 'P' to restart level.";

            if (Input.GetKey(KeyCode.P))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
        else if (starDestroyer.GetComponent<StarDestroyerParent>().isDestroyed)
        {
            restartText.text = "Congratulations! \n Press 'P' to restart level.";

            if (Input.GetKey(KeyCode.P))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }
}
