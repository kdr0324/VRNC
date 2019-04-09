//메뉴 관리 스크립트

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;



//가구 정보를 담는 클래스, JsonUtility 사용을 위해 Seiralizable
[System.Serializable]
public class ObjData
{
    public Vector3 position;
    public Quaternion rotation;
    public string name;
}

//가구 정보(ObjData) 클래스가 담길 리스트 클래스, JsonUtility 사용을 위해 Seiralizable
[System.Serializable]
public class ObjDataList
{
    public List<ObjData> objDataList;
}


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
        //Furniture 객체를 찾음 (추가되는 가구들의 부모객체)
        GameObject furniture = GameObject.Find("Furniture");
        //자식 객체 카운트 구함
        int childCnt = furniture.transform.childCount;

        //가구정보들을 담는 리스트 객체 초기화
        ObjDataList obj = new ObjDataList();
        obj.objDataList = new List<ObjData>();

        //총 가구 개수 만큼 리스트에 추가함
        for (int i = 0; i < childCnt; i++)
        {
            //자식 가구 Transform 받아옴
            Transform cur = furniture.transform.GetChild(i);

            //가구 정보 담는 객체 생성
            ObjData objData = new ObjData();
            //가구 정보 할당
            objData.position = cur.position;
            objData.rotation = cur.rotation;
            objData.name = cur.name;

            //리스트에 가구 정보 추가
            obj.objDataList.Add(objData);
        }

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
        Debug.Log(obj.objDataList.Count);

        //리스트의 길이 만큼 반복
        for (int i = 0; i < obj.objDataList.Count; i++)
        {
            //가구 이름 받아옴
            string name = obj.objDataList[i].name;
            //이름에 맞는 가구 Prefab Load
            GameObject NewGameObject = Resources.Load("Prefabs/" + name) as GameObject;
            //Load 된 Prefab을 가구 정보에 맞게 생성
            GameObject newObj = Instantiate(NewGameObject, obj.objDataList[i].position, obj.objDataList[i].rotation);
            //가구 이름도 설정
            newObj.name = obj.objDataList[i].name;
        }

    }

}

