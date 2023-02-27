using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private GameObject loginPanel;

    [SerializeField]
    private GameObject registrationPanel;

    [SerializeField]
    private GameObject emailVerificationPanel;
    
    [SerializeField]
    private TMP_Text emailVerificationText;
    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        registrationPanel.SetActive(false);
        emailVerificationPanel.SetActive(false);
    }

    public void OpenRegistrationPanel()
    {
        registrationPanel.SetActive(true);
        loginPanel.SetActive(false);
        emailVerificationPanel.SetActive(false);
    }

    public void ShowVerificationResponse(bool isEmailSent, string emailId, string errorMessage)
    {
        ClearUI();
        emailVerificationPanel.SetActive(true);
        if (isEmailSent)
        {
            emailVerificationText.text = $"Please verify your email address \n Verification Email has been sent to {emailId}";
        }
        else
        {
            emailVerificationText.text = $"Couldn't send email: {errorMessage}";
        }
    }

    public void ClearUI()
    {
        registrationPanel.SetActive(false);
        loginPanel.SetActive(false);
        emailVerificationPanel.SetActive(false);
    }
}
