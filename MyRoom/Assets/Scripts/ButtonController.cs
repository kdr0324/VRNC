using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip ButtonSound;
    public GameObject FurnitureMenu;
    public GameObject TextureMenu;
    public GameObject Delete;

    private void Awake()
    {
        audio = GameObject.Find("Room").GetComponent<AudioSource>();
        onAButtonClicked();
    }


    public void onAButtonClicked()
    {
        audio.clip = ButtonSound;
        audio.Play();

        FurnitureMenu.SetActive(true);
        TextureMenu.SetActive(false);
        Delete.SetActive(false);
    }

    public void onBButtonClicked()
    {
        audio.clip = ButtonSound;
        audio.Play();

        FurnitureMenu.SetActive(false);
        TextureMenu.SetActive(true);
        Delete.SetActive(false);

    }

    public void onCButtonClicked()
    {
        audio.clip = ButtonSound;
        audio.Play();

        FurnitureMenu.SetActive(false);
        TextureMenu.SetActive(false);
        Delete.SetActive(true);
    }
}
