using Cinemachine;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

namespace C3Runner.Multiplayer
{
    [AddComponentMenu("")]
    public class NetworkRoomManagerExt : NetworkRoomManager
    {


        /// <summary>
        /// This is called on the server when a networked scene finishes loading.
        /// </summary>
        /// <param name="sceneName">Name of the new scene.</param>
        public override void OnRoomServerSceneChanged(string sceneName)
        {
            // spawn the initial batch of Rewards
            //if (sceneName == GameplayScene)
            //    Spawner.InitialSpawn();


        }

        /// <summary>
        /// Called just after GamePlayer object is instantiated and just before it replaces RoomPlayer object.
        /// This is the ideal point to pass any data like player name, credentials, tokens, colors, etc.
        /// into the GamePlayer object as it is about to enter the Online scene.
        /// </summary>
        /// <param name="roomPlayer"></param>
        /// <param name="gamePlayer"></param>
        /// <returns>true unless some code in here decides it needs to abort the replacement</returns>
        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            Spectator spectator = gamePlayer.GetComponent<Spectator>();
            spectator.wantsToSpectate = roomPlayer.GetComponent<NetworkRoomPlayerExt>().wantsToSpectate;
            //return true;

            Player3D player = gamePlayer.GetComponent<Player3D>();
            player.playerColor = roomPlayer.GetComponent<NetworkRoomPlayerExt>().playerColor;
            player.playerName = roomPlayer.GetComponent<NetworkRoomPlayerExt>().playerName;
            player.playerType = roomPlayer.GetComponent<NetworkRoomPlayerExt>().playerType;
            //Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAA-" + player.playerName);
            return true;
        }


        public override void OnRoomStopClient()
        {
            base.OnRoomStopClient();
        }

        public override void OnRoomStopServer()
        {
            base.OnRoomStopServer();
        }

        /*
            This code below is to demonstrate how to do a Start button that only appears for the Host player
            showStartButton is a local bool that's needed because OnRoomServerPlayersReady is only fired when
            all players are ready, but if a player cancels their ready state there's no callback to set it back to false
            Therefore, allPlayersReady is used in combination with showStartButton to show/hide the Start button correctly.
            Setting showStartButton false when the button is pressed hides it in the game scene since NetworkRoomManager
            is set as DontDestroyOnLoad = true.
        */

        bool showStartButton;

        public override void OnRoomServerPlayersReady()
        {
            // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
#if UNITY_SERVER
            base.OnRoomServerPlayersReady();
#else
            showStartButton = true;
#endif
        }

        bool isUISet;
        Canvas canvas;
        Text startGameTxt;

        public override void OnGUI()
        {
            base.OnGUI();
            if (!isUISet)
            {
                SetUpUI();
            }

            if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME")) svrChngScene();

            if (btnStartGame != null)
                btnStartGame.gameObject.SetActive(allPlayersReady && showStartButton);


        }

        public Button btnStartGame;

        void SetUpUI()
        {
            //btnStartGame = CanvasHUD.canvasInstance.PanelRoom.transform.Find("btnReady").GetComponent<Button>();
            btnStartGame.onClick.AddListener(delegate { svrChngScene(); });







            isUISet = true;
        }

        void svrChngScene()
        {
            showStartButton = false;

            ServerChangeScene(GameplayScene);
        }
    }
}
