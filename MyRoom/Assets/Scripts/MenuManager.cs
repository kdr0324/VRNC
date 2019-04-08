using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject furnitureMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Menu manager Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            
            Quit();
        }
        else if(Input.GetKeyUp(KeyCode.E))
        {
            MakingRoom();
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
    public void MakingRoom()
    {
        GameObject mainMenu = GameObject.Find("Player/CanvasMainMenu/MainMenu");
        mainMenu.SetActive(false);
        //Instantiate(furnitureMenu, transform.position + Vector3.forward*2, furnitureMenu.transform.rotation);
        GameObject.Find("Player/CanvasMainMenu/FurnitureMenu").SetActive(true);

    }


}
