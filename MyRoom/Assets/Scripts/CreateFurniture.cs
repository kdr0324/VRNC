using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateFurniture : MonoBehaviour
{
    //public Transform Furniture;
    private List<GameObject> list;
    public AudioClip FurnitureSound;
    
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
        //GetComponentInParent<AudioSource>().clip = GetComponentInParent<AudioSource>().clip;
        GetComponentInParent<AudioSource>().Play();

        string current_name = EventSystem.current.currentSelectedGameObject.name;

        //Debug.Log(transform.root.name);
        transform.root.GetComponent<isLocalPlayer>().
            SpawnObject(current_name, transform.position + new Vector3(0, 0.5f, 0.5f));
       // GameObject NewGameObject = Resources.Load("Prefabs/" + current_name) as GameObject;


        //GameObject obj = Instantiate(NewGameObject, transform.position + new Vector3(0, 0.5f, 0.5f), NewGameObject.transform.rotation);
        //obj.name = name;

    }

}
