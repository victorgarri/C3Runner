using Mirror;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using C3Runner.Multiplayer;

public class CanvasHUD : MonoBehaviour
{
    public GameObject PanelStart;
    public GameObject PanelRoom;
    public GameObject PanelStop;

    public Button buttonHost, buttonServer, buttonClient, buttonStop;
    public Button buttonStartGame;

    public InputField inputFieldAddress;
    public GameObject playerList;
    public GameObject playerUIPrefab;


    public Text serverText;
    public Text clientText;


    public static CanvasHUD canvasInstance;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (canvasInstance == null)
        {
            canvasInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Update the canvas text if you have manually changed network managers address from the game object before starting the game scene
        if (NetworkManager.singleton.networkAddress != "localhost") { inputFieldAddress.text = NetworkManager.singleton.networkAddress; }

        //Adds a listener to the main input field and invokes a method when the value changes.
        inputFieldAddress.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        //Make sure to attach these Buttons in the Inspector
        buttonHost.onClick.AddListener(ButtonHost);
        buttonServer.onClick.AddListener(ButtonServer);
        buttonClient.onClick.AddListener(ButtonClient);
        buttonStop.onClick.AddListener(ButtonStop);

        //This updates the Unity canvas, we have to manually call it every change, unlike legacy OnGUI.
        SetupCanvas();
        SceneManager.activeSceneChanged += ChangedActiveScene;

    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        SetupCanvas();
    }

    // Invoked when the value of the text field changes.
    public void ValueChangeCheck()
    {
        NetworkManager.singleton.networkAddress = inputFieldAddress.text;
    }

    public void ButtonHost()
    {
        NetworkManager.singleton.StartHost();
        SetupCanvas();
    }

    public void ButtonServer()
    {
        NetworkManager.singleton.StartServer();
        SetupCanvas();
    }

    public void ButtonClient()
    {
        NetworkManager.singleton.StartClient();
        SetupCanvas();
    }

    public void ButtonStop()
    {
        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
        }

        SetupCanvas();
    }

    public void SetupCanvas()
    {
        // Here we will dump majority of the canvas UI that may be changed.

        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            //Start
            if (NetworkClient.active)
            {
                //Connecting phase
                PanelStart.SetActive(false);
                PanelRoom.SetActive(false);
                PanelStop.SetActive(true);
                clientText.text = "Connecting to " + NetworkManager.singleton.networkAddress + "..";
            }
            else
            {
                //MainScene
                PanelStart.SetActive(true);
                PanelRoom.SetActive(false);
                PanelStop.SetActive(false);
            }
        }
        else
        {
            //Not Start
            PanelStart.SetActive(false);
            PanelStop.SetActive(true);

            // server / client status message
            if (NetworkServer.active)
            {
                serverText.text = "Server: active. Transport: " + Transport.activeTransport;
            }
            if (NetworkClient.isConnected)
            {
                clientText.text = "Client: address=" + NetworkManager.singleton.networkAddress;
            }

            NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
            if (room)
            {
                //room scene
                if (room.showRoomGUI && NetworkManager.IsSceneActive(room.RoomScene))
                {
                    foreach (Transform item in playerList.transform)
                    {
                        item.GetComponent<RoomPlayerUI>().DeleteSelf();
                    }

                    //foreach (NetworkRoomPlayerExt rp in room.roomSlots)
                    //{
                    //    rp.
                    //}

                    NetworkRoomPlayerExt.indices.Clear();
                    NetworkRoomPlayerExt.playerUIs.Clear();

                    PanelRoom.SetActive(true);
                    //RoomCanvas();
                }
                else
                {
                    PanelRoom.SetActive(false);
                }
            }

        }
    }

    void RoomCanvas()
    {
        //var playerList = GameObject.Find("playerList");
        //if (playerList != null)
        //    transform.GetChild(0).parent = playerList.transform;
        var players = NetworkManager.singleton.numPlayers;
        for (int i = 0; i < players; i++)
        {
            //assign whatever it is needed
            GameObject playerUI = Instantiate(playerUIPrefab);
            playerUI.transform.parent = playerList.transform;
        }

    }
}