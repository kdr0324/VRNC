//메뉴 관리 스크립트

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Voice.Unity;
using Photon.Voice.Unity.UtilityScripts;
using HTC.UnityPlugin.Vive;

public class MenuManager : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip ButtonSound;
    public AudioClip SaveSound;
    public AudioClip LoadSound;
    public AudioClip ScreenshotSound;

    public GameObject mainMenu;
    public GameObject furnitureMenu;
    public GameObject settingsMenu;
    public GameObject selectTextureMenu;

    public GameObject Furniture;
    public GameObject saveSlotMenu;
    public GameObject loadSlotMenu;
    public GameObject firendSlotMenu;
    public Transform roomList;
    public GameObject screenshotWindow;

    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log("Menu manager Start");
        audio = GameObject.Find("Room").GetComponent<AudioSource>();
        Furniture = GameObject.Find("Furniture");

        if (Client.instance != null)
        {
            if (Client.instance.isConnect && Client.instance.isOwner)
            {
                if (Client.instance.roomType != 0)
                {
                    Client.instance.curRoom = Client.instance.UserID;
                    loadSlot(Client.instance.roomType);
                }
            }
        }
    }

    void Update()
    {
        //if(ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger))
        //{
        //    Debug.Log("트리거 눌렀지롱");
        //    ViveInput.TriggerHapticPulse(HandRole.RightHand);
        //}
    }

    //프로그램 종료
    public void Quit()
    {
        audio.clip = ButtonSound;
        audio.Play();

        Debug.Log("ClickedQuit");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		        Application.Quit();
#endif
    }

    //방만들기 UI 활성화 시켜줌
    public void CallFurnitureMenu()
    {
        audio.clip = ButtonSound;
        audio.Play();



        //기존에 떠있는 메인메뉴 UI 종료
        //GameObject mainMenu = transform.parent.Find("MainMenu").gameObject;

        mainMenu.SetActive(false);
        //Instantiate(furnitureMenu, transform.position + Vector3.forward*2, furnitureMenu.transform.rotation);

        //방만들기 UI 활성화
        //transform.parent.Find("FurnitureMenu").gameObject.SetActive(true);
        furnitureMenu.SetActive(true);

    }
    //슬롯 text 자동갱신
    public void timestamp(bool chk)
    {
        Text[] Labels;
        Button[] buttons;

        if (Client.instance != null && Client.instance.isConnect)
        {
            //연결이 되어있는 경우

            //방장이면 저장, 불러오기
            //아니면 저장만
            string[] timeStamp = Client.instance.Label_Load();

            if (chk)
            {
                Labels = saveSlotMenu.GetComponentsInChildren<Text>();
                buttons = saveSlotMenu.transform.GetComponentsInChildren<Button>();
            }
            else
            {
                Labels = loadSlotMenu.GetComponentsInChildren<Text>();
                buttons = loadSlotMenu.transform.GetComponentsInChildren<Button>();
            }

            for (int i = 0; i < 4; i++)
            {
                //
                buttons[i].interactable = true;
                Labels[i].text = timeStamp[i];

                //방장이 아니고 불러오기이면
                if (!Client.instance.isOwner && !chk)
                {
                    Labels[i].text = "호스트만 가능합니다.";
                    buttons[i].interactable = false;
                }
                //라벨이 비어있으면
                else if (!Labels[i].text.Contains("2") && !chk)
                {
                    Labels[i].text = "비어 있음";
                    buttons[i].interactable = false;
                }
            }

        }
        //바로 Main Scene부터 시작 한 경우
        else
        {
            if (chk)
            {
                Labels = saveSlotMenu.GetComponentsInChildren<Text>();
                buttons = saveSlotMenu.transform.GetComponentsInChildren<Button>();
            }
            else
            {
                Labels = loadSlotMenu.GetComponentsInChildren<Text>();
                buttons = loadSlotMenu.transform.GetComponentsInChildren<Button>();
            }

            for (int i = 0; i < 4; i++)
            {

                Labels[i].text = "서버 연결 안 됨";
                buttons[i].interactable = false;
            }
        }
    }

    //저장하기
    public void Save()
    {
        mainMenu.SetActive(false);
        saveSlotMenu.SetActive(true);

        audio.clip = ButtonSound;
        audio.Play();

        timestamp(true);
    }

    public void saveSlot(int idx)
    {

        //가구를 리스트에 담아서 반환하는 함수 호출
        ObjDataList obj =
            Furniture.GetComponent<FurnitureManager>().FurnitureToList();

        //가구 정보를 담는 리스트 클래스를 JSON 으로 변경
        string jsonData = JsonUtility.ToJson(obj, true);
        //JSON 파일 저장할 경로 설정
        //string path = Path.Combine(Application.dataPath, "objData.json");
        ////JSON 파일로 저장
        //File.WriteAllText(path, jsonData);

        if (Client.instance != null)
        {
            //방장
            //if(Client.instance.isOwner)
            if (true)
            {
                Client.instance.Save(jsonData, idx);
            }
        }
        else
        {
            string path = Path.Combine(Application.dataPath, "objData.json");
            //JSON 파일로 저장
            File.WriteAllText(path, jsonData);
        }
        audio.clip = SaveSound;
        audio.Play();
        timestamp(true);

    }

    //불러오기
    public void Load()
    {
        mainMenu.SetActive(false);
        loadSlotMenu.SetActive(true);

        audio.clip = ButtonSound;
        audio.Play();
        timestamp(false);
    }

    public void loadSlot(int idx)
    {

        if (idx != 5)
        {
            Furniture.GetComponent<FurnitureManager>().Clear();
            if (Client.instance != null)
            {
                //방장
                if (Client.instance.isOwner)
                {
                    string jsonData = Client.instance.Load(idx);

                    //읽어온 JSON 파일을 가구 정보를 담는 리스트 객체(ObjDataList)로 변환
                    ObjDataList obj = JsonUtility.FromJson<ObjDataList>(jsonData);

                    Furniture.GetComponent<FurnitureManager>().ListToFurniture(obj);
                }
            }
            else
            {
                //JSON 파일 불러올 경로 설정
                string path = Path.Combine(Application.dataPath, "objData.json");
                //JSON 파일 불러옴
                string jsonData = File.ReadAllText(path);

                //읽어온 JSON 파일을 가구 정보를 담는 리스트 객체(ObjDataList)로 변환
                ObjDataList obj = JsonUtility.FromJson<ObjDataList>(jsonData);

                Furniture.GetComponent<FurnitureManager>().ListToFurniture(obj);
            }
            audio.clip = LoadSound;
            audio.Play();
        }

        else
        {
            Furniture.GetComponent<FurnitureManager>().Clear();
            Back();
        }
    }

    //스크린샷 찍기
    public void Screenshot()
    {
        //byte[] bytes = I360Render.Capture(1024, true);
        //if (bytes != null)
        //{   // 파일 저장 경로 추후에 변경필요
        //    string path = Path.Combine(Application.dataPath, "360render.png");
        //    File.WriteAllBytes(path, bytes);
        //    Debug.Log("360 render saved to " + path);
        //}

        //take a screenshot

        //스크린샷을 위한 딜레이
        StartCoroutine(ScreenShotWait(0.3f));

        for (int i = 1; i < 1000000; i++)
        {
            if (!Capture.doesScreenshotExist(i))
            {
                Capture.TakeScreenShot(i);
                break;
            }
        }


        //record the location of the cube
        //CubesPos = GameObject.Find("Cube (1)").gameObject.transform.position;

        //you need to wait a small amount of time for the screenshot to be saved
        //Invoke("screenshotWindow.GetComponent<FadeInOut>().Fade()", 0.5f);






        audio.clip = ScreenshotSound;
        audio.Play();
    }

    // 설정하기
    public void CallSettingsMenu()
    {

        //GameObject mainMenu = transform.parent.Find("MainMenu").gameObject;
        //GameObject mainMenu = GameObject.Find("Player/CanvasMainMenu/MainMenu");
        mainMenu.SetActive(false);
        //Instantiate(furnitureMenu, transform.position + Vector3.forward*2, furnitureMenu.transform.rotation);
        //transform.parent.Find("SettingsMenu").gameObject.SetActive(true);
        settingsMenu.SetActive(true);

        audio.clip = ButtonSound;
        audio.Play();

    }

    //뒤로가기
    public void Back()
    {
        furnitureMenu.SetActive(false);
        settingsMenu.SetActive(false);
        loadSlotMenu.SetActive(false);
        saveSlotMenu.SetActive(false);
        firendSlotMenu.SetActive(false);

        //selectTextureMenu.SetActive(false);
        mainMenu.SetActive(true);
        //transform.parent.Find("FurnitureMenu").gameObject.SetActive(false);
        //transform.parent.Find("SettingsMenu").gameObject.SetActive(false);
        //transform.parent.Find("MainMenu").gameObject.SetActive(true);
        audio.clip = ButtonSound;
        audio.Play();
    }

    //메뉴창 키기
    public void CallMainMenu()
    {
        if (!mainMenu.activeSelf)
        {
            mainMenu.SetActive(true);
            furnitureMenu.SetActive(false);
            settingsMenu.SetActive(false);
            

            saveSlotMenu.SetActive(false);
            loadSlotMenu.SetActive(false);
            firendSlotMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
              
            furnitureMenu.SetActive(false);
            settingsMenu.SetActive(false);
            

            saveSlotMenu.SetActive(false);
            loadSlotMenu.SetActive(false);
            firendSlotMenu.SetActive(false);
        }
        audio.clip = ButtonSound;
        audio.Play();
    }

    //친구네집
    public void CallFriendMenu()
    {
        mainMenu.SetActive(false);
        firendSlotMenu.SetActive(true);

        audio.clip = ButtonSound;
        audio.Play();
        FriendsList();
    }


    public void FriendsList()
    {
        audio.clip = ButtonSound;
        audio.Play();

        if (Client.instance != null && Client.instance.isConnect)
        {
            string[] rooms;
            rooms = Client.instance.RoomList();
            Debug.Log("firendSlotMenu.start ," + rooms.Length);
            int i;
            int cnt = 0;
            for (i = 0; i < rooms.Length; i++)
            {
                Debug.Log("Compare " + rooms[i].CompareTo(Client.instance.UserID));
                //방이 내 방이면 건너 뜀
                //방이 이미 내가 있는 방이면 건너 뜀
                if(rooms[i].CompareTo(Client.instance.UserID) == 0 ||
                    rooms[i].CompareTo(Client.instance.curRoom) == 0)
                {
                    cnt++;
                    continue;
                }

                //버튼 번호
                int idx = i - cnt;

                //룸 번호
                int chk = i;
                roomList.GetChild(idx).GetComponent<VRUIOutlineButton>().interactable = true;
                roomList.GetChild(idx).GetComponentInChildren<Text>().text = rooms[i];
                VRUIOutlineButton btn = roomList.GetChild(idx).GetComponent<VRUIOutlineButton>();
                btn.onClick.AddListener(() => EnterFriendRoom(chk));
                btn.onClick.AddListener(() => Client.instance.curRoom = rooms[chk]);

            }
            for (i = i-cnt; i < roomList.childCount; i++)
            {
                roomList.GetChild(i).GetComponentInChildren<Text>().text = "비어 있음";
                roomList.GetChild(i).GetComponent<VRUIOutlineButton>().interactable = false;
            }
        }
        else
        {
            for (int i=0; i<roomList.childCount; i++)
            {
                roomList.GetChild(i).GetComponentInChildren<Text>().text = "비어 있음";
                roomList.GetChild(i).GetComponent<VRUIOutlineButton>().interactable = false;
            }
        }
    }

    public void EnterFriendRoom(int idx)
    {
        Debug.Log("Enter Friend Room , " + idx);
        Client.instance.RoomEnter(idx);
        audio.clip = ButtonSound;
        audio.Play();

        //연결 해제
        RoomNetwork roomNetwork = GameObject.Find("RoomNetworkManager").GetComponent<RoomNetwork>();
        GameObject voiceNetwork = GameObject.Find("RoomNetworkManager");


        roomNetwork.StopHost();
        roomNetwork.StopClient();

        //Get Component
        //음성 해제
        //voiceNetwork.GetComponent<VoiceConnection>().Client.Disconnect();

        roomNetwork.networkAddress = Client.instance.roomIp;
        //음성네트워크 방제
        //voiceNetwork.GetComponent<ConnectAndJoin>().RoomName = roomList.GetChild(idx).GetComponentInChildren<Text>().text;

        
        roomNetwork.StartClient();
        //StartCoroutine(WaitForTexture(0.7f));
        string voiceRoom = roomList.GetChild(idx).GetComponentInChildren<Text>().text;

        char[] checkName = roomList.GetChild(idx).GetComponentInChildren<Text>().text.ToCharArray();
        for (int i = 0; i < checkName.Length; i++)
        {
            if (!char.IsLetterOrDigit(checkName[i]))
            {
                Debug.Log(i);
                voiceRoom = roomList.GetChild(idx).GetComponentInChildren<Text>().text.Remove(i);
                break;
            }
        }
        roomNetwork.JoinOrCreateRoom(voiceRoom);
        //voiceNetwork.GetComponent<ConnectAndJoin>().ConnectNow();

    }

    public void EnterMyRoom()
    {

        RoomNetwork roomNetwork = GameObject.Find("RoomNetworkManager").GetComponent<RoomNetwork>();
        GameObject voiceNetwork = GameObject.Find("RoomNetworkManager");

        if(roomNetwork.networkAddress == "localhost")
        {
            return;
        }

        //loadSlotMenu.SetActive(true);

        //만약 이미 내 방이라면 옮길 필요가 없음

        Furniture.GetComponent<FurnitureManager>().Clear();

        Client.instance.RoomMake();
        audio.clip = ButtonSound;
        audio.Play();


        roomNetwork.StopHost();
        roomNetwork.StopClient();
        //voiceNetwork.GetComponent<VoiceConnection>().Client.Disconnect();
        //voiceNetwork.GetComponent<VoiceConnection>().


        roomNetwork.networkAddress = Client.instance.roomIp;
        Client.instance.curRoom = Client.instance.UserID;
        //voiceNetwork.GetComponent<ConnectAndJoin>().RoomName = Client.instance.UserID;
        StartCoroutine(Wait(0.5f));
        roomNetwork.StartHost();

        


        roomNetwork.JoinOrCreateRoom(Client.instance.UserID);
        //voiceNetwork.GetComponent<ConnectAndJoin>().ConnectNow();
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    IEnumerator WaitForTexture(float time)
    {
        yield return new WaitForSeconds(time);
        Transform Room = GameObject.Find("Room").transform;
        for(int i=0; i< Room.childCount; i++)
        {
            Room.GetChild(i).GetComponent<setTexture>().EnterFriendRoom();
        }
    }

    IEnumerator ScreenShotWait(float time)
    {

        mainMenu.GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("LocalPlayer").transform.GetChild(1).gameObject.SetActive(false);
        //왼손
        GameObject.Find("LocalPlayer").transform.GetChild(0).GetChild(0)
            .GetChild(1).gameObject.SetActive(false);
        //오른손
        GameObject.Find("LocalPlayer").transform.GetChild(0).GetChild(0)
            .GetChild(2).gameObject.SetActive(false);
        //레이저
        GameObject.Find("LocalPlayer").transform.GetChild(0).GetChild(1)
            .GetChild(0).gameObject.SetActive(false);


        yield return new WaitForSeconds(time);


        mainMenu.GetComponent<CanvasGroup>().alpha = 1f;
        GameObject.Find("LocalPlayer").transform.GetChild(1).gameObject.SetActive(true);
        //왼손
        GameObject.Find("LocalPlayer").transform.GetChild(0).GetChild(0)
            .GetChild(1).gameObject.SetActive(true);
        //오른손
        GameObject.Find("LocalPlayer").transform.GetChild(0).GetChild(0)
            .GetChild(2).gameObject.SetActive(true);
        //레이저
        GameObject.Find("LocalPlayer").transform.GetChild(0).GetChild(1)
           .GetChild(0).gameObject.SetActive(true);
    }

}

