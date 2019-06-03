using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public AudioClip Logins;
    public AudioClip Loginf;

    //인풋 필드
    public InputField login;
    public InputField password;

    //UI 게임 오브젝트
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

        //ID 와 비밀번호 영어와 숫자로만 입력 받을 수 있게 체크
        char[] ids = login.text.ToCharArray();
        char[] pass = password.text.ToCharArray();

        for(int i=0; i<ids.Length; i++)
        {
            //문자나 숫자가 아닌 경우 return
            if(!char.IsLetterOrDigit(ids[i]))
            {
                Debug.Log("ID 혹은 PASSWORD는 영문과 숫자만 입력 가능합니다");
            }
        }
        for(int i=0; i<pass.Length; i++)
        {
            if (!char.IsLetterOrDigit(pass[i]))
            {
                Debug.Log("ID 혹은 PASSWORD는 영문과 숫자만 입력 가능합니다");
            }
        }

       //if(false)
       if(Client.instance.Login(login.text, password.text))
        {
            //login success
            Debug.Log("Login Success");
            GetComponent<AudioSource>().clip = Logins;
            GetComponent<AudioSource>().Play();
            LoginUI.SetActive(false);
            SelectCharacterUI.SetActive(true);
        
        }
        else
        {
            Debug.Log("Login Fail");
            GetComponent<AudioSource>().clip = Loginf;
            GetComponent<AudioSource>().Play();
            login.text = "";
            password.text = "";
        }
    }

    public void SignUp()
    {
        Debug.Log(login.text);
        Debug.Log(password.text);

        char[] ids = login.text.ToCharArray();
        char[] pass = password.text.ToCharArray();

        for (int i = 0; i < ids.Length; i++)
        {
            //문자나 숫자가 아닌 경우 return
            if (!char.IsLetterOrDigit(ids[i]))
            {
                Debug.Log("ID 혹은 PASSWORD는 영문과 숫자만 입력 가능합니다");
            }
        }
        for (int i = 0; i < pass.Length; i++)
        {
            if (!char.IsLetterOrDigit(pass[i]))
            {
                Debug.Log("ID 혹은 PASSWORD는 영문과 숫자만 입력 가능합니다");
            }
        }


        if (Client.instance.SignUp(login.text, password.text))
        {
            //login success
            Debug.Log("SignUp Success");
            GetComponent<AudioSource>().clip = Logins;
            GetComponent<AudioSource>().Play();
            login.text = "";
            password.text = "";

        }
        else
        {
            Debug.Log("SignUp Fail");
            GetComponent<AudioSource>().clip = Loginf;
            GetComponent<AudioSource>().Play();
            login.text = "";
            password.text = "";
        }
    }


}
