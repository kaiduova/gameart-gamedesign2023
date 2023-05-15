using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EatingEnemyAnimation : MonoBehaviour
{


    [SerializeField] private EatingEnemy _eatingEnemy;


    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _player;

    private void FlipEnemy()
    {
        if (transform.position.x > _player.transform.position.x || transform.position.x == _player.transform.position.x) transform.eulerAngles = new Vector3(0, 0, 0);
        if (transform.position.x < _player.transform.position.x) transform.eulerAngles = new Vector3(0, 180, 0);
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FlipEnemy();
        
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
