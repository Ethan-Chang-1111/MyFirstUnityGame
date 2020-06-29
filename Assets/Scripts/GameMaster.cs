using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private GameObject EmptyPowerUp = null;
    [SerializeField] private GameObject EmptyLights = null;
    [SerializeField] private GameObject EnemyList = null;
    
    
    public void reloadScene(){
        for(int i = 0; i<EmptyPowerUp.transform.childCount; i++){
            EmptyPowerUp.transform.GetChild(i).GetComponent<PowerUpAbstract>().respawn();
        }
        for(int i = 0; i<EmptyLights.transform.childCount; i++){
            EmptyLights.transform.GetChild(i).GetComponent<FloatingLights>().respawn();
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i<enemies.Length;i++){
            Destroy(enemies[i]);
        }
        GameObject[] activeBullets = GameObject.FindGameObjectsWithTag("Bullet");
        for(int i = 0; i<activeBullets.Length;i++){
            Destroy(activeBullets[i]);
        }
        Destroy(GameObject.FindGameObjectWithTag("PlacedEnemies"));
        Instantiate(EnemyList);

    }
}
