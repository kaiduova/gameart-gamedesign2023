using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingEnemyAnimation : MonoBehaviour
{


    [SerializeField] private EatingEnemy _eatingEnemy;


    [SerializeField] private Animator _animator;




    [SerializeField] private GameObject _foregroundHead;

  

    void Start()
    {
        
    }


    private void ShowForegourndHead() {
        _foregroundHead.SetActive(true);
    }

    private void HideForegroundHead() {
        _foregroundHead.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_eatingEnemy.State == EatingEnemyState.Default)
        {
            _animator.SetBool("Patrol", true);

            _animator.SetBool("Notice", false);
            _animator.SetBool("Chase", false);

        }

        if (_eatingEnemy.State == EatingEnemyState.PlayerNoticed)
        {
            _animator.SetBool("Notice", true);

            _animator.SetBool("Patrol", false);
        }


        if (_eatingEnemy.State == EatingEnemyState.Attack)
        {
            _animator.SetBool("Chase", true);

            _animator.SetBool("Notice", false);

        }

        if (_eatingEnemy.State == EatingEnemyState.Swallowed)
        {
            _animator.SetBool("Chase", false);

        }


        if (_eatingEnemy.State == EatingEnemyState.Bounce)
        {
            _animator.SetBool("BouncePad", true);



        }

        if (_eatingEnemy.State == EatingEnemyState.WakeUp)
        {
            _animator.SetBool("BouncePad", false);

        }


    }
}
