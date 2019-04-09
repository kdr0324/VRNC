//메뉴 관리 스크립트

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;


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
        else if (Input.GetKeyUp(KeyCode.E))
        {
            MakingRoom();
        }
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
    public void MakingRoom()
    {
        //기존에 떠있는 메인메뉴 UI 종료
        GameObject mainMenu = GameObject.Find("Player/CanvasMainMenu/MainMenu");
        mainMenu.SetActive(false);
        //Instantiate(furnitureMenu, transform.position + Vector3.forward*2, furnitureMenu.transform.rotation);

        //방만들기 UI 활성화
        GameObject.Find("Player/CanvasMainMenu/FurnitureMenu").SetActive(true);

    }

    //저장하기
    public void Save()
    {
        //가구를 리스트에 담아서 반환하는 함수 호출
        ObjDataList obj = 
            GameObject.Find("Furniture").GetComponent<FurnitureManager>().FurnitureToList();

        //가구 정보를 담는 리스트 클래스를 JSON 으로 변경
        string jsonData = JsonUtility.ToJson(obj, true);
        //JSON 파일 저장할 경로 설정
        string path = Path.Combine(Application.dataPath, "objData.json");
        //JSON 파일로 저장
        File.WriteAllText(path, jsonData);
    }


    public void Load()
    {
        //JSON 파일 불러올 경로 설정
        string path = Path.Combine(Application.dataPath, "objData.json");
        //JSON 파일 불러옴
        string jsonData = File.ReadAllText(path);

        //읽어온 JSON 파일을 가구 정보를 담는 리스트 객체(ObjDataList)로 변환
        ObjDataList obj = JsonUtility.FromJson<ObjDataList>(jsonData);

        GameObject.Find("Furniture").GetComponent<FurnitureManager>().ListToFurniture(obj);


    }

}

