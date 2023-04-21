using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

namespace C3Runner.Multiplayer
{
    [AddComponentMenu("")]
    public class NetworkRoomPlayerExt : NetworkRoomPlayer
    {
        [SyncVar] public bool wantsToSpectate;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
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
            if (NetworkClient.active && isLocalPlayer)
            {
                GUILayout.BeginArea(new Rect(20f, 400f, 800f, 200f));

                //PLAYER NAME INPUT FIELD
                //GUI.Label(new Rect(0, 0, 40, 20), "Expectar: ");
                /*playerName = GUI.TextField(new Rect(30 + 20, 0, 100, 20), playerName == string.Empty ? "PlayerName" + netId : playerName); */

                GUI.Toggle(new Rect(0, 0, 800, 20), wantsToSpectate, "(E) Espectar desde host ");



                updateSyncVars(wantsToSpectate);

                //
                GUILayout.EndArea();
            }
        }


        //MUY IMPORTANTE, PORQUE SI LA SYNCVAR NO ES ACTUALIZADA DESDE EL SERVIDOR NO LA TOMA EN CUENTA.
        [Command]
        void updateSyncVars(bool w)
        {
            wantsToSpectate = w;
        }
    }
}

