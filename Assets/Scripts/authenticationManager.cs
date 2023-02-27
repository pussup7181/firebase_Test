using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using System;
using UnityEngine.SceneManagement;

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
    

    [Space]
    [Header("Registration")]
    public TMP_InputField nameRegistrationField;
    public TMP_InputField emailRegistrationField;
    public TMP_InputField passwordRegistrationField;
    public TMP_InputField confirmPasswordRegistrationField;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();

            }
            else
            {
                Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus); 
            }
        });
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if(auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed Out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed In " + user.UserId);
            }
        }
    }

    public void Login()
    {
        StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);
        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);
            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;
            string failedMessage = "Login Failed! Because: ";
            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is Missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "password is Missing";
                    break;
                default:
                    failedMessage = "Login Failed";
                    break;
            }
            Debug.Log(failedMessage);
        }
        else
        {
            user = loginTask.Result;
            Debug.LogFormat("{0} You are successfully logged in.", user.DisplayName);
            References.userName = user.DisplayName;
            SceneManager.LoadScene("GameScene");
        }
    }

    public void Register()
    {
        StartCoroutine(RegisterAsync(nameRegistrationField.text, emailRegistrationField.text, passwordRegistrationField.text, confirmPasswordRegistrationField.text));
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if(name == "")
        {
            Debug.LogError("User Name is Empty");
        }
        else if(email == "")
        {
            Debug.LogError("Email Field is Empty");
        }
        else if(passwordRegistrationField.text != confirmPasswordRegistrationField.text)
        {
            Debug.LogError("Password does not match");
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => registerTask.IsCompleted);
            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);
                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Registration Failed! Because: ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is Missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "password is Missing";
                        break;
                    default:
                        failedMessage = "Login Failed";
                        break;
                }
                Debug.Log(failedMessage);
            }
            else
            {
                user = registerTask.Result;
                UserProfile userProfile = new UserProfile { DisplayName = name };
                var updateProfileTask = user.UpdateUserProfileAsync(userProfile);
                yield return new WaitUntil(() => updateProfileTask.IsCompleted);
                if(updateProfileTask.Exception != null)
                {
                    user.DeleteAsync();
                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;

                    string failedMessage = "Registration Failed! Because: ";
                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += "Email is invalid";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "Wrong Password";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "Email is Missing";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "password is Missing";
                            break;
                        default:
                            failedMessage = "Login Failed";
                            break;
                    }
                    Debug.Log(failedMessage);
                }
                else
                {
                    Debug.Log("Registration Successful Welcome " + user.DisplayName);
                    UIManager.Instance.OpenLoginPanel();
                }

            }
        }
    }
}

