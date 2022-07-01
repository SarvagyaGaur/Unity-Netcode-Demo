using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TeamPlayer : NetworkBehaviour
{
    [SerializeField] private Color[] teamColours;
    //[SerializeField] private Renderer teamColourRenderer;
    // public GameObject gameObject;
    private NetworkVariable<byte> teamIndex = new NetworkVariable<byte>((byte)0);
    [ServerRpc]
    public void SetTeamServerRpc(byte newTeamIndex)
    {
        if (newTeamIndex > 3) { return; }
        teamIndex.Value = newTeamIndex;
    }
    private void OnEnable()
    {
        teamIndex.OnValueChanged += OnTeamChanged;
    }
    private void OnDisable()
    {
        teamIndex.OnValueChanged -= OnTeamChanged;
    }
    private void OnTeamChanged(byte oldTeamIndex, byte newTeamIndex)
    {
        if (!IsClient) { return; }
        this.GetComponent<Renderer>().material.color = teamColours[newTeamIndex];
    }
}