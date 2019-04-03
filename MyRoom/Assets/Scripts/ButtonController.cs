using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
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
        Debug.Log("Awake Button Controller");
        onAButtonClicked();
    }


    public void onAButtonClicked()
    {
        Debug.Log("Clicked A Button");
        transform.GetChild(0).gameObject.SetActive(true);
        //transform.GetChild(1).gameObject.SetActive(false);
        //transform.GetChild(1).gameObject.SetActive(false);
        //transform.GetChild(2).gameObject.SetActive(false);

    }

    public void onBButtonClicked()
    {
        Debug.Log("Clicked B Button");
        transform.GetChild(0).gameObject.SetActive(false);
        //transform.GetChild(1).gameObject.SetActive(true);
    }

    public void onCButtonClicked()
    {
        Debug.Log("Clicked C Button");
        transform.GetChild(0).gameObject.SetActive(false);
        //transform.GetChild(1).gameObject.SetActive(false);
    }
}
