using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace C3Runner.Multiplayer
{
    public class RoomPlayerUI : NetworkBehaviour
    {

        [SyncVar] public uint index;
        public Text txtPlayerNum, txtPlayerStatus;
        public Button btnDisconnect;

        public void DeleteSelf()
        {
            Destroy(gameObject);
        }

    }

}
