using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;



[System.Serializable]
public class ObjData
{
    public Vector3 position;
    public Quaternion rotation;
    public string name;
}

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


    public void Save()
    {
        GameObject furniture = GameObject.Find("Furniture");
        int childCnt = furniture.transform.childCount;


        ObjDataList obj = new ObjDataList();
        obj.objDataList = new List<ObjData>();
        

        for(int i=0; i<childCnt; i++)
        {
            Transform cur = furniture.transform.GetChild(i);
            ObjData objData = new ObjData();
            objData.position = cur.position;
            objData.rotation = cur.rotation;
            objData.name = cur.name;

            obj.objDataList.Add(objData);
        }

        string jsonData = JsonUtility.ToJson(obj, true);
        string path = Path.Combine(Application.dataPath, "objData.json");
        File.WriteAllText(path, jsonData);
    }


    public void Load()
    {
        string path = Path.Combine(Application.dataPath, "objData.json");
        string jsonData = File.ReadAllText(path);
        ObjDataList obj = JsonUtility.FromJson<ObjDataList>(jsonData);
        Debug.Log(obj.objDataList.Count);

        for(int i=0; i < obj.objDataList.Count; i++)
        {
            string name = obj.objDataList[i].name;
            GameObject NewGameObject = Resources.Load("Prefabs/" + name) as GameObject;
            GameObject newObj = Instantiate(NewGameObject, obj.objDataList[i].position, obj.objDataList[i].rotation);
            newObj.name = obj.objDataList[i].name;
        }

    }

}
