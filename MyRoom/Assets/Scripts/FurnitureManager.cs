using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


public class FurnitureManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ObjDataList FurnitureToList()
    {
        //자식 객체 카운트 구함
        int childCnt = transform.childCount;

        //가구정보들을 담는 리스트 객체 초기화
        ObjDataList obj = new ObjDataList();
        obj.objDataList = new List<ObjData>();

        //총 가구 개수 만큼 리스트에 추가함
        for (int i = 0; i < childCnt; i++)
        {
            //자식 가구 Transform 받아옴
            Transform cur = transform.GetChild(i);

            //가구 정보 담는 객체 생성
            ObjData objData = new ObjData();
            //가구 정보 할당
            objData.position = cur.position;
            objData.rotation = cur.rotation;
            objData.name = cur.name;

            //리스트에 가구 정보 추가
            obj.objDataList.Add(objData);
        }
        return obj;
    }

    public void ListToFurniture(ObjDataList obj)
    {
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
