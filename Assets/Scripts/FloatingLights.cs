using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingLights : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Rigidbody2D rb = null;
    Vector2 initPos;
    float range = 3f;
    private float maxDistanceDelta = 1f;

    public float accelerationTime = 2f;
    private Vector2 movement;
    private float timeLeft;

    void Start()
    {
        initPos = rb.position;
    }

    void Update(){
        timeLeft -= Time.deltaTime;
            if(timeLeft <= 0){
                movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                timeLeft += accelerationTime;
            }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //move from current position to a random position within range
        rb.position = Vector3.MoveTowards(rb.position, initPos+(movement * range), maxDistanceDelta*Time.deltaTime);
    }
}
