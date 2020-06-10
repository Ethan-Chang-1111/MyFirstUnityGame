using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    //parallaxing
    public Transform[] backgrounds;
    public float smoothing = 1f;

    private float[] parallaxScales;
    private Transform camera;
    private Vector3 previousCamPos;
    private Vector3 relVelocity = Vector3.zero;

    //moving the background with player/camera? or repeate the background as the player moves
    public GameObject player;
    Rigidbody2D rb;
    BasicMove playerMovement;
    Vector2 previousPlayerPosition;
    
    //called before Start, great for referances
    void Awake(){
        camera = Camera.main.transform;
        rb = player.GetComponent<Rigidbody2D>();
        playerMovement = player.GetComponent<BasicMove>();
        //Debug.Log(playerMovement.moveSpeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        previousPlayerPosition = rb.position;
        previousCamPos = camera.position;

        parallaxScales = new float[backgrounds.Length];
        for(int i = 0;i<backgrounds.Length;i++){
            parallaxScales[i] = backgrounds[i].position.z * -1;            
        }
    }

    // Update is called once per frame
    void Update()
    {
        parallaxScript();
        //backgroundFollowScript();
    }

    void parallaxScript(){
        //parallax update
        for(int i = 0; i<backgrounds.Length;i++){
            //parallax is the opposite of the camera movement because the previous frame multiplied by the scale
            float parallax = (previousCamPos.x - camera.position.x) * parallaxScales[i];
        
            //set target x position which is the current position plus parallax
            float backgroundPositionX = backgrounds[i].position.x + parallax;
            //create target position which is background's current position plus target x position
            //can also configure this to set parallax to the y position
            Vector3 target = new Vector3(backgroundPositionX,backgrounds[i].position.y,backgrounds[i].position.z);

            //fade target and current background position
            //Time.deltaTime converts frames to seconds
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, target,smoothing*Time.deltaTime);
            
        }
        //set the previous cam position to the new cam position
        previousCamPos = camera.position;
    }
    void backgroundFollowScript(){
        //background lock on player update
        for(int i = 0;i<backgrounds.Length;i++){
            float targetX = backgrounds[i].position.x + (rb.position.x - previousPlayerPosition.x);
            float targetY = backgrounds[i].position.y + (rb.position.y - previousPlayerPosition.y);
            Vector2 targetFull = new Vector2(targetX,targetY);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position,rb.position,(smoothing)*Time.deltaTime);
        }
        previousPlayerPosition = rb.position;
        previousCamPos = camera.position;
    }
}