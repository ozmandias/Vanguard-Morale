using UnityEngine;

public class ArenaManager : MonoBehaviour {
    Vanguard vanguardPlayer;
    Vanguard vanguardPerson;

    void Start() {
        CreateArena();
    }

    void Update() {
        CheckArena();
    }

    void CreateArena() {
        // get two vanguard data from global data
    }

    void CheckArena() {
        // check which Vanguard is dead

        // if there is death, declare winner, end arena and display arena winner ui
    }
}