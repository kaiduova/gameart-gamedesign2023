using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EatingEnemy;

public class EnemyDummy : MonoBehaviour
{


    public GameObject Dummies;
    public bool EnemyDummyReady;
    public bool PlayerTakingTime;

    public UniversalHealthSystem UniversalHealthSystem;
    public EatingEnemy EatingEnemy;

    public float DummyDispayTimer;

    public PlaceableCameraTrigger PlaceableCameraTrigger;

    void Start()
    {
        if (Dummies.activeInHierarchy) Dummies.SetActive(false);
        UniversalHealthSystem = GetComponent<UniversalHealthSystem>();
        EatingEnemy = GetComponent<EatingEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (UniversalHealthSystem.Dead) {
            EnemyDummyReady = true;
            PlaceableCameraTrigger.DesiredOthroSize = 12;
        }

        if (UniversalHealthSystem.Dead && EnemyDummyReady) {
            DummyDispayTimer += Time.deltaTime;
            if (DummyDispayTimer >= 15) {
                PlayerTakingTime = true;
                DummyDispayTimer = 15;
            }
        }

        if (EatingEnemy.State == EatingEnemyState.Bounce && PlayerTakingTime) Dummies.SetActive(true);
        else Dummies.SetActive(false);
    }
}
