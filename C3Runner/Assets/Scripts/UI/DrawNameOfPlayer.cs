using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawNameOfPlayer : MonoBehaviour
{
    Player3D player;
    public TextMesh playerName;
    void Start()
    {
        player = GetComponent<Player3D>();
        playerName.text = player.playerName;
        playerName.color = player.playerColor;
    }

}
