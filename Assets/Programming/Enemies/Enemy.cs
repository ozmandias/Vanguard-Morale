using UnityEngine;
using UnityEngine.AI;

public class Enemy : Person {
    [SerializeField] Info enemyInfo;

    public virtual void Start() {
        enemyInfo = GetComponent<Info>();
    }

    public virtual void Update() {
        Move();
        Attack();
    }

    public override void Move() {
        
    }

    public override void Rotate() {

    }

    public override void Attack() {

    }

    bool collision = false;
    void OnCollisionEnter(Collision otherCollision) {
        if(otherCollision.collider.gameObject.CompareTag("MasterKnightAttackCollider")) {
            if(collision == false && enemyInfo.isDead == false) {
                collision = true;
                Info attackCharacterInfo = otherCollision.collider.gameObject.GetComponentInParent<Info>();
                enemyInfo.ReduceHealth(attackCharacterInfo.damage);
            }
        }
    }

    void OnCollisionExit(Collision otherCollision) {
        collision = false;
    }
}