using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shooting : MonoBehaviour
{
    //Instance variables
    [SerializeField] private Transform shootingPoint; //Position of where the bullet is fired
    [SerializeField] private GameObject bulletPrefab; //Prefab of bullet
    [SerializeField] private SpriteRenderer playerDirection; //SpriteRenderer of the plaayer

    // Update is called once per frame
    private void Update()
    {
        //Update position of bullet based on where player is
        //Position is local to the player (makes math easier)
        float bulletX = shootingPoint.localPosition.x;
        float bulletY = shootingPoint.localPosition.y;
        float bulletZ = shootingPoint.localPosition.z;
        Quaternion bulletRotate = Quaternion.Euler(0, 0, 0); //Resets the rotation of the bullet
        Vector3 bulletPos = new Vector3(bulletX, bulletY, bulletZ);


        //If player fires, get the direction the player is facing (flipX)
        //Technically object pooling is better for optimization, but I can't be bothered
        if (Input.GetButtonDown("Fire1")) {
            if (playerDirection.flipX) {
                bulletPos.x = -bulletPos.x; //Change the initial position of bullet so it goes left
                bulletRotate = Quaternion.Euler(0, 0, 180); //Rotates the bullet on z axis so it goes left
            }
            Instantiate(bulletPrefab, transform.position + bulletPos, bulletRotate); //Create a bullet object
        }

        
    }
}
