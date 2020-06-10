using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHealth : MonoBehaviour
{
    public SpriteRenderer joe;  
    public Sprite hp100;
    public Sprite hp75;
    public Sprite hp50;
    public Sprite hp25;
    public Sprite hp0;

    Sprite[] bob = new Sprite[5];
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        bob[0] = hp100;
        bob[1] = hp75;
        bob[2] = hp50;
        bob[3] = hp25;
        bob[4] = hp0;
    }

    void Update(){
        joe.sprite = bob[index];
    }
    
    public void display(int i){
        index = i;
    }
}