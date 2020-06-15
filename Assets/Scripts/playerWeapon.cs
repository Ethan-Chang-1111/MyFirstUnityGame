using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerWeapon : MonoBehaviour
{
    [SerializeField] private Transform FirePoint = null;
    [SerializeField] private GameObject Bullet = null;//1
    [SerializeField] private GameObject BigBullet = null;//2
    [SerializeField] private GameObject BombBullet = null;//3
    [SerializeField] private GameObject Flare = null;//4

    //Animation
    [SerializeField] private Animator animator = null;
    [SerializeField] private GameObject BulletTextUI = null;
    Text text;

    //fireing cooldowns
    float shotCD1 = .5f;
    float shotCD2 = 2f;
    float shotCD3 = 3f;
    float shotCD4 = 1f;

    float timer1 = 0f;
    float timer2 = 0f;
    float timer3 = 0f;
    float timer4 = 0f;

    int ammo1 = 10;
    int ammo2 = 10;
    int ammo3 = 10;
    int ammo4 = 10;


    void Start(){
        text = BulletTextUI.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = toString();
        //chain of else if so player cant fire more than one bullet type at a time
        if(Input.GetButton("Fire1")){
            timer1 += Time.fixedDeltaTime;
            if(timer1 >= shotCD1 && ammo1>0){
                Shoot(1);
                timer1 = 0f;
            }
            animator.SetBool("IsFireing", true);
        }else if(Input.GetButtonUp("Fire1")){//only calls when button has been pressed and released
            timer1 = 0;
            animator.SetBool("IsFireing", false);
        }else if(Input.GetButton("Fire2")){
            timer2 += Time.fixedDeltaTime;
            if(timer2 >= shotCD2 && ammo2>0){
                Shoot(2);
                timer2 = 0f;
            }
            animator.SetBool("IsFireing", true);
        }else if(Input.GetButtonUp("Fire2")){
            timer2 = 0;
            animator.SetBool("IsFireing", false);
        }else if(Input.GetButton("Fire3")){
            timer3 += Time.fixedDeltaTime;
            if(timer3 >= shotCD3 && ammo3>0){
                Shoot(3);
                timer3 = 0f;
            }
            animator.SetBool("IsFireing", true);
        }else if(Input.GetButtonUp("Fire3")){
            timer3 = 0;
            animator.SetBool("IsFireing", false);
        }else if(Input.GetButton("Flare")){
            timer4 += Time.fixedDeltaTime;
            if(timer4 >= shotCD4 && ammo4>0){
                sendFlare();
                timer4 = 0f;
            }
            animator.SetBool("IsFireing", true);
        }else if(Input.GetButtonUp("Flare")){
            timer4 = 0;
            animator.SetBool("IsFireing", false);
        }

    }

    //i=1 bullet, i=2, bigbullet
    void Shoot(int type){
        //shooting logic
        bool inAir = animator.GetBool("InAir");
        if(!inAir){
            if(type==1){
                Instantiate(Bullet,FirePoint.position,FirePoint.rotation);
            }else if(type==2){
                Instantiate(BigBullet,FirePoint.position,FirePoint.rotation);
            }else if(type==3){
                Instantiate(BombBullet,FirePoint.position,FirePoint.rotation);
            }
            calcAmmo(type,-1);
        }
    }

    void sendFlare(){
        GameObject flare = Instantiate(Flare,FirePoint.position,FirePoint.rotation);
        
        if(Input.GetButton("LookUp")){
            flare.GetComponent<Rigidbody2D>().velocity = transform.up*25f;
        }else if(Input.GetButton("Crouch")){
            flare.GetComponent<Rigidbody2D>().velocity = transform.right*5f;
        }else{
            flare.GetComponent<Rigidbody2D>().velocity = transform.right*25f;
        }
        calcAmmo(4,-1);
        Destroy(flare, 20f);
    }

    public void powerUp(bool active, int type){
        shotCD1 = (type == 0)?active?(0f):(.5f):shotCD1;
        shotCD2 = (type == 1)?active?(0f):(2f):shotCD2;
        shotCD3 = (type == 4)?active?(0f):(3f):shotCD3;
    }

    public void calcAmmo(int type, int amount){
        ammo1 = (type == 1)?ammo1+amount:ammo1;
        ammo2 = (type == 2)?ammo2+amount:ammo2;
        ammo3 = (type == 3)?ammo3+amount:ammo3;
        ammo4 = (type == 4)?ammo4+amount:ammo4;
    }

    string toString(){
        string str = "";
        str += "Ammo1: " + ammo1 + "\n";
        str += "Ammo2: " + ammo2 + "\n";
        str += "Ammo3: " + ammo3 + "\n";
        str += "Ammo4: " + ammo4 + "\n";
        return str;
    }
}