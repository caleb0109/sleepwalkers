using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int strength;

    public Bars hBar;
    public Image pBox;

    private Sprite neutral;
    private Sprite damageTake;
    private void Start()
    {
        hBar.SetMax(maxHealth);
        neutral = Resources.Load<Sprite>("pfps/Yuichi/Yuichi_Neutral");
        damageTake = Resources.Load<Sprite>("pfps/Yuichi/Yuichi_TakeDamage");
        pBox.sprite = neutral;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            health = 0;
        }

        hBar.ShowHealth(health);
        pBox.sprite = damageTake;

        StopAllCoroutines();
        StartCoroutine(BackToNeutral());
    }

    // changes the sprite back to neutral after taking damage
    private IEnumerator BackToNeutral()
    {
        float duration = 1.0f;

        while (duration > 0f)
        {
            duration -= Time.deltaTime;

            if (duration < 0.1f)
            {
                pBox.sprite = neutral;
            }

            yield return null;
        }
    }
}
