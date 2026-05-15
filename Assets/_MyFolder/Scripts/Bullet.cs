using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    //’e‚ÌƒXƒs[ƒh
    [SerializeField] float speed = 10f;

    
    void Start()
    {
        // ’e‚ÌƒXƒs[ƒh‚Ì‚ğDataManager‚©‚çæ“¾‚·‚é
        speed = 10f + (DataManager.instance.playerSpeedLevel * 0.5f);
    }

    
    void Update()
    {
        //ˆÚ“®
        transform.position += transform.up * speed * Time.deltaTime;
    }

    //’e‚ª“G‚É“–‚½‚Á‚½‚ç’e‚ğ”j‰ó‚·‚é
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
