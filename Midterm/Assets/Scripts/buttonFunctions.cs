using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
<<<<<<< HEAD
    //GAME RES
    public int resWidth;
    public int resHeight;

    public void setWidth(int newWidth)
    {
        resWidth = newWidth;
    }

    public void setHeight(int newHeight)
    {
        resHeight = newHeight;
    }

    public void setRes()
    {
        Screen.SetResolution(resWidth, resHeight, false);
    }
    //MENUS
    public void playGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
=======
 

>>>>>>> 63f2536df7f7f232164ee22e71d262bc26746e79
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
