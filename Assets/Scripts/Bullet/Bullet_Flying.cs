using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Mechanics : MonoBehaviour
{
    //Instance variables
    //Movement
    [SerializeField] private float bulletSpeed; //Speed of bullet
    private Rigidbody2D rb; //Bullet's rigidbody

    //Deleting Bullets
    private Renderer bulletRenderer; //Bullet's renderer

    //Bullet Property
    [SerializeField] private int bulletDamage;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();//Initializes rb
        bulletRenderer = GetComponent<Renderer>(); //Initializes bulletRenderer

        //If the bullet is not pointed right, it's pointed left
        //Change the bullet speed to negative to make it go left
        if (transform.eulerAngles.z != 0f) {
            bulletSpeed = -bulletSpeed;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        rb.velocity = new Vector2(bulletSpeed, 0); //Moves the bullet
        destroyBullet(); //Destroys the bullet if conditions are met
    }

    private void OnTriggerEnter2D(Collider2D hitInfo) {
        if (!hitInfo.gameObject.name.Equals("Ground")) {
           Debug.Log(hitInfo.name);
           Destroy(gameObject);
        } 
    }

    //Destroys the bullet if it's not visible on camera
    //Object pooling is better but it's like 5 bullets on screen at a time so ¯\_(ツ)_/¯
    private void destroyBullet() {
        if (!bulletRenderer.isVisible) {
            Destroy(gameObject);
        }
    }
}
