using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class isLocalPlayer : NetworkBehaviour
{
    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            Debug.Log("this is server");
            
        }
        if (isLocalPlayer)
        {
            name = "LocalPlayer";

            if(isClient)
            {
                Transform room = GameObject.Find("Room").transform;
                for (int i = 0; i < room.childCount; i++)
                {
                    room.GetChild(i).GetComponent<setTexture>().EnterFriendRoom();
                }
            }
        }

        if (!isLocalPlayer)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            //transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (isServer)
    //    {
    //        Debug.Log("this is server");
    //    }
    //}

    public void SpawnObject(string name, Vector3 pos)
    {
        CmdSpawn(name,  pos);
    }

    public void SpawnObject(string name, Vector3 pos, int[] textures)
    {
        CmdSpawnWithTextures(name, pos, textures);
    }

    public void SpawnObjectRot(string name, Vector3 pos, Quaternion rot ,int[] textures)
    {
        CmdSpawnWithTexturesRot(name, pos, rot, textures);
    }

    [Command]
    void CmdSpawn(string name, Vector3 pos)
    {
        
        
        GameObject NewGameObject = Resources.Load("Prefabs/" + name) as GameObject;


        GameObject obj = Instantiate(NewGameObject, pos, NewGameObject.transform.rotation);
        obj.name = name;

        //NetworkServer.SpawnWithClientAuthority(obj, connectionToClient);
        NetworkServer.Spawn(obj);
        //NetworkServer.SpawnWithClientAuthority
    }

    [Command]
    void CmdSpawnWithTextures(string name, Vector3 pos, int[] textures)
    {


        GameObject NewGameObject = Resources.Load("Prefabs/" + name) as GameObject;


        GameObject obj = Instantiate(NewGameObject, pos, NewGameObject.transform.rotation);
        obj.name = name;

        NetworkServer.Spawn(obj);

        //자식 객체가 없는 경우
        obj.GetComponent<setParent>().loadTextures = textures;
    }

    [Command]
    void CmdSpawnWithTexturesRot(string name, Vector3 pos, Quaternion rot, int[] textures)
    {


        GameObject NewGameObject = Resources.Load("Prefabs/" + name) as GameObject;


        GameObject obj = Instantiate(NewGameObject, pos, rot);
        obj.name = name;

        NetworkServer.Spawn(obj);

        //자식 객체가 없는 경우
        obj.GetComponent<setParent>().loadTextures = textures;
    }

    [Command]
    public void CmdClientAuthority(GameObject obj)
    {
        //if(!hasAuthority)
        //    obj.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
        NetworkServer.objects[obj.GetComponent<NetworkIdentity>().netId].AssignClientAuthority(connectionToClient);
    }

    [Command]
    public void CmdServerAuthority(GameObject obj)
    {
        //if(hasAuthority)
        //    obj.GetComponent<NetworkIdentity>().RemoveClientAuthority(connectionToServer);

        
        NetworkServer.objects[obj.GetComponent<NetworkIdentity>().netId].RemoveClientAuthority(connectionToClient);
    }

    [Command]
    public void CmdDeleteFurniture(GameObject obj)
    {
        NetworkServer.Destroy(obj);
    }

    [Command]
    public void CmdSetSyncListInt(GameObject obj, int idx, int num)
    {
        Debug.Log("CmdSet Int : " + idx);
        obj.GetComponent<setParent>().syncListTexture[idx] = num;
    }

    [Command]
    public void CmdSetSyncWallTexture(GameObject obj, int idx, int num)
    {
        Debug.Log("CmdSet Int : " + idx);
        obj.GetComponent<setTexture>().syncListTexture[idx] = num;
    }
}
