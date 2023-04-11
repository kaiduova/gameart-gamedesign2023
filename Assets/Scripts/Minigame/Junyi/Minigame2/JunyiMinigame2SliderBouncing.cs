using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input;

public class JunyiMinigame2SliderBouncing : InputMonoBehaviour
{    /* PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables */
    enum AllBouncePadState {locked, unlock,returning }
    [Header("Internally referenced components")]
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    [Header("Components")]
    [SerializeField] GameObject playerBall;
    [Header("Variables")]
    int _lockCount;
    AllBouncePadState bouncePadState = AllBouncePadState.locked;
    bool isLocked=true;
    private void Awake()
    {
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocked)
        {
            if (_spriteRenderer.color != Color.green)
            {
                _spriteRenderer.color = Color.green;
            }
            if (_lockCount > 80)
            {
                if (_rigidbody2D.rotation != 0)
                {
                    if (_rigidbody2D.rotation < 2 && _rigidbody2D.rotation > -2f)
                    {
                        _rigidbody2D.MoveRotation(0);
                    }
                    else
                    {
                        if (_rigidbody2D.rotation > 0)
                        {
                            _rigidbody2D.MoveRotation(_rigidbody2D.rotation - 1);
                        }
                        else if (_rigidbody2D.rotation < 0)
                        {
                            _rigidbody2D.MoveRotation(_rigidbody2D.rotation + 1);
                        }

                    }
                }

            }
            else
            {
                _lockCount++;

            }
        }
        else
        {
            if (_spriteRenderer.color != Color.magenta)
            {
                _spriteRenderer.color = Color.magenta;
            }

            if (CurrentInput.RightStick.x < -0.4f || CurrentInput.RightStick.x > 0.4f)
            {
                _rigidbody2D.MoveRotation(_rigidbody2D.rotation - CurrentInput.RightStick.x);
            }
            //if (CurrentInput.RightStick.x < -0.7f)
            //{

            //    _rigidbody2D.MoveRotation(_rigidbody2D.rotation + 0.8f) ;
            //}
            //else if (CurrentInput.RightStick.x < -0.55f)
            //{

            //    _rigidbody2D.MoveRotation(_rigidbody2D.rotation + 0.5f);
            //}
            //else if (CurrentInput.RightStick.x < -0.4f)
            //{

            //    _rigidbody2D.MoveRotation(_rigidbody2D.rotation + 0.35f);
            //}

            //if (CurrentInput.RightStick.x > 0.7f)
            //{

            //    _rigidbody2D.MoveRotation(_rigidbody2D.rotation - 0.8f);
            //}
            //else if (CurrentInput.RightStick.x > 0.55f)
            //{

            //    _rigidbody2D.MoveRotation(_rigidbody2D.rotation - 0.5f);
            //}
            //else if (CurrentInput.RightStick.x > 0.4f)
            //{

            //    _rigidbody2D.MoveRotation(_rigidbody2D.rotation - 0.35f);
            //}

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == playerBall)
        {
            isLocked = true;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != playerBall)
        {
            isLocked = true;
        }    
        else if (collision.gameObject == playerBall)
        {
            isLocked = false;
            _lockCount = 0;
        }

    }
}
