using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomSelectManager : MonoBehaviour
{
    public Transform roomParent;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        RoomList();
    }

    public void RoomMake()
    {
        Client.instance.RoomMake();

        // Loads the second Scene
        SceneManager.LoadScene("Main");

        //GameObject.Find("NetworkManager").GetComponent<ServerController>().RunHost();
    }

    public void RoomList()
    {
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
                btn.onClick.AddListener(() => SceneManager.LoadScene("Main"));
                GameObject.Find("NetworkManager").GetComponent<ClientController>().RunClient();
            }

        }
    }
}
