using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelectManager : MonoBehaviour
{
    public Transform roomParent;
    private Client cli;
    // Start is called before the first frame update
    void Start()
    {
        cli = GameObject.Find("NetworkManager").GetComponent<Client>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RoomMake()
    {
        cli.RoomMake();
    }

    public void Quit()
    {
        string[] rooms;
        rooms = cli.RoomList();

        for(int i=0; i<rooms.Length; i++)
        {
            roomParent.GetChild(i).GetComponentInChildren<Text>().text = rooms[i];
            Button btn = roomParent.GetChild(i).GetComponent<Button>();
            int temp = i;
            btn.onClick.AddListener(() => cli.RoomEnter(temp));
        }
    }
}
