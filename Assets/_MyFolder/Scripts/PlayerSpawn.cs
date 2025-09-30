using UnityEngine;
using DG.Tweening;

public class PlayerSpawn : MonoBehaviour
{

    [SerializeField] GameObject playerPrefab;

    [SerializeField] Transform playerSpawn;

    

    void Start()
    {
        Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);

    }

    
    
}
