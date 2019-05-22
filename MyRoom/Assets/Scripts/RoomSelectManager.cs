using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomSelectManager : MonoBehaviour
{
    public AudioClip ButtonSound;

    public Transform roomParent;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        //RoomList();
    }


    public void RoomMake()
    {
        Client.instance.RoomMake();
        GetComponent<AudioSource>().clip = ButtonSound;
        GetComponent<AudioSource>().Play();

        // Loads the second Scene
        SceneManager.LoadScene("Main");

        //GameObject.Find("NetworkManager").GetComponent<ServerController>().RunHost();
    }

    public void RoomList()
    {
        GetComponent<AudioSource>().clip = ButtonSound;
        GetComponent<AudioSource>().Play();

        if (Client.instance.isConnect)
        {
            string[] rooms;
            rooms = Client.instance.RoomList();

            for (int i = 0; i < rooms.Length; i++)
            {
                roomParent.GetChild(i).GetComponentInChildren<Text>().text = rooms[i];
                Button btn = roomParent.GetChild(i).GetComponent<Button>();
                int temp = i;
                btn.onClick.AddListener(() => Client.instance.RoomEnter(temp));
                btn.onClick.AddListener(() => GetComponent<AudioSource>().clip = ButtonSound);
                btn.onClick.AddListener(() => GetComponent<AudioSource>().Play());
                btn.onClick.AddListener(() => SceneManager.LoadScene("Main"));
                
                //GameObject.Find("NetworkManager").GetComponent<ClientController>().RunClient();
            }

        }
    }
}
