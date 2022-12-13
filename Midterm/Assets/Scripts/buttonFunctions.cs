using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.gameManagerAud.PlayOneShot(gameManager.instance.unpauseSound, gameManager.instance.unpauseSoundVolume);
        gameManager.instance.unPause();
        gameManager.instance.isPaused = !gameManager.instance.isPaused;
    }

    public void restart()
    {
        gameManager.instance.unPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.gameManagerAud.PlayOneShot(gameManager.instance.restartSound, gameManager.instance.restartSoundVolume);
    }

    public void quit()
    {
        Application.Quit();
    }


    public void respawnPlayer()
    {
        gameManager.instance.playerScript.resetPlayerHP();

        gameManager.instance.unPause();

        gameManager.instance.playerScript.setPlayerPos();

    }  
}
