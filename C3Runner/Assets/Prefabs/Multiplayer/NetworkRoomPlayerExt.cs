using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine.UI;
using System;

namespace C3Runner.Multiplayer
{
    [AddComponentMenu("")]
    public class NetworkRoomPlayerExt : NetworkRoomPlayer
    {
        [SyncVar] public bool wantsToSpectate;
        [SyncVar] public string playerName;
        [SyncVar] public Color playerColor = Color.clear;
        [SyncVar] public float playerType; //0 male, 100 female


        Color player1Color, player2Color, player3Color, player4Color, player5Color, player6Color, player7Color, player8Color;

        private void Start()
        {
            base.Start();
            Color newCol;
            if (ColorUtility.TryParseHtmlString("#A9E065", out newCol))
                player1Color = newCol;

            if (ColorUtility.TryParseHtmlString("#E0974F", out newCol))
                player2Color = newCol;

            if (ColorUtility.TryParseHtmlString("#38B9E0", out newCol))
                player3Color = newCol;

            if (ColorUtility.TryParseHtmlString("#C948E0", out newCol))
                player4Color = newCol;

            if (ColorUtility.TryParseHtmlString("#DB463B", out newCol))
                player5Color = newCol;

            if (ColorUtility.TryParseHtmlString("#2B25DB", out newCol))
                player6Color = newCol;

            if (ColorUtility.TryParseHtmlString("#DBB90F", out newCol))
                player7Color = newCol;

            if (ColorUtility.TryParseHtmlString("#008F47", out newCol))
                player8Color = newCol;




        }



        public void Update()
        {
            if (isServer && Input.GetKeyDown(KeyCode.E))
            {
                wantsToSpectate = !wantsToSpectate;
                if (toggleSpectate != null) toggleSpectate.isOn = wantsToSpectate;
            }

        }

        public GameObject PanelRoom;
        public InputField ifName;
        public Slider sldType;
        public Toggle toggleSpectate;

        public override void OnStartClient()
        {
            try
            {
                PanelRoom = CanvasHUD.canvasInstance.PanelRoom;

                ifName = GameObject.Find("ifName").GetComponent<InputField>();
                sldType = GameObject.Find("sldType").GetComponent<Slider>();
                toggleSpectate = GameObject.Find("toggleSpectator").GetComponent<Toggle>();

                toggleSpectate.gameObject.SetActive(false);
                ifName.onValueChanged.AddListener(delegate { updateSyncVarName(ifName.text); });
                sldType.onValueChanged.AddListener(delegate { updateSyncVarType(sldType.value); });

                toggleSpectate.onValueChanged.AddListener(delegate { updateSyncVarSpectate(toggleSpectate.isOn); });
            }
            catch (Exception e) { }
        }

        public override void OnClientEnterRoom()
        {
            //Debug.Log($"OnClientEnterRoom {SceneManager.GetActiveScene().path}");
        }

        public override void OnClientExitRoom()
        {
            //Debug.Log($"OnClientExitRoom {SceneManager.GetActiveScene().path}");
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            //Debug.Log($"IndexChanged {newIndex}");
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            //Debug.Log($"ReadyStateChanged {newReadyState}");
        }

        public override void OnGUI()
        {
            base.OnGUI();
            DrawNameInputField();
            DrawNameInputFieldCanvas();
        }


