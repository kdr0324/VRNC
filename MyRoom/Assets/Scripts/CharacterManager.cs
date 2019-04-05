using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterManager: MonoBehaviour
{ //SelectButton, QuitButton;
    bool m_SceneLoaded;
    public Button SelectButton, QuitButton;

    void Awake()
    {
        // Outputs the current active Scene to the console
        Debug.Log("Active Scene : " + SceneManager.GetActiveScene().name);

        // Check that this Button exists
        if (SelectButton != null)
        {
            // Fetch the Button from the Inspector. Make sure to set this in the Inspector window
            Button loadButton = SelectButton.GetComponent<Button>();

            // Call the LoadScene2Button() function when this Button is clicked
            loadButton.onClick.AddListener(LoadSceneButton);
        }

        if (QuitButton != null)
        {
            Button buttonTwo = QuitButton.GetComponent<Button>();
            buttonTwo.onClick.AddListener(SetActiveSceneButton);
        }
    }

    // Load the Scene when this Button is pressed
    public void LoadSceneButton()
    {
        // Check that the second Scene hasn't been added yet
        if (m_SceneLoaded == false)
        {
            // Loads the second Scene
            SceneManager.LoadScene("Main", LoadSceneMode.Additive);

            // Outputs the name of the current active Scene.
            // Notice it still outputs the name of the first Scene
            Debug.Log("Active Scene : " + SceneManager.GetActiveScene().name);

            // The Scene has been loaded, exit this method
            m_SceneLoaded = true;
        }
    }

    // Change the newly loaded Scene to be the active Scene if it is loaded
    public void SetActiveSceneButton()
    {
        // Allow this other Button to be pressed when the other Button has been pressed (Scene 2 is loaded)
        if (m_SceneLoaded == true)
        {
            // Set Scene2 as the active Scene
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));

            // Ouput the name of the active Scene
            // See now that the name is updated
            Debug.Log("Active Scene : " + SceneManager.GetActiveScene().name);
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