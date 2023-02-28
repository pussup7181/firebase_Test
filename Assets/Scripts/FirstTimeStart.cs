using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstTimeStart : MonoBehaviour
{
    public bool isFirstTime;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("isFirstTime"))
        {
            isFirstTime = false;
            SceneManager.LoadScene("SignUporSignIn");
        }
        else
        {
            isFirstTime = true;
            PlayerPrefs.SetInt("isFirstTime", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
