using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class shopUI : MonoBehaviour
{
    public static shopUI instance;
    [SerializeField] private GameObject shopInterface;
    [SerializeField] TextMeshProUGUI shopGunName;
    [SerializeField] private GameObject pressE;
    private bool triggerActive = false;
    public GameObject gunShopModel;
    public gunStats[] gunSelection;
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
        }

        if (Input.GetButtonDown("Cancel") && gameManager.instance.activeMenu == shopInterface)
        {
            turnOffShopUI();
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
            triggerActive = false;
            pressE.SetActive(false);
        }
    }

    public void setGunModel(int iterator = 0)
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
        setGunModel();
        shopGunName.text = gunSelection[0].gunName.ToString();
    }

    private void turnOffShopUI()
    {
        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.activeMenu = null;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pressE.SetActive(true);
        gameManager.instance.turnCameraOn = true;
    }

}
