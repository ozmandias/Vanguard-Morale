using UnityEngine;

public class StateMachineManager : MonoBehaviour {
    Person mainPerson;
    public StateMachine state = StateMachine.Idle;
    public bool stateMachineMoving = false;
    public bool stateMachineTargeting = false;
    public bool stateMachineAttacking = false;
    public bool stateMachineWorking = false;

    void Start() {
        mainPerson = gameObject.GetComponent<Person>();
    }

    void Update() {
        CheckState();
    }

    void CheckState() {
        if ((mainPerson.GetInfo().aiType == AIType.StateMachine || mainPerson.GetInfo().aiType == AIType.QuestAI) && mainPerson.GetInfo().stateMachineDead == false) {
            switch(state) {
                case StateMachine.Idle:
                    if(mainPerson.target) {
                        if(stateMachineTargeting) {
                            ChangeState(StateMachine.Follow);
                        }
                    }
                    
                    if(mainPerson.destination) {
                        if(stateMachineMoving) {
                            ChangeState(StateMachine.Move);
                        } else {
                            // ChangeState(StateMachine.Work);
                        }
                    }
                    break;
                case StateMachine.Move:
                    if(mainPerson.target) {
                        if(stateMachineTargeting) {
                            ChangeState(StateMachine.Follow);
                        }
                    }
                    
                    if(mainPerson.destination) {
                        if(!stateMachineMoving) {
                            ChangeState(StateMachine.Idle);
                        }
                    } else {
                        ChangeState(StateMachine.Idle);
                    }
                    break;
                case StateMachine.Work:
                    break;
                case StateMachine.Follow:
                    if(mainPerson.target) {
                        if(stateMachineTargeting && stateMachineAttacking) {
                            ChangeState(StateMachine.Attack);
                        }
                    }

                    if(mainPerson.destination) {
                        if(!stateMachineTargeting) {
                            ChangeState(StateMachine.Move);
                        }
                    } else {
                        if(!stateMachineTargeting) {
                            ChangeState(StateMachine.Idle);
                        }
                    }
                    break;
                case StateMachine.Attack:
                    if(mainPerson.target) {
                        if(stateMachineTargeting && !stateMachineAttacking) {
                            ChangeState(StateMachine.Follow);
                        }
                    }
                    
                    if(mainPerson.destination) {
                        if(!stateMachineTargeting) {
                            ChangeState(StateMachine.Move);
                        }          
                    } else {
                        if(!stateMachineTargeting) {
                            ChangeState(StateMachine.Idle);
                        }
                    }
                    break;
                case StateMachine.Combat:
                    ChangeState(StateMachine.Idle);
                    break;
                case StateMachine.Hurt:
                    if(!mainPerson.GetInfo().isDead) {
                        if(mainPerson.target) {
                            if(stateMachineTargeting && stateMachineAttacking) {
                                ChangeState(StateMachine.Attack);
                            } else {
                                ChangeState(StateMachine.Follow);
                            }
                        }

                        if(mainPerson.destination) {
                            if(!stateMachineTargeting) {
                                ChangeState(StateMachine.Move);
                            }
                        } else {
                            if(!stateMachineTargeting) {
                                ChangeState(StateMachine.Idle);
                            }
                        }
                    } else {
                        ChangeState(StateMachine.Dead);
                    }
                    break;
                default:
                    break;
            }

            if (mainPerson.isHurt)
            {
                ChangeState(StateMachine.Hurt);
            }

            if(mainPerson.GetInfo().isDead == true && mainPerson.GetInfo().stateMachineDead == false){
                ChangeState(StateMachine.Dead);
            }
        } else if(mainPerson.GetInfo().aiType == AIType.CombatAI) {
            ChangeState(StateMachine.Combat);
        }
    }

    public void ChangeState(StateMachine _state)
    {
        state = _state;
    }
}