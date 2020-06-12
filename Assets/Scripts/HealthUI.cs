using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{

    Image picture;  
    [SerializeField] private Sprite hp100 = null;
    [SerializeField] private Sprite hp75 = null;
    [SerializeField] private Sprite hp50 = null;
    [SerializeField] private Sprite hp25 = null;
    [SerializeField] private Sprite hp0 = null;

    Sprite[] bob = new Sprite[5];
    public int index = 4;

    // Start is called before the first frame update
    void Start()
    {
        picture = GetComponent<Image>();
        index = 4;

        bob[0] = hp0;
        bob[1] = hp25;
        bob[2] = hp50;
        bob[3] = hp75;
        bob[4] = hp100;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        index = index>4?4:index<0?0:index;
        picture.sprite = bob[index];
        
    }
}
