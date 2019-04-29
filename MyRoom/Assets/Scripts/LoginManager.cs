using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public InputField login;
    public InputField password;

    //private Client cli = Client.instance;
    // Start is called before the first frame update
    void Start()
    {
        //cli = GameObject.Find("NetworkManager").GetComponent<Client>();
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

        
        if(true)
        //if(Client.instance.Login(login.text, password.text))
        {
            //login success
            Debug.Log("Login Success");

            GameObject CanvasLogin = transform.parent.Find("CanvasLogin").gameObject;
            CanvasLogin.SetActive(false);

            
            GameObject SelectCharacter = transform.parent.Find("SelectCharacter").gameObject;
            SelectCharacter.SetActive(true);
            
            
            
            
        }
        else
        {
            Debug.Log("Login Fail");
        }
    }


}
