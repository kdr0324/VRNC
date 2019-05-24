using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class setParent : NetworkBehaviour
{
    private Texture[] textures;
    private int childCnt = 0;

    public SyncListInt syncListTexture = new SyncListInt();
    


    // Start is called before the first frame update
    void Start()
    {
        childCnt = transform.childCount;
        transform.SetParent(GameObject.Find("Furniture").transform, true);


        if (name.Contains("Clone"))
        {
            string[] split = name.Split('(');
            name = split[0];
        }
        textures = GameObject.Find("Furniture").GetComponent<FurnitureManager>().FurnitureTextures;


        if (isServer)
        {
            //자식 없는 경우
            if (childCnt == 0)
                syncListTexture.Add(-1);
            //자식 있는 경우
            else
            {
                for (int i = 0; i < childCnt; i++)
                {
                    syncListTexture.Add(-1);
                }
            }
        }
        else
        {
            if (childCnt == 0)
            {
                SetTexture(gameObject, syncListTexture[0]);
            }
            else
            {
                for (int i = 0; i < syncListTexture.Count; i++)
                {
                    SetTexture(transform.GetChild(i).gameObject, syncListTexture[i]);
                }
            }
            
        }
        syncListTexture.Callback += MyCallBack;
        
    }

    void Update()
    {
        if (transform.position.y < -15)
        {
            GameObject.Find("LocalPlayer").GetComponent<isLocalPlayer>().CmdDeleteFurniture(gameObject);
        }
    }

    private void MyCallBack(SyncListInt.Operation op, int index)
    {
        int num = syncListTexture[index];

        if (num == -1) return;

        if(childCnt == 0)
        {
            SetTexture(gameObject, num);
        }
        else
        {
            GameObject target = transform.GetChild(index).gameObject;
            SetTexture(target, num);
        }

    }


    public void SetSyncListInt(int idx, int num)
    {
        Debug.Log("Set Int : " + idx);
        
        GameObject.Find("LocalPlayer").GetComponent<isLocalPlayer>().CmdSetSyncListInt(gameObject, idx, num);
        
        //syncListTexture[idx] = num;
    }



    public int getTextureNum(string textureName)
    {
        for(int i=0; i<textures.Length; i++)
        {
            if (string.Compare(textureName, textures[i].name) == 0)
            {
                return i;
            }
        }
        return -1;
    }

    private void SetTexture(GameObject target, int num)
    {
        if (num == -1) return;
        Debug.Log("Set Texture : " + num);
        target.GetComponent<MeshRenderer>().material.mainTexture = textures[num];
        target.GetComponent<DropObject>().SetMaterial(textures[num]);
        target.GetComponent<DropObject>().textureName = textures[num].name;
    }


   
}
