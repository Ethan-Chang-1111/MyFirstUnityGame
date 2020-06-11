using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpAbstract : MonoBehaviour
{
    public abstract void OnTriggerEnter2D(Collider2D hitInfo);

    public abstract void effect(Collider2D player);
}
