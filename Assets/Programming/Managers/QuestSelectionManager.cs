using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSelectionManager : MonoBehaviour { // for Quests
    [SerializeField] QuestScriptableObject questScriptableObject;
    [SerializeField] QuestSerializable currentQuest;
    List<QuestSerializable> questList;
    Dictionary<string, GameObject> questDictionary = new Dictionary<string, GameObject>();
    public QuestKeyValueSerializable []questKeyValueSerializables;

    public static QuestSelectionManager instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        questScriptableObject = GlobalData.currentKingdomQuestScriptableObject;

        foreach(QuestKeyValueSerializable questKeyValue in questKeyValueSerializables) {
            questDictionary.Add(questKeyValue.key, questKeyValue.value);
        }

        LoadAllQuests();
    }

    void Update() {

    }

    public void LoadAllQuests() {
        // loop questScriptableObject and SetupQuest()
        foreach(QuestSerializable quest in questScriptableObject.dataList) {
            SetupQuest(quest);
        }
    }

    public void AcceptQuest(QuestSerializable questToAccept) {
        questList.Add(questToAccept);
    }

    public void FocusQuest(QuestSerializable questToFocus) {
        currentQuest = questToFocus;
    }

    public void UnfoucsQuest() {
        currentQuest = null;
    }

    public void AbandonQuest(QuestSerializable questToAbandon) {
        questList.Remove(questToAbandon);
    }

    void SetupQuest(QuestSerializable questToSetup) {
        // spawn quest giver in the world
        // spawn quest into UI based on quest type in the world as child of QuestSelectionManager
        // quest will spawn its associated assets in the world
        // get transform locations from spawn manager and spawn at random locations in the range of the length of spawn locations
        // set asset for spawn manager to spawn
        // associate assets of the quests to spawned quests.
        // spawn rewards of the current quest on completing the quests
        for(int i = 0; i < questToSetup.questInfo.Length; i = i + 1) {
            switch(questToSetup.questInfo[i].questType) {
                case QuestType.Kill:
                    SetupKillQuest(questToSetup, i);
                    break;
                case QuestType.Collect:
                    break;
                case QuestType.Talk:
                    break;
                case QuestType.Travel:
                    break;
                case QuestType.Protect:
                    break;
                case QuestType.Destroy:
                    break;
                default:
                    break;
            }
        }
    }

    void SetupKillQuest(QuestSerializable questToSetup, int questInfoLocation) {
        ToKillSpawnSerializable []toKillSpawnSerializables = questToSetup.questInfo[questInfoLocation].toKillSpawnSerializables;
        
        KillQuest killQuest = Instantiate(questDictionary["Kill"], transform).GetComponent<KillQuest>();
        killQuest.questDetails = questToSetup;
        killQuest.questInfoId = questInfoLocation;

        for(int i = 0; i < toKillSpawnSerializables.Length; i = i + 1) {
            toKillSpawnSerializables[i].spawnTransforms = SpawnManager.instance.spawnLocations;
            for(int j = 0; j < toKillSpawnSerializables[i].spawnCount; j = j + 1) {
                GameObject toKillObject = Instantiate(
                    toKillSpawnSerializables[i]
                    .assetObject,
                    toKillSpawnSerializables[i]
                    .spawnTransforms[Random.Range(0, toKillSpawnSerializables[i].spawnTransforms.Length)]
                    .position,
                    Quaternion.identity
                );
                toKillObject.GetComponent<QuestManager>().questBehaviourSerializable = toKillSpawnSerializables[i].behaviourSerializable;
                killQuest.toKillList.Add(toKillObject);
                
                // associate object with quest action
                QuestManager toKillQuestManager = toKillObject.GetComponent<QuestManager>();
                toKillQuestManager.mainQuest = killQuest;
            }
        }
    }
}