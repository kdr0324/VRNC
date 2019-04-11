using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTexture : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SelectTextureColor();

    }

    public void SelectTextureColor()

    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("MouseDown");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
                //if(hit.collider.name == "cabinet_1")
                //{
                Transform SelectMenu = GameObject.Find("Player/CanvasMainMenu/SelectTexture").transform;
                Debug.Log(SelectMenu.name);
                SelectMenu.gameObject.SetActive(true);
                //SelectMenu.position = hit.transform.position + Vector3.back;
                //} 


                SelectMenu.position = hit.point + (hit.transform.position - transform.position).normalized * 1.0f; // add a little distance to avoid z-fighting
                //SelectMenu.rotation = Quaternion.LookRotation(hit.point, hit.transform.up);

            }
        }
    }


}
