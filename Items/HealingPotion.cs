using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        GameObject go = collision.gameObject;

        if (go.name == "Player")
        {
            go.GetComponent<Player>().Heal();

            this.GetComponent<AudioSource>().Play();
            this.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(this.gameObject, 2f);
        }
    }
}
