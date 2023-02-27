using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text message;
    // Start is called before the first frame update
    void Start()
    {
        ShowWelcomeMessage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowWelcomeMessage()
    {
        message.text = $"Welcome {References.userName} to our Game Scene";
    }
}