        void DrawNameInputField()
        {
            if (NetworkClient.active && isLocalPlayer && isServer)
            {
                GUILayout.BeginArea(new Rect(20f, 600f, 800f, 200f));

                GUI.Toggle(new Rect(0, 0, 800, 20), wantsToSpectate, "(E) Espectar desde host ");

                updateSyncVars(wantsToSpectate, playerName, playerColor, (int)playerType);

                //
                GUILayout.EndArea();
            }

            if (NetworkClient.active && isLocalPlayer)
            {
                GUILayout.BeginArea(new Rect(20f, 400f, 150f, 200f));

                //PLAYER NAME INPUT FIELD
                GUI.color = playerColor;

                GUI.Label(new Rect(0, 0, 40, 20), "Name: ");
                playerName = GUI.TextField(new Rect(30 + 20, 0, 100, 20), playerName == string.Empty ? "Player" + netId : playerName, 8);

                Color c = Color.white;
                var id = index + 1;
                switch ((id % 9) + 1)
                {
                    case 1: c = player1Color; break;
                    case 2: c = player1Color; break;
                    case 3: c = player2Color; break;
                    case 4: c = player3Color; break;
                    case 5: c = player4Color; break;
                    case 6: c = player5Color; break;
                    case 7: c = player6Color; break;
                    case 8: c = player7Color; break;
                    case 9: c = player8Color; break;
                }


                //playerColor = Color.HSVToRGB((netId % 5) / 5f, 1, 1);
                playerColor = c;

                GUI.color = Color.white;

                GUI.Label(new Rect(0, 30, 40, 20), "Type: ");
                playerType = Mathf.Floor(GUI.HorizontalSlider(new Rect(30 + 20, 30, 100, 20), (int)playerType / 100, 0, 1) * 100);

                updateSyncVars(wantsToSpectate, playerName, playerColor, (int)playerType);

                //
                GUILayout.EndArea();
            }

        }
        void DrawNameInputFieldCanvas()
        {
            if (NetworkClient.active && isLocalPlayer && isServer)
            {
                toggleSpectate.gameObject.SetActive(true);
                updateSyncVars(wantsToSpectate, playerName, playerColor, (int)playerType);
            }

            if (NetworkClient.active && isLocalPlayer)
            {
                //GUILayout.BeginArea(new Rect(20f, 400f, 150f, 200f));

                //PLAYER NAME INPUT FIELD
                //GUI.color = playerColor;

                //GUI.Label(new Rect(0, 0, 40, 20), "Name: ");
                //playerName = GUI.TextField(new Rect(30 + 20, 0, 100, 20), playerName == string.Empty ? "Player" + netId : playerName, 8);
                ifName.text = playerName == string.Empty ? "Player" + netId : playerName;

                Color c = Color.white;
                var id = index + 1;
                switch ((id % 9) + 1)
                {
                    case 1: c = player1Color; break;
                    case 2: c = player1Color; break;
                    case 3: c = player2Color; break;
                    case 4: c = player3Color; break;
                    case 5: c = player4Color; break;
                    case 6: c = player5Color; break;
                    case 7: c = player6Color; break;
                    case 8: c = player7Color; break;
                    case 9: c = player8Color; break;
                }


                playerColor = c;
                ifName.transform.parent.Find("txtName").GetComponent<Text>().color = playerColor;
                //GUI.color = Color.white;

                //GUI.Label(new Rect(0, 30, 40, 20), "Type: ");
                //playerType = Mathf.Floor(GUI.HorizontalSlider(new Rect(30 + 20, 30, 100, 20), (int)playerType / 100, 0, 1) * 100);

                sldType.value = playerType;

                //updateSyncVars(wantsToSpectate, playerName, playerColor, (int)playerType);

                //
                //GUILayout.EndArea();
                updateSyncVars(wantsToSpectate, playerName, playerColor, (int)playerType);
            }

        }

        //MUY IMPORTANTE, PORQUE SI LA SYNCVAR NO ES ACTUALIZADA DESDE EL SERVIDOR NO LA TOMA EN CUENTA.
        [Command]
        void updateSyncVars(bool wantsToSpectate, string playerName, Color playerColor, int playerType)
        {
            this.wantsToSpectate = wantsToSpectate;
            this.playerName = playerName;
            this.playerColor = playerColor;
            this.playerType = playerType;
        }

        [Command]
        public void updateSyncVarName(string playerName)
        {
            this.playerName = playerName;
        }

        [Command]
        public void updateSyncVarColor(Color playerColor)
        {
            this.playerColor = playerColor;
        }

        [Command]
        public void updateSyncVarType(float playerType)
        {
            //this.playerType = ((int)playerType / 100) * 100;
            this.playerType = playerType * 100;
            sldType.value = this.playerType;
        }

        [Command]
        public void updateSyncVarSpectate(bool wantsToSpectate)
        {
            this.wantsToSpectate = wantsToSpectate;
            //something();
        }
    }
}

