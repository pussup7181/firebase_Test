using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;

public class authenticationManager : MonoBehaviour
{
    public TMP_InputField Email;
    public TMP_InputField password;
    public Button signup;
    FirebaseAuth auth;
    // Start is called before the first frame update
    void Start()
    {
         auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Email.onValueChanged.AddListener(delegate
        {
            Debug.Log(Email.text);
            if (Email.text.Contains("@"))
            {
                
            }
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void signUp()
    {
        auth.CreateUserWithEmailAndPasswordAsync(Email.text, password.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            
        });
    }
}
