using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Sticky : MonoBehaviour
{
    //When player stands on top of platform, sync the player's movement to the platform
    private void OnTriggerEnter2D(Collider2D collision) {
       if (collision.gameObject.name.Equals("Player")) {
           collision.gameObject.transform.SetParent(transform);
       }
    }

    //When player leaves the top of platform, stop syncing the player's movement
    private void OnTriggerExit2D(Collider2D collision) {
       if (collision.gameObject.name.Equals("Player")) {
           collision.gameObject.transform.SetParent(null);
       }
    }

}
