using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class EnemyHealth : MonoBehaviour
{
    public Slider HealthSlider;

    public Animator anim;
    private int Health;
    private bool EnemyIsDead;
    // Start is called before the first frame update
    void Start()
    {
        Health = 100;
        EnemyIsDead = false;
    }

    public void SetHealth(int damage)
    {
        Health -= damage;
        Debug.Log("Health" + Health);
        UpdateHealthUI();
        CheckIfDead();
    }

    public int GetHealth()
    {
        return Health;
    }

    void CheckIfDead()
    {
        if (Health <= 0)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("knockdown_loop_A"))
            {
                EnemyIsDead = true;
            }
            else
            {
                anim.SetBool("Death_b", true);
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                agent.speed = 0.0f;

                //anim.enabled = false;
                //anim.enabled = true;
                //anim.SetTrigger("knockdown");
                EnemyIsDead = true;
            }
        }
    }

    void RemoveIfDead()
    {
        if (EnemyIsDead)
        {
            //GameManager.Instance.OneEnemyKilled();
            Destroy(gameObject);
        }
    }

    void UpdateHealthUI()
    {
        HealthSlider.value = Health;
    }
}