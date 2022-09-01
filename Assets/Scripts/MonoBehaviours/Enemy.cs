using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public int damageStrength;
    // 2
    Coroutine damageCoroutine;

    float hitPoints;


    public override void ResetCharacter()
    {
        // 2
        hitPoints = startingHitPoints;
    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        // 2
        while (true)
        {
            StartCoroutine(FlickerCharacter());
            hitPoints = hitPoints - damage;
            // 4
            if (hitPoints <= float.Epsilon)
            {
                // 5
                KillCharacter();
                break;
            }
            // 6
            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                // 7
                break;
            }
        }
    }

    private void OnEnable()
    {
        // 1
        ResetCharacter();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 2
        if (collision.gameObject.CompareTag("Player"))
        {
            // 3
            Player player = collision.gameObject.GetComponent<Player>();
            // 4
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(player.DamageCharacter(damageStrength, 1.0f));
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 2
        if (collision.gameObject.CompareTag("Player"))
        {
            // 3
            if (damageCoroutine != null)
            {
                // 4
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

}
