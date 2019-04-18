using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

//public class CreateFurniture : MonoBehaviour
public class CreateFurniture : NetworkBehaviour
{
    public GameObject NewGameObject;
    //public Transform Furniture;
    private List<GameObject> list;


    // Start is called before the first frame update
    void Start()
    {
        list = NetworkManager.singleton.spawnPrefabs;

    }

    // Update is called once per frame
    void Update()
    {

    }

    [ClientRpc]
    public void RpcFurnitureCreate()
    {
        
        string current_name = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("Prefabs/" + current_name);
        //NewGameObject = Resources.Load("Prefabs/" + current_name) as GameObject;
        foreach (GameObject target in list)
        {
            if(target.name == current_name)
            {
                NewGameObject = target;
            }

        }
        Debug.Log(NewGameObject.name);

        GameObject obj = Instantiate(NewGameObject, transform.position + new Vector3(0, 0.5f, 0.5f), NewGameObject.transform.rotation);
        obj.name = current_name;

        
        NetworkServer.Spawn(obj);
    }

    //public void SetParent(GameObject newParent)
    //{

    //    NewGameObject.transform.parent = newParent.transform;
    //}
}
