using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    public float maxHealth;
    public Text hitUI;

    public GameObject[] enemiesAttacks;

    public void TakeDamage(float damage)
    {
        health -= damage;
        hitUI.text = damage.ToString();
        hitUI.gameObject.GetComponent<Animator>().SetBool("gotHit", true);

        StopAllCoroutines();
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        float duration = 2f;
        while (duration > 0f)
        {
            duration -= Time.deltaTime;

            if (duration < 1f)
            {
                hitUI.gameObject.GetComponent<Animator>().SetBool("gotHit", false);
            }

            yield return null;
        }
    }
}
