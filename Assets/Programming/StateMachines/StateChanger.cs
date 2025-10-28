using UnityEngine;

public class StateChanger : MonoBehaviour {
    Person mainPerson;

    void Start() {
        mainPerson = gameObject.GetComponent<Person>();
    }

    void Update() {
        CheckState();
    }

    void CheckState() {
        if (mainPerson.GetInfo().aiType == AIType.StateMachine) {
            switch(mainPerson.personState) {
                case StateMachine.Idle:
                    if(mainPerson.target) {
                        if(mainPerson.attackingTarget) {
                            mainPerson.ChangeState(StateMachine.Follow);
                        }
                    }
                    
                    if(mainPerson.destination) {
                        if(mainPerson.reachDestination) {
                            // mainPerson.ChangeState(StateMachine.Work);
                        } else {
                            mainPerson.ChangeState(StateMachine.Move);
                        }
                    }
                    break;
                case StateMachine.Move:
                    if(mainPerson.target) {
                        if(mainPerson.attackingTarget) {
                            mainPerson.ChangeState(StateMachine.Follow);
                        }
                    }
                    
                    if(mainPerson.destination) {
                        if(mainPerson.reachDestination) {
                            mainPerson.ChangeState(StateMachine.Idle);
                        }
                    } else {
                        mainPerson.ChangeState(StateMachine.Idle);
                    }
                    break;
                case StateMachine.Work:
                    break;
                case StateMachine.Follow:
                    if(mainPerson.target) {
                        if(mainPerson.attackingTarget && mainPerson.atAttackDistance) {
                            mainPerson.ChangeState(StateMachine.Attack);
                        }
                    }

                    if(mainPerson.destination) {
                        if(!mainPerson.attackingTarget) {
                            mainPerson.ChangeState(StateMachine.Move);
                        }
                    } else {
                        if(!mainPerson.attackingTarget) {
                            mainPerson.ChangeState(StateMachine.Idle);
                        }
                    }
                    break;
                case StateMachine.Attack:
                    if(mainPerson.target) {
                        if(mainPerson.attackingTarget && !mainPerson.atAttackDistance) {
                            mainPerson.ChangeState(StateMachine.Follow);
                        }
                    }
                    
                    if(mainPerson.destination) {
                        if(!mainPerson.attackingTarget) {
                            mainPerson.ChangeState(StateMachine.Move);
                        }          
                    } else {
                        if(!mainPerson.attackingTarget) {
                            mainPerson.ChangeState(StateMachine.Idle);
                        }
                    }
                    break;
                case StateMachine.Combat:
                    mainPerson.ChangeState(StateMachine.Idle);
                    break;
                case StateMachine.Hurt:
                    if(!mainPerson.GetInfo().isDead) {
                        if(mainPerson.target) {
                            if(mainPerson.attackingTarget && mainPerson.atAttackDistance) {
                                mainPerson.ChangeState(StateMachine.Attack);
                            } else {
                                mainPerson.ChangeState(StateMachine.Follow);
                            }
                        }

                        if(mainPerson.destination) {
                            if(!mainPerson.attackingTarget) {
                                mainPerson.ChangeState(StateMachine.Move);
                            }
                        } else {
                            if(!mainPerson.attackingTarget) {
                                mainPerson.ChangeState(StateMachine.Idle);
                            }
                        }
                    } else {
                        mainPerson.ChangeState(StateMachine.Dead);
                    }
                    break;
                default:
                    break;
            }

            if (mainPerson.isHurt)
            {
                mainPerson.ChangeState(StateMachine.Hurt);
            }
        } else if(mainPerson.GetInfo().aiType == AIType.CombatAI) {
            mainPerson.ChangeState(StateMachine.Combat);
        }
    }
}