using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    //GAME RES
    public int resWidth;
    public int resHeight;
    public string options;
    public string back1;
    public string back2;
    public string back3;
    public string graphics;
    public string volume;

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
    public void optionsmenubutton()
    {
        options = "jgvjhgv";
        SceneManager.LoadScene(options);
    }
    public void playGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
 
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
