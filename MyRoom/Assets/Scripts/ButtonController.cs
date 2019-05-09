using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject FurnitureMenu;
    public GameObject TextureMenu;
    public GameObject Delete;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        onAButtonClicked();
    }


    public void onAButtonClicked()
    {
        FurnitureMenu.SetActive(true);
        TextureMenu.SetActive(false);
        Delete.SetActive(false);
    }

    public void onBButtonClicked()
    {
        FurnitureMenu.SetActive(false);
        TextureMenu.SetActive(true);
        Delete.SetActive(false);

    }

    public void onCButtonClicked()
    {
        FurnitureMenu.SetActive(false);
        TextureMenu.SetActive(false);
        Delete.SetActive(true);
    }
}
