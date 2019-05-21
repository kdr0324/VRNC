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
            Debug.Log("this is local player");
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

    [Command]
    void CmdSpawn(string name, Vector3 pos)
    {
        
        
        GameObject NewGameObject = Resources.Load("Prefabs/" + name) as GameObject;


        GameObject obj = Instantiate(NewGameObject, pos, NewGameObject.transform.rotation);
        obj.name = name;

        NetworkServer.SpawnWithClientAuthority(obj, connectionToClient);
        //NetworkServer.SpawnWithClientAuthority
    }
}
