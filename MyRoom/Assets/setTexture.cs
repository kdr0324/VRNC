using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class setTexture : NetworkBehaviour
{
    private Texture[] textures;
    private int childCnt = 0;

    public SyncListInt syncListTexture = new SyncListInt();

    // Start is called before the first frame update
    void Start()
    {

        textures = GameObject.Find("Furniture").GetComponent<FurnitureManager>().FurnitureTextures;


        if (isServer)
        {
            syncListTexture.Add(-1);
        }
        else
        {
            SetTexture(gameObject, syncListTexture[0]);
        }

        syncListTexture.Callback += MyCallBack;

    }

    private void MyCallBack(SyncListInt.Operation op, int index)
    {
        int num = syncListTexture[index];

        if (num == -1) return;

        SetTexture(gameObject, num);

    }


    public void SetSyncWallTexture(int idx, int num)
    {
        Debug.Log("Set Wall Texture : " + num);

        GameObject.Find("LocalPlayer").GetComponent<isLocalPlayer>().CmdSetSyncWallTexture(gameObject, idx, num);

        //syncListTexture[idx] = num;
    }



    public int getTextureNum(string textureName)
    {
        for (int i = 0; i < textures.Length; i++)
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
