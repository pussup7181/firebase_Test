using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;

public class authenticationManager : MonoBehaviour
{
    //Firebase
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    [Space]
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public Button signup;

    [Space]
    [Header("Registration")]
    public TMP_InputField nameRegistrationField;
    public TMP_InputField emailRegistrationField;
    public TMP_InputField passwordRegistrationField;
    public TMP_InputField confirmPasswordRegistrationField;


   
}
