using UnityEngine;

public class CombatManager : MonoBehaviour {
    float counterTimer = 0;
    public void CounterAttack(float attackTimer) {
        counterTimer += Time.deltaTime;
        if(counterTimer < attackTimer / 2) {
            
        }
    }
}