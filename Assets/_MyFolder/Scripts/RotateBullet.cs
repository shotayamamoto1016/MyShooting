using UnityEngine;

public class RotateBullet : MonoBehaviour
{

    [SerializeField] float rotateSpeed = 1f;
    
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotateSpeed));
    }
}
