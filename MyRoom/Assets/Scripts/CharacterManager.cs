using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterManager: MonoBehaviour
{ //SelectButton, QuitButton;
    bool m_SceneLoaded;

    private Client cli;

    void Start()
    {
        cli = GameObject.Find("NetworkManager").GetComponent<Client>();
    }

    // Load the Scene when this Button is pressed
    public void LoadSceneButton()
    {
        // Check that the second Scene hasn't been added yet
        if (m_SceneLoaded == false)
        {

            //선택한 캐릭터 정보 보내기 , 일단 0번
            cli.CharacterSelect(0);

            //Main 씬 호출
            // Loads the second Scene
            SceneManager.LoadScene("Main");
            
            
            //불려진 씬 확인 디버그
            Debug.Log("Active Scene : " + SceneManager.GetActiveScene().name);

            // The Scene has been loaded, exit this method
            m_SceneLoaded = true;
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
}