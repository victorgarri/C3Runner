using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

namespace C3Runner.Multiplayer
{
    [AddComponentMenu("")]
    public class NetworkRoomPlayerExt : NetworkRoomPlayer
    {
        [SyncVar] public bool wantsToSpectate;
        [SyncVar] public string playerName;

        [SyncVar] public Color playerColor = Color.clear;
        float hue = .5F;
        float lastHue;

        Color player1Color, player2Color, player3Color, player4Color;

        private void Awake()
        {
            Color newCol;
            if (ColorUtility.TryParseHtmlString("#A9E065", out newCol))
                player1Color = newCol;

            if (ColorUtility.TryParseHtmlString("#E0974F", out newCol))
                player2Color = newCol;

            if (ColorUtility.TryParseHtmlString("#38B9E0", out newCol))
                player3Color = newCol;

            if (ColorUtility.TryParseHtmlString("#C948E0", out newCol))
                player4Color = newCol;
        }

        public void Update()
        {
            if (isServer && Input.GetKeyDown(KeyCode.E))
            {
                wantsToSpectate = !wantsToSpectate;
            }
        }

        public override void OnStartClient()
        {
            //Debug.Log($"OnStartClient {gameObject}");
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
        }


        void DrawNameInputField()
        {
            if (NetworkClient.active && isLocalPlayer && isServer)
            {
                GUILayout.BeginArea(new Rect(20f, 600f, 800f, 200f));

                GUI.Toggle(new Rect(0, 0, 800, 20), wantsToSpectate, "(E) Espectar desde host ");
                ////PLAYER NAME INPUT FIELD
                //GUI.Label(new Rect(0, 0, 40, 20), "Name: ");
                //playerName = GUI.TextField(new Rect(30 + 20, 0, 100, 20), playerName == string.Empty ? "PlayerName" + netId : playerName);

                updateSyncVars(wantsToSpectate, playerName, playerColor);

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

                //if (playerColor.Equals(Color.clear))
                //{

                Color c = Color.white;
                var id = index + 1;
                switch ((id % 5) + 1)
                {
                    case 1: c =player1Color; break;
                    case 2: c = player1Color; break;
                    case 3: c = player2Color; break;
                    case 4: c = player3Color; break;
                    case 5: c = player4Color; break;
                }


                //playerColor = Color.HSVToRGB((netId % 5) / 5f, 1, 1);
                playerColor = c;

                //}

                GUI.color = Color.white;

                updateSyncVars(wantsToSpectate, playerName, playerColor);

                //
                GUILayout.EndArea();
            }

        }


        //MUY IMPORTANTE, PORQUE SI LA SYNCVAR NO ES ACTUALIZADA DESDE EL SERVIDOR NO LA TOMA EN CUENTA.
        [Command]
        void updateSyncVars(bool wantsToSpectate, string playerName, Color playerColor)
        {
            this.wantsToSpectate = wantsToSpectate;
            this.playerName = playerName;
            this.playerColor = playerColor;
        }
    }
}

