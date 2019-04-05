using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public InputField login;
    public InputField password;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Login manager Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {

            Quit();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            

            //MakingRoom();
        }
    }



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
        Debug.Log("Login");
    }


}
