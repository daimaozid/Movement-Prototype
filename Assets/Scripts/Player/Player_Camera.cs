using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{

    //Instance variables
    [SerializeField] private Transform player; //Player position, rotation and scale
    [SerializeField] private Rigidbody2D rb; //Player position, rotation and scale
    [SerializeField] private Vector3 offset; //Offests for the camera position
    [SerializeField] private Vector3 downOffset; //Offests for the camera position
    [SerializeField] private float smoothTime; //Time for camera to catch up, smaller value = less lag behind player
    private Vector3 velocity = Vector3.zero; //IDK why you need this but you need it for SmoothDamp

    //FixedUpdate is frame independent update
    //It's supposed to be used for physics but it works for smoothing camera so *shrugs*
    private void FixedUpdate()
    {

        //Defines the desired position of the camera
        Vector3 desiredPosition = new Vector3(player.position.x, 0, 0) + offset;

        if (Input.GetAxisRaw("Vertical") < 0f && rb.velocity.y == 0f) {
            desiredPosition = player.position + offset + downOffset;
        }

        //SmoothDamp smoothes the camera movement
        //Arguements: initial pos, final pos, idk but you need it, smoothing time
        //Higher smooting time = slower camera, vice versa
        Vector3 smoothedPosition = 
        Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        //Centers the camera on the player's position
        transform.position = smoothedPosition;

    }
}
