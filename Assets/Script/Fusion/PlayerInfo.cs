using Fusion;
using UnityEngine;
[System.Serializable]
public struct PlayerInfo : INetworkStruct
{
    public int health;
    public int mana;
    public int score;
}
