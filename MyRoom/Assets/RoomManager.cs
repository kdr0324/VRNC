using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject furnitureMenu;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Room manager Start");
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
            RoomDecorate();
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
    public void RoomDecorate()
    {
        GameObject mainMenu = GameObject.Find("[CameraRig]/Canvas/MainMenu");
        mainMenu.SetActive(!mainMenu.active);
        //Instantiate(furnitureMenu, transform.position + Vector3.forward*2, furnitureMenu.transform.rotation);
        GameObject.Find("[CameraRig]/Canvas/Scroll View").SetActive(true);

    }


}
