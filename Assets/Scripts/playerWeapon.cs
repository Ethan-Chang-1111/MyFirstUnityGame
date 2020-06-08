using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerWeapon : MonoBehaviour
{
    public Transform FirePoint;
    public GameObject Bullet;
    public GameObject BigBullet;
    //Animation
    public Animator animator;
    int shotCD1 = 1;
    int shotCD2 = 3;
    float timer1 = 0f;
    float timer2 = 0f;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1")){
            timer1 += Time.fixedDeltaTime;
            if((int)timer1 == shotCD1){
                Shoot(1);
                timer1 = 0f;
            }
            animator.SetBool("IsFireing", true);
        }else if(Input.GetButtonUp("Fire1")){
            timer1 = 0;
            animator.SetBool("IsFireing", false);
        }else if(Input.GetButton("Fire2")){
            timer2 += Time.fixedDeltaTime;
            if((int)timer2 == shotCD2){
                Shoot(2);
                timer2 = 0f;
            }
            animator.SetBool("IsFireing", true);
        }else if(Input.GetButtonUp("Fire2")){
            timer2 = 0;
            animator.SetBool("IsFireing", false);
        }
    }
    //i=1 bullet, i=2, bigbullet
    void Shoot(int i){
        //shooting logic
        if(i==1){
            Instantiate(Bullet,FirePoint.position,FirePoint.rotation);
        }else if(i==2){
            Instantiate(BigBullet,FirePoint.position,FirePoint.rotation);
        }
    }
}
