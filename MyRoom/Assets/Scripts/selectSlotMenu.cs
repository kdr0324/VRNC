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
                Button[] buttons;

                Labels = selectSlotmenu.GetComponentsInChildren<Text>();
                buttons = selectSlotmenu.transform.GetComponentsInChildren<Button>();

                for (int i = 0; i < 4; i++)
                {
                    Labels[i].text = timeStamp[i];
                    //만약 TimeStamp를 받아오지 않았다면 클릭되지 않게 수정한다.
                    if(!Labels[i].text.Contains("2"))
                    {
                        buttons[i].interactable = false;
                    }
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
