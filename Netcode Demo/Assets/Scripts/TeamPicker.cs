using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TeamPicker : MonoBehaviour
{
    public void SelectTeam(int teamIndex)
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId;
        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return;
        }
        if (!networkClient.PlayerObject.TryGetComponent<TeamPlayer>(out TeamPlayer teamPlayer))
        {
            return;
        }
        teamPlayer.SetTeamServerRpc((byte)teamIndex);
    }
}
