using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueChanged : MonoBehaviour
{
    Transform Player;

    public void Start()
    {
        Player = GameObject.Find("LocalPlayer").transform.GetChild(0);
    }

    public void ValueChangeCheck(VRUISlider slider)
    {

        Debug.Log(slider.value);
        //GameObject.Find("LocalPlayer").transform.GetChild(0).position = Vector3.up * slider.value;

        
        Player.transform.position = Vector3.up * slider.value;
    }
}
