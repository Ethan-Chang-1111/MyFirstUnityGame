using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemy = null;
    [SerializeField] private Transform location = null;
    [SerializeField] private int rate = 2;
    float timer = 0f;
    // Update is called once per frame
    void Update()
    {
        timer += Time.fixedDeltaTime;
            if((int)timer == rate){
                Instantiate(enemy,location.position,location.rotation);
                timer = 0f;
            }
    }
}