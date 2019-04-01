using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateFurniture : MonoBehaviour
{
    public GameObject Archery;
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
        Debug.Log(current_name);
    }
}
