using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateFurniture : MonoBehaviour
{
    public GameObject NewGameObject;
    //public Transform Furniture;
    


    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void FurnitureCreate()
    {

        string current_name = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("Prefabs/" + current_name);
        NewGameObject = Resources.Load("Prefabs/" + current_name) as GameObject;
        Debug.Log(NewGameObject.name);


        GameObject obj = Instantiate(NewGameObject, transform.position + Vector3.forward, NewGameObject.transform.rotation);
        obj.name = current_name;
       
        



    }

    //public void SetParent(GameObject newParent)
    //{

    //    NewGameObject.transform.parent = newParent.transform;
    //}
}
