using Fusion;
using UnityEngine;
using Unity.Cinemachine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    CinemachineCamera cam;
    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("Camera").GetComponent<CinemachineCamera>();
    }
    public void PlayerJoined(PlayerRef player)
    {
        if(player == Runner.LocalPlayer)
        {
            NetworkObject networkObject = Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            cam.Follow = networkObject.transform;
            cam.LookAt = networkObject.transform;

        }
    }
}
