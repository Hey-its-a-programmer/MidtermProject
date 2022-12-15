using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class shopUI : MonoBehaviour
{
    //Made Into Singlton
    //Couldn't get reference of object in Shop UI without it.
    //Probably should be in Game Manager
    [SerializeField] AudioSource shopAud;
    public static shopUI instance;
    [SerializeField] private GameObject shopInterface;
    [SerializeField] TextMeshProUGUI shopGunName;
    [SerializeField] private GameObject pressE;
    private bool triggerActive = false;
    [SerializeField] GameObject gunShopModel;
    [SerializeField] gunStats[] gunSelection;
    private int shopIterator;

    //shop theme
    [SerializeField] public AudioClip shopMusic;
    [Range(0, 1)] [SerializeField] public float shopMusicVolume;

    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Player"))
        {
            triggerActive = true;
            pressE.SetActive(true);
        }
        
    }

    private void Update()
    {
        if (triggerActive && Input.GetKey(KeyCode.E))
        {
            turnOnShopUI();
            instance = this;
        }

        if (Input.GetButtonDown("Cancel") && gameManager.instance.activeMenu == shopInterface)
        {
            gameManager.instance.activeMenu.SetActive(false);
            Cursor.visible = false;
            gameManager.instance.turnCameraOn = true;
            pressE.SetActive(true);
            shopAud.Stop();
        }
    }

    private void shopActions()
    {
        gameManager.instance.activeMenu = shopInterface;
        gameManager.instance.activeMenu.SetActive(true);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager.instance.activeMenu != null)
            {
                turnOffShopUI();
            }
            triggerActive = false;
            pressE.SetActive(false);
        }
    }

    public void setGunModel(int iterator)
    {
        gunShopModel.GetComponent<MeshFilter>().sharedMesh = gunSelection[iterator].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunShopModel.GetComponent<MeshRenderer>().sharedMaterial = gunSelection[iterator].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    private void turnOnShopUI()
    {
        shopActions();
        pressE.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        gameManager.instance.turnCameraOn = false;
        setGunModel(shopIterator);
        shopGunName.text = gunSelection[shopIterator].gunName.ToString();
        shopAud.clip = shopMusic;
        shopAud.loop = true;
        shopAud.Play();
    }

    private void turnOffShopUI()
    {
        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.activeMenu = null;
        Cursor.visible = false;
        gameManager.instance.turnCameraOn = true;
    }


    public void next()
    {
        if (shopIterator < gunSelection.Length - 1)
        {
            shopIterator++;
            setGunModel(shopIterator);
            shopGunName.text = gunSelection[shopIterator].gunName.ToString();
        }

    }

    public void prev()
    {
        if (shopIterator > 0)
        {
            shopIterator--;
            setGunModel(shopIterator);
        }

    }

    public void buy()
    {
        if (gunSelection[shopIterator].gunPrice <= gameManager.instance.playerScript.coins && 0 <= gameManager.instance.playerScript.coins - gunSelection[shopIterator].gunPrice )
        {
            gameManager.instance.playerScript.coins -= gunSelection[shopIterator].gunPrice;
            gameManager.instance.playerScript.gunPickup(gunSelection[shopIterator]);
        }

    }
}
