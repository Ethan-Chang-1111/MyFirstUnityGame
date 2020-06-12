using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerWeapon : MonoBehaviour
{
    [SerializeField] private Transform FirePoint = null;
    [SerializeField] private GameObject Bullet = null;
    [SerializeField] private GameObject BigBullet = null;
    [SerializeField] private GameObject BombBullet = null;

    //Animation
    [SerializeField] private Animator animator = null;

    //fireing cooldowns
    float shotCD1 = .5f;
    float shotCD2 = 2f;
    float shotCD3 = 3f;
    float timer1 = 0f;
    float timer2 = 0f;
    float timer3 = 0f;

    //aiming up
    Quaternion rotation;

    // Update is called once per frame
    void Update()
    {
        //chain of else if so player cant fire more than one bullet type at a time
        if(Input.GetButton("Fire1")){
            timer1 += Time.fixedDeltaTime;
            if(timer1 >= shotCD1){
                Shoot(1);
                timer1 = 0f;
            }
            animator.SetBool("IsFireing", true);

        }else if(Input.GetButtonUp("Fire1")){//only calls when button has been pressed and released
            timer1 = 0;
            animator.SetBool("IsFireing", false);


        }else if(Input.GetButton("Fire2")){
            timer2 += Time.fixedDeltaTime;
            if(timer2 >= shotCD2){
                Shoot(2);
                timer2 = 0f;
            }
            animator.SetBool("IsFireing", true);
        }else if(Input.GetButtonUp("Fire2")){
            timer2 = 0;
            animator.SetBool("IsFireing", false);


        }else if(Input.GetButton("Fire3")){
            timer3 += Time.fixedDeltaTime;
            if(timer3 >= shotCD3){
                Shoot(3);
                timer3 = 0f;
            }
            animator.SetBool("IsFireing", true);
        }else if(Input.GetButtonUp("Fire3")){
            timer3 = 0;
            animator.SetBool("IsFireing", false);
        }


        if(Input.GetButton("LookUp")){
            aimUp(true);
        }else if(!Input.GetButton("LookUp")){
            aimUp(false);
        }
    }

    public void aimUp(bool up){
        if(up){
            rotation = Quaternion.Euler(0, 0, 90f);
        }else{
            rotation = FirePoint.rotation;
        }
    }

    //i=1 bullet, i=2, bigbullet
    void Shoot(int i){
        //shooting logic
        bool inAir = animator.GetBool("InAir");
        if(!inAir){
            if(i==1){
                Instantiate(Bullet,FirePoint.position,rotation);
            }else if(i==2){
                Instantiate(BigBullet,FirePoint.position,rotation);
            }else if(i==3){
                Instantiate(BombBullet,FirePoint.position,rotation);
            }
        }
    }

    public void powerUp(bool active, int type){
        shotCD1 = (type == 0)?active?(0f):(.5f):shotCD1;
        shotCD2 = (type == 1)?active?(0f):(2f):shotCD2;
        shotCD3 = (type == 4)?active?(0f):(3f):shotCD3;

        
    }
}