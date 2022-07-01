using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlaneSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject planePrefab;
    private void Update()
    {
        if(!IsOwner) { return; }
        if(!Input.GetKeyDown(KeyCode.Space)) { return; }
        SpawnPlaneServerRpc();
        Instantiate(planePrefab, transform.position, Quaternion.identity);
    }
    [ServerRpc(Delivery = RpcDelivery.Unreliable)]
    private void SpawnPlaneServerRpc()
    {
        SpawnPlaneClientRpc();
    }
    [ClientRpc(Delivery = RpcDelivery.Unreliable)]
    private void SpawnPlaneClientRpc()
    {
        if(IsOwner) { return; }
        Instantiate(planePrefab, transform.position , Quaternion.identity);
    }
}
