using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;
    public InputField passwordField;
    public Button loginButton;
    public Button startButton;

    // sign up
    public Button singUpButton;
    public Button submitSingUpButton;
    public InputField userSignUp;
    public InputField emailSignUp;
    public InputField passwordSignUp;
    public Button backButton;
    public GameObject wrongSignIn;
    public GameObject wrongLogIn;


    public static UIManager getInstance()
    {
        return instance;
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            // Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        Client.instance.ConnectToServer();
        startButton.gameObject.SetActive(false);
        usernameField.gameObject.SetActive(true);
        passwordField.gameObject.SetActive(true);
        loginButton.gameObject.SetActive(true);
        singUpButton.gameObject.SetActive(true);
    }

    public void Login()
    {
        wrongSignIn.gameObject.SetActive(false);
        wrongLogIn.gameObject.SetActive(false);

        Client.instance.Login();
    }

    public void SignUp()
    {
        wrongSignIn.gameObject.SetActive(false);
        wrongLogIn.gameObject.SetActive(false);

        Client.instance.SignUp();
    }

    public void ShowSignUp()
    {
        DontShowMenu();
        submitSingUpButton.gameObject.SetActive(true);
        userSignUp.gameObject.SetActive(true);
        emailSignUp.gameObject.SetActive(true);
        passwordSignUp.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);

        wrongSignIn.gameObject.SetActive(false);
        wrongLogIn.gameObject.SetActive(false);
    }

    public void BackButton()
    {
        DontShowMenu();
        usernameField.gameObject.SetActive(true);
        passwordField.gameObject.SetActive(true);
        loginButton.gameObject.SetActive(true);
        singUpButton.gameObject.SetActive(true);

        wrongSignIn.gameObject.SetActive(false);
        wrongLogIn.gameObject.SetActive(false);
    }

    public void DontShowMenu()
    {
        usernameField.gameObject.SetActive(false);
        passwordField.gameObject.SetActive(false);
        loginButton.gameObject.SetActive(false);
        singUpButton.gameObject.SetActive(false);
        submitSingUpButton.gameObject.SetActive(false);
        userSignUp.gameObject.SetActive(false);
        emailSignUp.gameObject.SetActive(false);
        passwordSignUp.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        wrongSignIn.gameObject.SetActive(false);
        wrongLogIn.gameObject.SetActive(false);
    }

    public void WrongSingIn()
    {
        wrongSignIn.gameObject.SetActive(true);
        wrongLogIn.gameObject.SetActive(false);
    }

    public void WrongLogIn()
    {
        wrongSignIn.gameObject.SetActive(false);
        wrongLogIn.gameObject.SetActive(true);
    }
}
