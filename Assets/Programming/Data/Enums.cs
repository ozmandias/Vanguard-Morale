// can rename ...Type to ...Set
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
    Combat,
    Work,
    Follow,
    Wait,
    Hurt,
    Dead
}

public enum PlayerCharacter {
    Vanguard,
    Hero,
    Justitcer,
    DeathMage,
    Ghost,
    Viking,
    SciBot,
    Samurai
}

public enum Gender {
    Male,
    Female
}

public enum Faction {
    Fortis,
    Desertus,
    Unmortus,
    Ghostian,
    Damnum,
    Ignis,
    Scientia,
    Karus,
    Forest,
    Storia
}

public enum CircleType {
    Semicircle,
    FullCircle
}

public enum UIType {
    Intro,
    MainMenu,
    Selection,
    Game,
    Speak,
    Shop,
    Skill,
    Effect
}

public enum CombatType {
    Melee,
    Range
}

public enum AIType {
    StateMachine,
    CombatAI,
    BossAI,
    QuestAI
}

public enum EffectType {
    Play,
    Spawn
}

public enum QuestType {
    Kill,
    Collect,
    Talk,
    Travel,
    Protect,
    Destroy
}

public enum RewardType {
    Level,
    Health,
    Damage,
    Strength,
    Agility,
    Intelligence,
    Magic,
    Item,
    Weapon
}