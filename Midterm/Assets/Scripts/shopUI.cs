using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class shopUI : MonoBehaviour
{
    public static shopUI instance;
    [SerializeField] private GameObject shopInterface;
    [SerializeField] TextMeshProUGUI shopGunSelected;
    [SerializeField] private GameObject pressE;
    private bool triggerActive = false;
    public GameObject gunShopModel;
    public int shopSelectedGun = 0;
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
            
            shopActions();
            pressE.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            gameManager.instance.turnCameraOn = false;
            setShopGunMeshAndMaterial();
        }

        if (Input.GetButtonDown("Cancel") && gameManager.instance.activeMenu == shopInterface)
        {
            gameManager.instance.activeMenu.SetActive(false);
            gameManager.instance.activeMenu = null;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            pressE.SetActive(true);
            gameManager.instance.turnCameraOn = true;
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
                gameManager.instance.activeMenu.SetActive(false);
                gameManager.instance.activeMenu = null;
            }

            pressE.SetActive(false);
        }
    }

    public void setShopGunMeshAndMaterial()
    {
        gunShopModel.GetComponent<MeshFilter>().sharedMesh = gameManager.instance.playerScript.getGunSharedMesh();
        gunShopModel.GetComponent<MeshRenderer>().sharedMaterial = gameManager.instance.playerScript.getGunSharedMaterial();
    }


}
