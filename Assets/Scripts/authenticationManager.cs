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
    public TMP_InputField phoneNumber;
    public TMP_InputField OTP;


    [Space]
    [Header("Registration")]
    public TMP_InputField nameRegistrationField;
    public TMP_InputField emailRegistrationField;
    public TMP_InputField passwordRegistrationField;
    public TMP_InputField confirmPasswordRegistrationField;


    private uint phoneAuthTimeoutMs = 60 * 1000;
    PhoneAuthProvider provider;
    string VerificationId;

    private void Start()
    {
        StartCoroutine(CheckAndFixDependenciesAsync());
    }
    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);
        dependencyStatus = dependencyTask.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
            InitializeFirebase();
            yield return new WaitForEndOfFrame();
            StartCoroutine(CheckForAutoLogin());

        }
        else
        {
            Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus);
        }

    }
    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private IEnumerator CheckForAutoLogin()
    {
        if (user != null)
        {
            var reloadUserTask = user.ReloadAsync();
            yield return new WaitUntil(() => reloadUserTask.IsCompleted);
            AutoLogin();
        }
        else
        {
            UIManager.Instance.OpenLoginPanel();
        }
    }

    private void AutoLogin()
    {
        if (user != null)
        {
            if (user.IsEmailVerified)
            {
                References.userName = user.DisplayName;
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                SendEmailForVerification();
            }
            
        }
        else
        {
            UIManager.Instance.OpenLoginPanel();
        }
    }

    private void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
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

    public void logout()
    {
        if(auth!=null && user != null)
        {
            auth.SignOut();
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
            

            if (user.IsEmailVerified)
            {
                References.userName = user.DisplayName;
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                SendEmailForVerification();
            }
        }
    }

    public void Register()
    {
        StartCoroutine(RegisterAsync(nameRegistrationField.text, emailRegistrationField.text, passwordRegistrationField.text, confirmPasswordRegistrationField.text));
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (name == "")
        {
            Debug.LogError("User Name is Empty");
        }
        else if (email == "")
        {
            Debug.LogError("Email Field is Empty");
        }
        else if (passwordRegistrationField.text != confirmPasswordRegistrationField.text)
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
                if (updateProfileTask.Exception != null)
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
                    if (user.IsEmailVerified)
                    {
                        UIManager.Instance.OpenLoginPanel();
                    }
                    else
                    {
                        SendEmailForVerification();
                    }
                }

            }
        }
    }

    public void SendEmailForVerification()
    {
        StartCoroutine(SendEmailForVerificationAsync());
    }

    private IEnumerator SendEmailForVerificationAsync()
    {
        if (user != null)
        {
            var sendEmailTask = user.SendEmailVerificationAsync();
            yield return new WaitUntil(() => sendEmailTask.IsCompleted);
            if(sendEmailTask.Exception != null)
            {
                FirebaseException firebaseException = sendEmailTask.Exception.GetBaseException() as FirebaseException;
                AuthError error = (AuthError)firebaseException.ErrorCode;
                string errorMessage = "Unknown Error: Please Try again later.";
                switch (error) 
                {
                    case AuthError.Cancelled:
                        errorMessage = "Email Verification Was Cancelled";
                        break;
                    case AuthError.TooManyRequests:
                        errorMessage = "Too Many Request";
                        break;
                    case AuthError.InvalidRecipientEmail:
                        errorMessage = "The Email you entered is Invalid.";
                        break;
                }
                UIManager.Instance.ShowVerificationResponse(false, user.Email, errorMessage);
            }
            else
            {
                Debug.Log("Email Successfully sent.");
                UIManager.Instance.ShowVerificationResponse(true, user.Email, null);

            }
        }
    }

    public void phoneLogin()
    {
        provider = PhoneAuthProvider.GetInstance(auth);
        provider.VerifyPhoneNumber("+91"+phoneNumber.text, phoneAuthTimeoutMs, null,
          verificationCompleted: (credential) => {
      // Auto-sms-retrieval or instant validation has succeeded (Android only).
      // There is no need to input the verification code.
      // `credential` can be used instead of calling GetCredential().
  },
          verificationFailed: (error) => {
      // The verification code was not sent.
      // `error` contains a human readable explanation of the problem.
  },
          codeSent: (id, token) => {
              VerificationId = id;
      // Verification code was successfully sent via SMS.
      // `id` contains the verification id that will need to passed in with
      // the code from the user when calling GetCredential().
      // `token` can be used if the user requests the code be sent again, to
      // tie the two requests together.
  },
          codeAutoRetrievalTimeOut: (id) => {
      // Called when the auto-sms-retrieval has timed out, based on the given
      // timeout parameter.
      // `id` contains the verification id of the request that timed out.
  });
    }

    public void verifyOTP()
    {
        Credential credential = provider.GetCredential(VerificationId, OTP.text);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " +
                               task.Exception);
                return;
            }

            user = task.Result;
            Debug.Log("User signed in successfully");
            // This should display the phone number.
            Debug.Log("Phone number: " + user.PhoneNumber);
            // The phone number providerID is 'phone'.
            Debug.Log("Phone provider ID: " + user.ProviderId);

            References.userName = user.DisplayName;
            SceneManager.LoadScene("GameScene");
        });
    }
}

