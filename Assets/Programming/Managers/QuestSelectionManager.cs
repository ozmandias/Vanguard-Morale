using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSelectionManager : MonoBehaviour { // for Quests
    [SerializeField] QuestScriptableObject questScriptableObject;
    [SerializeField] QuestModel currentQuest;
    List<QuestModel> questList;
    Dictionary<string, GameObject> questDictionary = new Dictionary<string, GameObject>();
    public QuestKeyValueModel []questKeyValueModels;

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

        foreach(QuestKeyValueModel questKeyValue in questKeyValueModels) {
            questDictionary.Add(questKeyValue.key, questKeyValue.value);
        }

        LoadAllQuests();
    }

    void Update() {

    }

    public void LoadAllQuests() {
        // loop questScriptableObject and SetupQuest()
        foreach(QuestModel quest in questScriptableObject.dataList) {
            SetupQuest(quest);
        }
    }

    public void AcceptQuest(QuestModel questToAccept) {
        questList.Add(questToAccept);
    }

    public void FocusQuest(QuestModel questToFocus) {
        currentQuest = questToFocus;
    }

    public void UnfoucsQuest() {
        currentQuest = null;
    }

    public void AbandonQuest(QuestModel questToAbandon) {
        questList.Remove(questToAbandon);
    }

    void SetupQuest(QuestModel questToSetup) {
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

    void SetupKillQuest(QuestModel questToSetup, int questInfoLocation) {
        ToKillSpawnModel []toKillSpawnModels = questToSetup.questInfo[questInfoLocation].toKillSpawnModels;
        
        KillQuest killQuest = Instantiate(questDictionary["Kill"], transform).GetComponent<KillQuest>();
        killQuest.questDetails = questToSetup;
        killQuest.questInfoId = questInfoLocation;

        for(int i = 0; i < toKillSpawnModels.Length; i = i + 1) {
            toKillSpawnModels[i].spawnTransforms = SpawnManager.instance.spawnLocations;
            for(int j = 0; j < toKillSpawnModels[i].spawnCount; j = j + 1) {
                GameObject toKillObject = Instantiate(
                    toKillSpawnModels[i]
                    .assetObject,
                    toKillSpawnModels[i]
                    .spawnTransforms[Random.Range(0, toKillSpawnModels[i].spawnTransforms.Length)]
                    .position,
                    Quaternion.identity
                );
                toKillObject.GetComponent<QuestManager>().questBehaviourModel = toKillSpawnModels[i].behaviourModel;
                killQuest.toKillList.Add(toKillObject);
                
                // associate object with quest action
                QuestManager toKillQuestManager = toKillObject.GetComponent<QuestManager>();
                toKillQuestManager.mainQuest = killQuest;
            }
        }
    }
}