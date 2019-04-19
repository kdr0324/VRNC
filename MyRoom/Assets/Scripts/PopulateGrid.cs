
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour
{
    public GameObject prefab; // This is our prefab object that will be exposed in the inspector

    public Sprite[] sprites;

    //public int numberToCreate; // number of objects to create. Exposed in inspector

    void Start()
    {
        Populate();
    }

    void Update()
    {

    }

    void Populate()
    {
        GameObject newObj; // Create GameObject instance

        for (int i = 0; i < sprites.Length; i++)
        {
            newObj = (GameObject)Instantiate(prefab, transform);
            newObj.GetComponent<Image>().sprite = sprites[i];
            newObj.GetComponent<FunitureSelect>().name = sprites[i].name;
        }


        //newObj = (GameObject)Instantiate(prefab, transform);
        //newObj.GetComponent<Image>().sprite = sprites[0];

        //newObj = (GameObject)Instantiate(prefab, transform);
        //newObj.GetComponent<Image>().sprite = sprites[1];

        //for (int i = 0; i < numberToCreate; i++)
        //{
        //    // Create new instances of our prefab until we've created as many as we specified
        //    newObj = (GameObject)Instantiate(prefab, transform);
        //    //newObj.GetComponent<Image>().sprite.;
        //    // Randomize the color of our image
        //    //   newObj.GetComponent<Image>().color = Random.ColorHSV();
        //}

    }
}