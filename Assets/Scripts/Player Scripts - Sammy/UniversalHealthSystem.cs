using UnityEngine;

public class UniversalHealthSystem : MonoBehaviour {

    /* PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables */

    public float CurrentHealth;
    public float MaxHealth;
    public bool Dead;

    public void TakeDamage(float damageRecieved) {
        CurrentHealth -= damageRecieved;
    }

    private void Start() {
        CurrentHealth = MaxHealth;
    }

    private void Update() {
        if (CurrentHealth <= 0) {
            CurrentHealth = 0;
            Dead = true;
        }
    }

    public static void TryDealDamage(GameObject go, float value)
    {
        if (go.TryGetComponent<UniversalHealthSystem>(out var health))
        {
            health.TakeDamage(value);
        }
    }
}