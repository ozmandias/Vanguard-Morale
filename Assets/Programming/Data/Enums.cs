public enum Morality {
    Good = 60,
    Neutral = 50,
    Evil = 40
}

public enum PersonType {
    Friend,
    Normal,
    Enemy,
    Companion,
    Boss
}

public enum StateMachine {
    Idle,
    Move,
    Attack,
    Work,
    Follow,
    Wait,
    Hurt,
    Dead
}

public enum PlayerCharacter {
    MasterKnight,
    Hero
}

public enum Gender {
    Male,
    Female
}

public enum CircleType {
    Semicircle,
    FullCircle
}

public enum UIType {
    MainMenu,
    Game,
    Shop,
    Skill
}

public enum CombatType {
    Melee,
    Range
}

public enum AIType {
    StateMachine,
    CombatAI,
    BossAI
}