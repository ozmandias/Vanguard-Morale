using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour {
    public static AIManager instance;

    public List<NavMeshAgent> soldierAIList;
    public List<NavMeshAgent> personAIList;
    public List<NavMeshAgent> enemyAIList;

    public float RadiusAroundTarget = 8f;
    public float DistanceAroundDestination = 8f;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    public void AddToList(PersonType personType, NavMeshAgent aiAgent) {
        switch(personType) {
            case PersonType.Friend:
                soldierAIList.Add(aiAgent);
                break;
            case PersonType.Enemy:
                enemyAIList.Add(aiAgent);
                break;
            default:
                personAIList.Add(aiAgent);
                break;
        }
    }

    public void RemoveFromList(PersonType personType, NavMeshAgent aiAgent) {
        switch(personType) {
            case PersonType.Friend:
                soldierAIList.Remove(aiAgent);
                break;
            case PersonType.Enemy:
                enemyAIList.Remove(aiAgent);
                break;
            default:
                personAIList.Remove(aiAgent);
                break;
        }
    }

    public bool ListsContain(NavMeshAgent aiAgent) {
        if(soldierAIList.Contains(aiAgent) || personAIList.Contains(aiAgent) || enemyAIList.Contains(aiAgent)) {
            return true;
        }
        return false;
    }

    public void AgentCircleTarget(PersonType personType, NavMeshAgent aiAgent, Transform targetTransform, CircleType circleType) {
        List<NavMeshAgent> aiList;
        int circleMultiplier = circleType == CircleType.FullCircle ? 2 : 1;
        int agentIndex = -1;

        switch(personType) {
            case PersonType.Friend:
                aiList = soldierAIList;
                break;
            case PersonType.Enemy:
                aiList = enemyAIList;
                break;
            default:
                aiList = personAIList;
                break;
        }
        agentIndex = aiList.IndexOf(aiAgent);

        if(agentIndex != -1) {
            Vector3 circleDestination = new Vector3(
                targetTransform.position.x + RadiusAroundTarget * Mathf.Cos(circleMultiplier * Mathf.PI * agentIndex / aiList.Count),
                targetTransform.position.y,
                targetTransform.position.z + RadiusAroundTarget * Mathf.Sin(circleMultiplier * Mathf.PI * agentIndex / aiList.Count)
            );
            aiAgent.SetDestination(circleDestination);
        }
    }

    public void AgentRepositionAtDestination(PersonType personType, NavMeshAgent aiAgent, Transform destinationTransform) {
        List<NavMeshAgent> aiList;
        int agentIndex = -1;

        switch(personType) {
            case PersonType.Friend:
                aiList = soldierAIList;
                break;
            case PersonType.Enemy:
                aiList = enemyAIList;
                break;
            default:
                aiList = personAIList;
                break;
        }
        agentIndex = aiList.IndexOf(aiAgent);

        if(agentIndex != -1 /*&& agentIndex < 5*/) {
            Vector3 reposition = new Vector3(
                destinationTransform.position.x + DistanceAroundDestination * Mathf.Cos(Mathf.PI * agentIndex / aiList.Count),
                destinationTransform.position.y,
                destinationTransform.position.z + DistanceAroundDestination * Mathf.Sin(Mathf.PI * agentIndex / aiList.Count)
            );
            aiAgent.SetDestination(reposition);
        } /*else {
            Debug.Log("Move to next line of position");
        }*/
    }

    public void AgentsCircleTarget(PersonType personType, Transform targetTransform, CircleType circleType) {
        List<NavMeshAgent> aiList;
        int circleMultiplier = circleType == CircleType.FullCircle ? 2 : 1;

        switch(personType) {
            case PersonType.Friend:
                aiList = soldierAIList;
                break;
            case PersonType.Enemy:
                aiList = enemyAIList;
                break;
            default:
                aiList = personAIList;
                break;
        }

        for(int i = 0; i < aiList.Count; i = i + 1) {
            Vector3 circleDestination = new Vector3(
                targetTransform.position.x + RadiusAroundTarget * Mathf.Cos(circleMultiplier * Mathf.PI * i / aiList.Count),
                targetTransform.position.y,
                targetTransform.position.z + RadiusAroundTarget * Mathf.Sin(circleMultiplier * Mathf.PI * i / aiList.Count)
            );
            aiList[i].SetDestination(circleDestination);
        }
    }

    public void ListCircleTarget(List<NavMeshAgent> aiList, Transform targetTransform, CircleType circleType) {
        int circleMultiplier = circleType == CircleType.FullCircle ? 2 : 1;
        for(int i = 0; i < aiList.Count; i = i + 1) {
            Vector3 circleDestination = new Vector3(
                targetTransform.position.x + RadiusAroundTarget * Mathf.Sin(circleMultiplier * Mathf.PI * i / aiList.Count),
                targetTransform.position.y,
                targetTransform.position.z + RadiusAroundTarget * Mathf.Sin(circleMultiplier * Mathf.PI * i / aiList.Count)
            );
            aiList[i].SetDestination(circleDestination);
        }
    }

    void OnGUI() {
        /*if(GUI.Button(new Rect(10, 10, 150, 50), "AI To Target")) {
            foreach(Friend soldier in GameManager.instance.soldierList) {
                if(soldier.target) AIManager.instance.AgentCircleTarget(soldier.personInfo.personType, soldier.personAgent, soldier.target.transform, CircleType.Semicircle);
            }

            foreach(Enemy enemy in GameManager.instance.enemyList) {
                if(enemy.target) AIManager.instance.AgentCircleTarget(enemy.personInfo.personType, enemy.personAgent, enemy.target.transform, CircleType.Semicircle);
            }
        }

        if(GUI.Button(new Rect(10, 80, 150, 50), "AI To Destination")) {
            Debug.Log("soldierDestination - tansform.forward: " + GameManager.instance.soldierDestination.forward);
            foreach(Friend soldier in GameManager.instance.soldierList) {
                AIManager.instance.AgentRepositionAtDestination(soldier.personInfo.personType, soldier.personAgent, soldier.destination);
            }

            Debug.Log("enemyDestination - transform.forward: " + GameManager.instance.enemyDestination.forward);
            foreach(Enemy enemy in GameManager.instance.enemyList) {
                AIManager.instance.AgentRepositionAtDestination(enemy.personInfo.personType, enemy.personAgent, enemy.destination);
            }
        }*/
    }
}