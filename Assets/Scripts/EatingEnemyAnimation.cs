using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingEnemyAnimation : MonoBehaviour
{


    [SerializeField] private EatingEnemy _eatingEnemy;


    [SerializeField] private Animator _animator;






    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_eatingEnemy.State == EatingEnemyState.Default)
        {
            _animator.SetBool("Patrolling", true);
            _animator.SetBool("ChasingPlayer", false);
            _animator.SetBool("ChewingPlayer", false);

        }

        if (_eatingEnemy.State == EatingEnemyState.Attack)
        {
            _animator.SetBool("Patrolling", false);
            _animator.SetBool("ChasingPlayer", true);
        }

        if (_eatingEnemy.State == EatingEnemyState.Swallowed)
        {
            _animator.SetBool("ChasingPlayer", false);
            _animator.SetBool("ChewingPlayer", true);
        }


        if (_eatingEnemy.State == EatingEnemyState.Bounce)
        {
            _animator.SetBool("BouncePad", true);
        }

    }
}
