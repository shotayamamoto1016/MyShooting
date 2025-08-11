using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    //’e‚ÌƒXƒs[ƒh
    [SerializeField] float speed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
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
