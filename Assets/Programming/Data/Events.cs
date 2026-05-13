using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[System.Serializable] public class PlayerReadyEvent : UnityEvent<PlayerCharacter, GameObject> { }
[System.Serializable] public class WarReadyEvent: UnityEvent<Transform, Transform> { }

[System.Serializable] public class PlayerMovementEvent : UnityEvent<Enemy> { }
[System.Serializable] public class PlayerCombatEvent : UnityEvent<Enemy> { }
[System.Serializable] public class PlayerCounterEvent : UnityEvent<Enemy> { }

[System.Serializable] public class EnemyStartEvent : UnityEvent<Enemy> { }
[System.Serializable] public class EnemyStopEvent : UnityEvent<Enemy> { }
[System.Serializable] public class EnemyRetreatEvent : UnityEvent<Enemy> { }
[System.Serializable] public class EnemyHurtEvent : UnityEvent<Enemy> { }

[System.Serializable] public class AgentsCircleEvent : UnityEvent<List<NavMeshAgent>, Transform, CircleType> { }

[System.Serializable] public class CombatAISetupEvent: UnityEvent { }