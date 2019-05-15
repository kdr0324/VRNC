﻿//메뉴 관리 스크립트

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;


public class MenuManager : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip clickSound;

   
    public GameObject mainMenu;
    public GameObject furnitureMenu;
    public GameObject settingsMenu;
    public GameObject selectTextureMenu;

    public GameObject Furniture;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Menu manager Start");
        audio = gameObject.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            byte[] bytes = I360Render.Capture(1024, true);
            if (bytes != null)
            {   // 파일 저장 경로 추후에 변경필요
                string path = Path.Combine(Application.dataPath, "360render.png");
                File.WriteAllBytes(path, bytes);
                Debug.Log("360 render saved to " + path);
            }
        }
        //else if (Input.GetKeyUp(KeyCode.E))
        //{
        //    MakingRoom();
        //}
        //else if (Input.GetKeyUp(KeyCode.R))
        //{
        //    Settings();
        //}
        //else if (Input.GetKeyDown(KeyCode.Backspace))
        //{
        //    Back();
        //}
        //else if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Esc();
        //}
    }
   
    
    //프로그램 종료
    public void Quit()
    {
        Debug.Log("ClickedQuit");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		        Application.Quit();
#endif
    }

    //방만들기 UI 활성화 시켜줌
    public void CallFurnitureMenu()
    {
        audio.Play();
    
        //기존에 떠있는 메인메뉴 UI 종료
        //GameObject mainMenu = transform.parent.Find("MainMenu").gameObject;

        mainMenu.SetActive(false);
        //Instantiate(furnitureMenu, transform.position + Vector3.forward*2, furnitureMenu.transform.rotation);

        //방만들기 UI 활성화
        //transform.parent.Find("FurnitureMenu").gameObject.SetActive(true);
        furnitureMenu.SetActive(true);

    }
    // 설정하기
    public void CallSettingsMenu()
    {
        
        //GameObject mainMenu = transform.parent.Find("MainMenu").gameObject;
        //GameObject mainMenu = GameObject.Find("Player/CanvasMainMenu/MainMenu");
        mainMenu.SetActive(false);
        //Instantiate(furnitureMenu, transform.position + Vector3.forward*2, furnitureMenu.transform.rotation);
        //transform.parent.Find("SettingsMenu").gameObject.SetActive(true);
        settingsMenu.SetActive(true);

    }


    //저장하기
    public void Save()
    {
        //가구를 리스트에 담아서 반환하는 함수 호출
        ObjDataList obj =
            Furniture.GetComponent<FurnitureManager>().FurnitureToList();

        //가구 정보를 담는 리스트 클래스를 JSON 으로 변경
        string jsonData = JsonUtility.ToJson(obj, true);
        //JSON 파일 저장할 경로 설정
        //string path = Path.Combine(Application.dataPath, "objData.json");
        ////JSON 파일로 저장
        //File.WriteAllText(path, jsonData);
        
        if(Client.instance != null)
        {
            //방장
            //if(Client.instance.isOwner)
            if(true)
            {
                Client.instance.Save(jsonData);
            }
        }
        else
        {
            string path = Path.Combine(Application.dataPath, "objData.json");
            //JSON 파일로 저장
            File.WriteAllText(path, jsonData);
        }
    }
    //뒤로가기
    public void Back()
    {
        furnitureMenu.SetActive(false);
        settingsMenu.SetActive(false);
        //selectTextureMenu.SetActive(false);
        mainMenu.SetActive(true);
        //transform.parent.Find("FurnitureMenu").gameObject.SetActive(false);
        //transform.parent.Find("SettingsMenu").gameObject.SetActive(false);
        //transform.parent.Find("MainMenu").gameObject.SetActive(true);
    }

    public void Cancel()
    {
        mainMenu.SetActive(false);
    }
    //메뉴창 키기
    public void CallMainMenu()
    {
        if(!mainMenu.activeSelf)
        {
            mainMenu.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(false);
        }
    }

    public void Load()
    {



        if (Client.instance != null)
        {
            //방장
            if(Client.instance.isOwner)
            {
                string jsonData = Client.instance.Load();

                //읽어온 JSON 파일을 가구 정보를 담는 리스트 객체(ObjDataList)로 변환
                ObjDataList obj = JsonUtility.FromJson<ObjDataList>(jsonData);

                Furniture.GetComponent<FurnitureManager>().ListToFurniture(obj);
            }
        }
        else
        {
            //JSON 파일 불러올 경로 설정
            string path = Path.Combine(Application.dataPath, "objData.json");
            //JSON 파일 불러옴
            string jsonData = File.ReadAllText(path);

            //읽어온 JSON 파일을 가구 정보를 담는 리스트 객체(ObjDataList)로 변환
            ObjDataList obj = JsonUtility.FromJson<ObjDataList>(jsonData);

            Furniture.GetComponent<FurnitureManager>().ListToFurniture(obj);
        }

    }

}

