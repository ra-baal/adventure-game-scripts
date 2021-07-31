using UnityEngine;
using System.Collections;

public class ColliderSensor : MonoBehaviour
{
    private int collisionCounter = 0;

    private float disableTimer;

    private void OnEnable() 
        => this.collisionCounter = 0;

    public bool IsOn()
    {
        if (disableTimer > 0)
            return false;

        return this.collisionCounter > 0;
    }

    public void Disable(float duration)
        => disableTimer = duration;

    void OnTriggerEnter2D(Collider2D collider2d)
    {
        if (collider2d.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            this.collisionCounter++;
    }

    void OnTriggerExit2D(Collider2D collider2d)
    {
        if (collider2d.gameObject.layer == LayerMask.NameToLayer("Terrain")) 
            this.collisionCounter--;
    }

    void Update()
        => disableTimer -= Time.deltaTime;


}
