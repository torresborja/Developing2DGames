using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour
{
    public float maxHitPoints;
    public float startingHitPoints;

    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }

    public abstract void ResetCharacter();

    public abstract IEnumerator DamageCharacter(int damage, float interval);

    public virtual IEnumerator FlickerCharacter()
    {
        // 1
        GetComponent<SpriteRenderer>().color = Color.red;
        // 2
        yield return new WaitForSeconds(0.1f);
        // 3
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
