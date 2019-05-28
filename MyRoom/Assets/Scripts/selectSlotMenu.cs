using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class selectSlotMenu : MonoBehaviour
{
    public GameObject selectSlotmenu;


    // Start is called before the first frame update
    void Start()
    {
        //timestamp();
    }



    public void timestamp()
    {
        Debug.Log("select 1");
        if (Client.instance != null)
        {
            Debug.Log("select 2");
            //방장
            if (Client.instance.isConnect)
            {
                string[] timeStamp = Client.instance.Label_Load();
                Text[] Labels;

                Labels = selectSlotmenu.GetComponentsInChildren<Text>();

                for (int i = 0; i < 4; i++)
                {
                    Labels[i].text = timeStamp[i];
                }
            }
        }
    }


    public void loadSlot(int idx)
    {
        Client.instance.isOwner = true;
        Client.instance.roomType = idx;

        Client.instance.RoomMake();
        SceneManager.LoadScene("Main");

    }

}
