using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class InteractionPromptUI : MonoBehaviour
{
    private Camera _mainCam;
    [SerializeField] private GameObject _uiPanel;
    [SerializeField]private TextMeshProUGUI promptText;
    private void Start()
    {
        _mainCam = Camera.main;
        _uiPanel.SetActive(false);
    }

    private void LateUpdate()
    {
       var rotation = _mainCam.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);   
        
    }
    public bool IsDisplayed = false;
    public void SetUp(string promptText)
    {
        
        _uiPanel.SetActive(true);
        IsDisplayed = true;
    }
    public void Close()
    {
        _uiPanel.SetActive(false);
        IsDisplayed = false;
    }

    internal void SetUp(object interactionPrompt)
    {
        throw new NotImplementedException();
    }
}
