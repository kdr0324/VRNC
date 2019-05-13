using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public InputField login;
    public InputField password;

    public GameObject LoginUI;
    public GameObject SelectCharacterUI;

    //private Client cli = Client.instance;
    // Start is called before the first frame update

    public void Quit()
    {
        Debug.Log("ClickedQuit");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		        Application.Quit();
#endif
    }
    public void Login()
    {
        Debug.Log(login.text);
        Debug.Log(password.text);

        
        //if(true)
        if(Client.instance.Login(login.text, password.text))
        {
            //login success
            Debug.Log("Login Success");
            LoginUI.SetActive(false);
            SelectCharacterUI.SetActive(true);
        
        }
        else
        {
            Debug.Log("Login Fail");
        }
    }


}
