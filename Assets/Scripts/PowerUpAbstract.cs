using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpAbstract : MonoBehaviour
{
    public abstract void Start();

    public abstract void Update();

    public abstract void OnTriggerEnter2D(Collider2D hitInfo);

    public abstract void startEffect();

    public abstract void endEffect();
}
