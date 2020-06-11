using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHealth : MonoBehaviour
{
    [SerializeField] private SpriteRenderer joe = null;  
    [SerializeField] private Sprite hp100 = null;
    [SerializeField] private Sprite hp75 = null;
    [SerializeField] private Sprite hp50 = null;
    [SerializeField] private Sprite hp25 = null;
    [SerializeField] private Sprite hp0 = null;

    Sprite[] bob = new Sprite[5];
    int index;

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