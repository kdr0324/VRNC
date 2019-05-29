using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//가구 정보를 담는 클래스, JsonUtility 사용을 위해 Seiralizable
[System.Serializable]
public class ObjData
{
    public Vector3 position;
    public Quaternion rotation;
    public string name;
    public int[] textures;
}

//가구 정보(ObjData) 클래스가 담길 리스트 클래스, JsonUtility 사용을 위해 Seiralizable
[System.Serializable]
public class ObjDataList
{
    public int top, wall1, wall2, wall3, wall4, bottom;
    public List<ObjData> objDataList;
}


public struct StructFurniture
{
    public float x1, y1, z1;
    public float x2, y2, z2, w2;
    [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 20)]
    public string name;
}



public class FurnitureManager : MonoBehaviour
{
    public Texture[] FurnitureTextures;
    private Client cli;

    private bool isOnline;
    // Start is called before the first frame update
    void Start()
    {
        //네트워크 
        if (Client.instance == null)
        {

            Debug.Log("NO");
            isOnline = false;

        }
        else
        {

                Debug.Log("YES");
            isOnline = true;
            cli = Client.instance;
            
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (isOnline)
    //    {
    //        lock (cli.sendLockObject)
    //        {

    //            Debug.Log(cli.sendTask.Count);
    //            Byte[] temp = StructToArray();
    //            cli.sendTask.Enqueue(temp);
    //            Debug.Log("Enqueue " + temp.Length );
    //        }


    //        //if (cli.recvTask.Count > 0)
    //        //{

    //        //    byte[] buf = cli.recvTask.Dequeue();
    //        //    Debug.Log("recv Pakcet" + buf.Length);

    //        //    //                StructFurniture temp = fromBytes(buf);
    //        //    //              Debug.Log(temp.name);
    //        //}


    //    }
    //}

    public void Clear()
    {
        Debug.Log("clear_1");
        // 전체 삭제
        isLocalPlayer LocalPlayer = GameObject.Find("LocalPlayer").GetComponent<isLocalPlayer>();
        foreach (Transform child in transform)
        {
            LocalPlayer.CmdDeleteFurniture(child.gameObject);
        }

        ObjDataList obj = new ObjDataList();
        Debug.Log("clear_2");
        GameObject[] wallTexture = new GameObject[6];
        wallTexture[0] = GameObject.Find("Room").transform.Find("Top").gameObject;
        wallTexture[1] = GameObject.Find("Room").transform.Find("Wall1").gameObject;
        wallTexture[2] = GameObject.Find("Room").transform.Find("Wall2").gameObject;
        wallTexture[3] = GameObject.Find("Room").transform.Find("Wall3").gameObject;
        wallTexture[4] = GameObject.Find("Room").transform.Find("Wall4").gameObject;
        wallTexture[5] = GameObject.Find("Room").transform.Find("Bottom").gameObject;

        Debug.Log("clear_3");
        int[] walls = new int[6] { obj.top, obj.wall1, obj.wall2, obj.wall3, obj.wall4, obj.bottom };

        for (int i = 0; i < wallTexture.Length; i++)
        {           
                wallTexture[i].GetComponent<MeshRenderer>().material.mainTexture = FurnitureTextures[walls[0]];
                wallTexture[i].GetComponent<DropObject>().SetMaterial(FurnitureTextures[walls[0]]);
                wallTexture[i].GetComponent<DropObject>().textureName = FurnitureTextures[walls[0]].name;
            
        }
        Debug.Log("clear_4");


    }






    public byte[] StructToArray()
    {
        int childCnt = transform.childCount;
        StructFurniture furniture = new StructFurniture();
        int size = Marshal.SizeOf(furniture);
        byte[] furnitures = new byte[size * transform.childCount];
        byte[] temp;
        
        for (int i = 0; i < childCnt; i++)
        {
            //자식 가구 Transform 받아옴
            Transform cur = transform.GetChild(i);

            //가구 정보 할당
            furniture.x1 = cur.position.x;
            furniture.y1 = cur.position.y;
            furniture.z1 = cur.position.z;
            furniture.x2 = cur.rotation.x;
            furniture.y2 = cur.rotation.y;
            furniture.z2 = cur.rotation.z;
            furniture.w2 = cur.rotation.w;
            furniture.name = cur.name;

            temp = getBytes(furniture);
            Array.Copy(temp, 0, furnitures, i * size, size);
        }

        return furnitures;
    }

    public ObjDataList FurnitureToList()
    {
        //자식 객체 카운트 구함
        int childCnt = transform.childCount;
        int len = FurnitureTextures.Length;

        //가구정보들을 담는 리스트 객체 초기화
        ObjDataList obj = new ObjDataList();

        string[] wallTexture = new  string[6];
        wallTexture[0] = GameObject.Find("Room").transform.Find("Top").GetComponent<DropObject>().textureName;
        wallTexture[1] = GameObject.Find("Room").transform.Find("Wall1").GetComponent<DropObject>().textureName;
        wallTexture[2] = GameObject.Find("Room").transform.Find("Wall2").GetComponent<DropObject>().textureName;
        wallTexture[3] = GameObject.Find("Room").transform.Find("Wall3").GetComponent<DropObject>().textureName;
        wallTexture[4] = GameObject.Find("Room").transform.Find("Wall4").GetComponent<DropObject>().textureName;
        wallTexture[5] = GameObject.Find("Room").transform.Find("Bottom").GetComponent<DropObject>().textureName;

        
        if (wallTexture[0] == "") obj.top = -1;
        if (wallTexture[1] == "") obj.wall1 = -1;
        if (wallTexture[2] == "") obj.wall2 = -1;
        if (wallTexture[3] == "") obj.wall3 = -1;
        if (wallTexture[4] == "") obj.wall4 = -1;
        if (wallTexture[5] == "") obj.bottom= -1;

        for (int k = 0; k < len; k++)
        {
            //Debug.Log(cur.GetChild(j).GetComponent<DropObject>().textureName + ", " + FurnitureTextures[k]);

            if (FurnitureTextures[k].name == wallTexture[0]) obj.top = k;
            if (FurnitureTextures[k].name == wallTexture[1]) obj.wall1  = k;
            if (FurnitureTextures[k].name == wallTexture[2]) obj.wall2  = k;
            if (FurnitureTextures[k].name == wallTexture[3]) obj.wall3  = k;
            if (FurnitureTextures[k].name == wallTexture[4]) obj.wall4  = k;
            if (FurnitureTextures[k].name == wallTexture[5]) obj.bottom = k;

        }
        obj.objDataList = new List<ObjData>();

        //총 가구 개수 만큼 리스트에 추가함
        for (int i = 0; i < childCnt; i++)
        {
            //자식 가구 Transform 받아옴
            Transform cur = transform.GetChild(i);

            //가구 정보 담는 객체 생성
            ObjData objData = new ObjData();
            objData.textures = new int[10];

            //가구 정보 할당
            objData.position = cur.position;
            objData.rotation = cur.rotation;
            objData.name = cur.name;

            
            
            //색 저장
            int cnt = cur.childCount;
            

            if (cnt > 0)
            {
                for (int j = 0; j < cnt; j++)
                {

                    if (cur.GetChild(j).GetComponent<MeshRenderer>() != null)
                    {
                        if (cur.GetChild(j).GetComponent<DropObject>().textureName == "")
                        {
                            objData.textures[j] = -1;
                        }

                        
                        for (int k = 0; k < len; k++)
                        {
                            //Debug.Log(cur.GetChild(j).GetComponent<DropObject>().textureName + ", " + FurnitureTextures[k]);
                            if (FurnitureTextures[k].name == cur.GetChild(j).GetComponent<DropObject>().textureName)
                            {
                                Debug.Log(cur.GetChild(j).GetComponent<DropObject>().textureName + ", " + FurnitureTextures[k].name);
                                objData.textures[j] = k;
                            }

                        }

                        //Debug.Log(cur.GetChild(j).GetComponent<MeshRenderer>().material.mainTexture.name);

                    }
                }
            }
            else
            {
                if (cur.GetComponent<DropObject>() != null)
                {
                    if (cur.GetComponent<DropObject>().textureName == "")
                    {
                        objData.textures[0] = -1;
                    }
                    else
                    {
                        for (int k = 0; k < len; k++)
                        {
                            //Debug.Log(cur.GetChild(j).GetComponent<DropObject>().textureName + ", " + FurnitureTextures[k]);
                            if (FurnitureTextures[k].name == cur.GetComponent<DropObject>().textureName)
                            {
                                Debug.Log(cur.GetComponent<DropObject>().textureName + ", " + FurnitureTextures[k].name);
                                objData.textures[0] = k;
                            }

                        }
                    }
                }
            }

            //리스트에 가구 정보 추가
            obj.objDataList.Add(objData);


        }
        return obj;
    }

    public void ListToFurniture(ObjDataList obj)
    {
        //가구 생성에 필요한 스크립트 호출
        isLocalPlayer LocalPlayer = GameObject.Find("LocalPlayer").GetComponent<isLocalPlayer>();


        Debug.Log(obj.objDataList.Count);
        GameObject[] wallTexture = new GameObject[6];
        wallTexture[0] = GameObject.Find("Room").transform.Find("Top").gameObject;
        wallTexture[1] = GameObject.Find("Room").transform.Find("Wall1").gameObject;
        wallTexture[2] = GameObject.Find("Room").transform.Find("Wall2").gameObject;
        wallTexture[3] = GameObject.Find("Room").transform.Find("Wall3").gameObject;
        wallTexture[4] = GameObject.Find("Room").transform.Find("Wall4").gameObject;
        wallTexture[5] = GameObject.Find("Room").transform.Find("Bottom").gameObject;


        int[] walls = new int[6] {obj.top, obj.wall1, obj.wall2, obj.wall3, obj.wall4, obj.bottom};

        for (int i = 0; i < wallTexture.Length; i++)
        {
            if (walls[i] != -1)
            {
                wallTexture[i].GetComponent<MeshRenderer>().material.mainTexture = FurnitureTextures[walls[i]];
                wallTexture[i].GetComponent<DropObject>().SetMaterial(FurnitureTextures[walls[i]]);
                wallTexture[i].GetComponent<DropObject>().textureName = FurnitureTextures[walls[i]].name;
            }
        }
        

        //int furnitureLen = transform.childCount;
        //for(int i=0; i<furnitureLen; i++)
        //{
        //    Destroy(transform.GetChild(0).gameObject);
        //}

        //리스트의 길이 만큼 반복
        for (int i = 0; i < obj.objDataList.Count; i++)
        {
            //가구 이름 받아옴
            string name = obj.objDataList[i].name;

            
            //이름에 맞는 가구 Prefab Load
            GameObject NewGameObject = Resources.Load("Prefabs/" + name) as GameObject;
            //Load 된 Prefab을 가구 정보에 맞게 생성
            GameObject newObj = Instantiate(NewGameObject, obj.objDataList[i].position, obj.objDataList[i].rotation);
            newObj.name = name;
            int cnt = newObj.transform.childCount;

            if (cnt == 0)
            {
                if (obj.objDataList[i].textures[0] == -1) break;

                newObj.GetComponent<MeshRenderer>().material.mainTexture = FurnitureTextures[obj.objDataList[i].textures[0]];
                newObj.GetComponent<DropObject>().SetMaterial(FurnitureTextures[obj.objDataList[i].textures[0]]);
                newObj.GetComponent<DropObject>().textureName = FurnitureTextures[obj.objDataList[i].textures[0]].name;

            }
            else
            {
                for (int j = 0; j < cnt; j++)
                {
                    if (newObj.transform.GetChild(j).GetComponent<MeshRenderer>() != null)
                    {
                        if (obj.objDataList[i].textures[j] == -1)
                        {
                            continue;
                        }
                        newObj.transform.GetChild(j).GetComponent<MeshRenderer>().material.mainTexture = FurnitureTextures[obj.objDataList[i].textures[j]];
                        newObj.transform.GetChild(j).GetComponent<DropObject>().SetMaterial(FurnitureTextures[obj.objDataList[i].textures[j]]);
                        newObj.transform.GetChild(j).GetComponent<DropObject>().textureName = FurnitureTextures[obj.objDataList[i].textures[j]].name;
                        //Debug.Log(newObj.transform.GetChild(j).GetComponent<DropObject>().textureName);
                    }

                }
            }
        }
    }

    byte[] getBytes(StructFurniture str)
    {
        int size = Marshal.SizeOf(str);
        byte[] arr = new byte[size];

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(str, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }


    StructFurniture fromBytes(byte[] arr)
    {
        StructFurniture str = new StructFurniture();

        int size = Marshal.SizeOf(str);
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.Copy(arr, 0, ptr, size);

        str = (StructFurniture)Marshal.PtrToStructure(ptr, str.GetType());
        Marshal.FreeHGlobal(ptr);

        return str;
    }
}
