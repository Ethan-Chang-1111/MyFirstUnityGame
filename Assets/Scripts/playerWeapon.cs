using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerWeapon : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform FirePoint = null;
    [SerializeField] private GameObject Bullet = null;//1
    [SerializeField] private GameObject BigBullet = null;//2
    [SerializeField] private GameObject BombBullet = null;//3
    [SerializeField] private GameObject Flare = null;//4

    //Animation
    [SerializeField] private Animator animator = null;
    [SerializeField] private GameObject BulletTextUI = null;
    Text text;

    //active firing cooldowns
    float shotCD0 = .5f;
    float shotCD1 = 1f;
    float shotCD2 = 2f;
    float shotCD3 = .5f;

    //base firing cooldown
    static float BshotCD0 = .5f;
    static float BshotCD1 = 1f;
    static float BshotCD2 = 2f;
    static float BshotCD3 = .5f;

    //timer for each firing cooldown
    float timer0 = 0f;
    float timer1 = 0f;
    float timer2 = 0f;
    float timer3 = 0f;

    int[] ammo = new int[4];
    int startAmmo = 100;

    //alternate energy based ammo system
    int maxEnergy = 1000;
    int energy = 0;
    float timer5 = 0f;
    float bEnergyRegenTime = 1f;
    static float cEnergyRegenTime; 
    static int bEnergyRegenAmt = 10;
    int cEnergyRegenAmt;

    bool usingBullets = false;

    #endregion

    void Start(){
        text = BulletTextUI.GetComponent<Text>();
        for(int i=0;i<ammo.Length;i++){
            ammo[i] = startAmmo;
        }

        shotCD0 = BshotCD0;
        shotCD1 = BshotCD1;
        shotCD2 = BshotCD2;
        shotCD3 = BshotCD3;

        energy = maxEnergy;
        cEnergyRegenAmt = bEnergyRegenAmt;
        cEnergyRegenTime = bEnergyRegenTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        if(usingBullets){
            text.text = toStringBullet();
        }else{
        //energy regen
        timer5 += Time.fixedDeltaTime;
        if(timer5 >= cEnergyRegenTime){
            if(energy>maxEnergy){//if above max, decrease energy
                cEnergyRegenAmt = -1 * bEnergyRegenAmt;   
            }else if(energy<maxEnergy){//if under max, increase energy
                cEnergyRegenAmt = bEnergyRegenAmt;
            }

            int differance = Mathf.Abs(energy - maxEnergy);
            if(differance < Mathf.Abs(cEnergyRegenAmt)){
                energy = maxEnergy;
            }else{
                energy += cEnergyRegenAmt;
            }

            timer5 = 0f;
        }
            text.text = toStringEnergy();
        }

        #region If-Else mess
        //chain of else if so player cant fire more than one bullet type at a time
        if(Input.GetButton("Fire1")){
            timer0 += Time.fixedDeltaTime;
            if(timer0 >= shotCD0 && ammo[0]>0){
                Shoot(0);
                timer0 = 0f;
            }
            animator.SetBool("IsFireing", true);
        }else if(Input.GetButtonUp("Fire1")){//only calls when button has been pressed and released
            timer0 = 0;
            animator.SetBool("IsFireing", false);
        }else if(Input.GetButton("Fire2")){
            timer1 += Time.fixedDeltaTime;
            if(timer1 >= shotCD1 && ammo[1]>0){
                Shoot(1);
                timer1 = 0f;
            }
            animator.SetBool("IsFireing", true);
        }else if(Input.GetButtonUp("Fire2")){
            timer1 = 0;
            animator.SetBool("IsFireing", false);
        }else if(Input.GetButton("Fire3")){
            timer2 += Time.fixedDeltaTime;
            if(timer2 >= shotCD2 && ammo[2]>0){
                Shoot(2);
                timer2 = 0f;
            }
            animator.SetBool("IsFireing", true);
        }else if(Input.GetButtonUp("Fire3")){
            timer2 = 0;
            animator.SetBool("IsFireing", false);
        }else if(Input.GetButton("Flare")){
            timer3 += Time.fixedDeltaTime;
            if(timer3 >= shotCD3 && ammo[3]>0){
                Shoot(3);
                timer3 = 0f;
            }
            animator.SetBool("IsFireing", true);
        }else if(Input.GetButtonUp("Flare")){
            timer3 = 0;
            animator.SetBool("IsFireing", false);
        }
        #endregion

    }

    void Shoot(int type){
        bool inAir = animator.GetBool("InAir");
        if(!inAir){
            if(usingBullets){
                pickType(type);
                calcAmmo(type,-1);
            }else{
                bool enoughEnergy = energyBullet(type);
                if(enoughEnergy){
                    pickType(type);
                }
            }
        }
    }

    void pickType(int type){
        switch(type){
            case 0:
                Instantiate(Bullet,FirePoint.position,FirePoint.rotation);
                break;
            case 1:
                Instantiate(BigBullet,FirePoint.position,FirePoint.rotation);
                break;
            case 2:
               Instantiate(BombBullet,FirePoint.position,FirePoint.rotation);
                break;
            case 3:
                sendFlare();
                break;
            default:
                Debug.Log("Invalid Bullet Type");
                break;
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
        Destroy(flare, 20f);
    }

    public void powerUp(bool active, int PwrType){
        shotCD0 = (PwrType == 0)?((active)?(0f):BshotCD0):shotCD0;
        shotCD1 = (PwrType == 1)?((active)?(0f):BshotCD1):shotCD2;
        shotCD2 = (PwrType == 4)?((active)?(0f):BshotCD2):shotCD2;
        shotCD3 = (PwrType == 5)?((active)?(0f):BshotCD3):shotCD3;   
    }
    
    #region Bullet System
    public void calcAmmo(int type, int amount){
        ammo[type] += amount;
    }

    public void resetAmmo(){
        for(int i=0;i<ammo.Length;i++){
            ammo[i] = startAmmo;
        }
    }

    string toStringBullet(){
        string str = "";
        for(int i=0;i<ammo.Length;i++){
            str += "Ammo" + (i+1).ToString() + ": " + ammo[i] + "\n";
        }
        return str;
    }
    #endregion

    #region Energy System
    public bool energyBullet(int type){
        int curEnergy = energy;
        switch(type){
            case 0:
                energy -= 1;
                break;
            case 1:
                energy -= 10;
                break;
            case 2:
                energy -= 20;
                break;
            case 3:
                energy -= 5;
                break;
            default:
                Debug.Log("Invalid Bullet Type");
                break;
        }
        if(energy >= 0){//enough energy to fire
            return true;
        }else{
            energy = curEnergy;
            return false;
        }
    }

    public void calcEnergy(int amount){
        energy += amount;
    }

    public void resetEnergy(){
        energy = maxEnergy;
    }

    string toStringEnergy(){
        return "Energy: " + energy.ToString() + "/" + maxEnergy.ToString();
    }


    #endregion
}