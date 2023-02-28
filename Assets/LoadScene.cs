using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public Button getStarted, SignIn;
    // Start is called before the first frame update
    void Start()
    {
        getStarted.onClick.AddListener(() => SceneManager.LoadScene("SignUp"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
