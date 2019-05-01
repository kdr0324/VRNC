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
            objData.textures = new int[10];

            //가구 정보 할당
            objData.position = cur.position;
            objData.rotation = cur.rotation;
            objData.name = cur.name;

            
            
            //색 저장
            int cnt = cur.childCount;
            for(int j=0; j<cnt; j++)
            {

                if (cur.GetChild(j).GetComponent<MeshRenderer>() != null)
                {
                    if (cur.GetChild(j).GetComponent<DropObject>().textureName == "")
                    {
                        objData.textures[j] = -1;
                    }

                    int len = FurnitureTextures.Length;
                    for(int k =0; k<len; k++)
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
            int cnt = newObj.transform.childCount;

            if(cnt == 0)
            {
                newObj.GetComponent<MeshRenderer>().material.mainTexture = FurnitureTextures[obj.objDataList[i].textures[0]];
                newObj.GetComponent<DropObject>().SetMaterial(FurnitureTextures[obj.objDataList[i].textures[0]]);
            }
            for(int j=0; j<cnt; j++)
            {
                if (newObj.transform.GetChild(j).GetComponent<MeshRenderer>() != null)
                {
                    if(obj.objDataList[i].textures[j] == -1)
                    {
                        continue;
                    }
                    newObj.transform.GetChild(j).GetComponent<MeshRenderer>().material.mainTexture = FurnitureTextures[obj.objDataList[i].textures[j]];
                    newObj.transform.GetChild(j).GetComponent<DropObject>().SetMaterial(FurnitureTextures[obj.objDataList[i].textures[j]]);
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
