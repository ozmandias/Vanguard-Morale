using UnityEngine;

public class StateMachineChanger : MonoBehaviour {
    Person mainPerson;
    public StateMachine state = StateMachine.Init;
    public bool stateMachineReady = false;
    public bool stateMachineMoving = false;
    public bool stateMachineTargeting = false;
    public bool stateMachineAttacking = false;
    public bool stateMachineWorking = false;
    public bool stateMachineDead = false;

    void Start() {
        mainPerson = gameObject.GetComponent<Person>();
    }

    void Update() {
        CheckState();
    }

    void CheckState() {
        if ((mainPerson.personAI.aiType == AIType.StateMachine || mainPerson.personAI.aiType == AIType.QuestAI) && mainPerson.personState.stateMachineDead == false) {
            switch(state) {
                case StateMachine.Init:
                    if(stateMachineReady) {
                        ChangeState(StateMachine.Idle);
                    }
                    break;
                case StateMachine.Idle:
                    if(mainPerson.target) {
                        if(stateMachineTargeting) {
                            ChangeState(StateMachine.Follow);
                        }
                    }
                    
                    if(mainPerson.destination != null) {
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
                    
                    if(mainPerson.destination != null) {
                        if(!stateMachineMoving) {
                            ChangeState(StateMachine.Idle);
                        }
                    } else {
                        ChangeState(StateMachine.Idle);
                    }
                    break;
                case StateMachine.Work:
                    if(mainPerson.target) {
                        if(stateMachineTargeting) {
                            ChangeState(StateMachine.Follow);
                        }
                    }
                    break;
                case StateMachine.Follow:
                    if(mainPerson.target) {
                        if(stateMachineTargeting && stateMachineAttacking) {
                            ChangeState(StateMachine.Attack);
                        }
                    }

                    if(mainPerson.destination != null) {
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
                    
                    if(mainPerson.destination != null) {
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

                        if(mainPerson.destination != null) {
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

            if(mainPerson.GetInfo().isDead == true && mainPerson.personState.stateMachineDead == false){
                ChangeState(StateMachine.Dead);
            }
        } else if(mainPerson.personAI.aiType == AIType.CombatAI) {
            ChangeState(StateMachine.Combat);
        }
    }

    public void ChangeState(StateMachine _state)
    {
        state = _state;
    }
}