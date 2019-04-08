using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setParent : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(GameObject.Find("Furniture").transform, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
