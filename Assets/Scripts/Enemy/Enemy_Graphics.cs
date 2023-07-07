using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_Graphics : MonoBehaviour
{

    [SerializeField] private Transform player;
    private SpriteRenderer spr; //Sprite Renderer

    // Start is called before the first frame update
    private void Start()
    {
        spr = GetComponent<SpriteRenderer>(); //Initializes Sprite Renderer
    }

    // Update is called once per frame
    private void Update()
    {
        if (player.position.x < transform.position.x) {
            spr.flipX = true;
        } else {
            spr.flipX = false;
        }
    }
}
