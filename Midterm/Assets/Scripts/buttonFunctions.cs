using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.unPause();
        gameManager.instance.isPaused = !gameManager.instance.isPaused;
    }

    public void restart()
    {
        gameManager.instance.unPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void addJump(int amount)
    {
        if (gameManager.instance.coins >= gameManager.instance.jumpCost)
        {
            gameManager.instance.playerScript.addJump(amount);
        }
    }

    public void respawnPlayer()
    {
        gameManager.instance.playerScript.resetPlayerHP();

        gameManager.instance.unPause();

        gameManager.instance.playerScript.setPlayerPos();

    }  
}
