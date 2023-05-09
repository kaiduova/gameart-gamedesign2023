using Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemyManager : InputMonoBehaviour
{
    public GameObject Player;
    Rigidbody2D rb;
    public float _enemyVelocity, _timer;
    public int _waypointNum;

    public GameObject Waypoint1, Waypoint2, Waypoint3, Waypoint4, Waypoint5;
    public List<GameObject> wayPoints;
    public GameObject _bullet;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        rb = this.GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Square");
        _enemyVelocity = 3f;
        _waypointNum = 0;
        _timer = 4;

    }

    // Update is called once per frame
    void Update()
    {
        // Movements();

        Vector3 CurrentWaypoint = wayPoints[_waypointNum].transform.position;
        Vector3 newPos = Vector3.MoveTowards(transform.position, CurrentWaypoint, _enemyVelocity * Time.deltaTime);
        rb.position = newPos;

        float nextWaypoint = Vector3.Distance(transform.position, CurrentWaypoint);

        if (nextWaypoint <= 0.5)
        {
            _waypointNum++;    
        }
        if (_waypointNum >= 5) _waypointNum = 0;   
        
        if (CurrentInput.GetKeyA)
        { 
            Instantiate(_bullet, new Vector2(rb.position.x, rb.position.y), Quaternion.identity);
        }
       // rb.MovePosition()
        
    }

    public void Movements()
    {
        switch (_waypointNum)
        {
            case 1: //rb.MovePosition(Waypoint1.transform.position);
                //this.transform.position = (Waypoint1.transform.position);
                Vector3 newPos = Vector3.MoveTowards(transform.position, Waypoint1.transform.position, (_enemyVelocity * Time.deltaTime));
                rb.position = newPos;
                print(rb.position);
                _waypointNum = 2;
                break;
            case 2:
                //rb.MovePosition(Waypoint2.transform.position);
                //this.transform.position = (Waypoint2.transform.position);
                Vector3 newPos2 = Vector3.MoveTowards(transform.position, Waypoint2.transform.position, (_enemyVelocity * Time.deltaTime));
                rb.position = newPos2;
                print(rb.position);
                _waypointNum = 3;
                break;
            case 3:
                //rb.MovePosition(Waypoint3.transform.position);
                // this.transform.position = (Waypoint3.transform.position);
                Vector3 newPos3 = Vector3.MoveTowards(transform.position, Waypoint3.transform.position, (_enemyVelocity * Time.deltaTime));
                rb.position = newPos3;
                print(rb.position);
                _waypointNum = 4;
                break;
            case 4:
                //rb.MovePosition(Waypoint4.transform.position);
                //this.transform.position = (Waypoint4.transform.position);
                Vector3 newPos4 = Vector3.MoveTowards(transform.position, Waypoint4.transform.position, (_enemyVelocity * Time.deltaTime));
                rb.position = newPos4;
                print(rb.position);
                _waypointNum = 5;
                break;
            case 5:
                // rb.MovePosition(Waypoint5.transform.position);
                //this.transform.position = (Waypoint5.transform.position);
                Vector3 newPos5 = Vector3.MoveTowards(transform.position, Waypoint5.transform.position, (_enemyVelocity * Time.deltaTime));
                rb.position = newPos5;
                print(rb.position);
                _waypointNum = 1;
                break;
        }

    }


    private void FixedUpdate()
    {
       /* _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            Movements();   
            _timer = 4;

        }*/

       // if (rb.position.x > 0 && rb.position.x < 7) rb.AddForce(new Vector2(_enemyVelocity, rb.velocity.y));
       //if (rb.position.x < 0 && rb.velocity.x > -7) rb.AddForce(new Vector2(-_enemyVelocity, rb.velocity.y));
    }
}
