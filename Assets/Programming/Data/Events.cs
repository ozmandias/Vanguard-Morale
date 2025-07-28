using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class PlayerCombatEvent : UnityEvent<Enemy> { }
[System.Serializable] public class PlayerCounterEvent : UnityEvent<Enemy> { }

[System.Serializable] public class EnemyStopEvent : UnityEvent<Enemy> { }
[System.Serializable] public class EnemyRetreatEvent : UnityEvent<Enemy> { }
[System.Serializable] public class EnemyHurtEvent : UnityEvent<Enemy> { }