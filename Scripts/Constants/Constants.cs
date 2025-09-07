using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Constants
{
    public enum ConditionType
    {
        Health,
        Hunger,
        Stamina
    }

    //Normalized Value
    public const float Normalized_Value_ZERO = 0f;
    public const float Normalized_Value_ONE = 1f;

    //Player Movement Constants
    public const float DEFAULT_MOVEMENT_SPEED = 5f;
    public const float RUN_MOVEMENT_SPEED = 7f;
    public const float DEFAULT_ROTATION_SPEED = 0.1f;
    public const float DEFAULT_JUMP_FORCE = 10f;
    public const float JUMP_STAMINA_CONSUMPTION = 10f;
    public const float RUN_STAMINA_CONSUMPTION = 10f;
    public const float JUMP_STAMINA_RECOVERY_DELAY = 1f;
    public const float MIN_X_LOOK = -60f;
    public const float MAX_X_LOOK = 60f;
    public const float LAY_CHECK_INTERVAL = 0.1f;
    public const float INTERACT_INVOKE_DELAY = 0.5f;
    public const float FOOTSTEP_SOUND_INTERVAL = 0.5f;
    public const float RUN_FOOTSTEP_SOUND_INTERVAL = 0.3f;

    // UI and HUD Constants
    public const float HIT_EFFECT_START_ALPHA = 0.3f;

    // Weapon and Combat Constants
    public const float DEFAULT_ATTACK_DURATION = 2f;
    public const float DEFAULT_WEAPON_DAMAGE = 1f;
    public const float WEAPON_IDLE_MOVE_Y = 0.05f;
    public const float WEAPON_IDLE_DURATION = 1f;
    public const float WEAPON_RAYCAST_DISTANCE = 2f;
    public const float WINDUP_ROTATION_X = -30f;
    public const float WINDUP_POSITION_Y = 0.3f;
    public const float WINDUP_POSITION_Z = -0.1f;
    public const float WINDUP_DURATION_MULTIPLIER = 0.3f;
    public const float SMASH_ROTATION_X = 30f;
    public const float SMASH_ROTATION_Z = -30f;
    public const float SMASH_POSITION_Y = -0.1f;
    public const float SMASH_POSITION_Z = 0.3f;
    public const float SMASH_DURATION_MULTIPLIER = 0.4f;
    public const float RECOVERY_DURATION_MULTIPLIER = 0.3f;

    // Player Sensor Constants
    public const float DEFAULT_SENSOR_DISTANCE = 0.5f;
    public const float SENSOR_RAY_DISTANCE = 3f;
    public const float FOOT_SENSOR_OFFSET = 0.2f;
    public const float FOOT_SENSOR_UP_OFFSET = 0.01f;
    public const float LANDING_CHECK_INTERVAL = 0.1f;

    // Timer and Game Loop Constants
    public const float DEFAULT_START_TIME = 24f;
    public const float TIME_SCALE_DEFAULT = 1f;
    public const float SECONDS_PER_MINUTE = 60f;
    public const float TIMER_TARGET_1 = 18f * 60f + 30f; // 18분 30초
    public const float TIMER_TARGET_2 = 12f * 60f; // 12분
    public const float TIMER_TARGET_3 = 5f * 60f + 15f; // 5분 15초
    public const float TIMER_TARGET_RESET_1 = 18f * 60f + 30f;
    public const float TIMER_TARGET_RESET_2 = 12f * 60f;
    public const float TIMER_TARGET_RESET_3 = 5f * 60f;
    public const int INITIAL_LOOP_COUNT = 0;
    public const int INITIAL_FLOOR_NUM = 0;
    public const float PAUSE_TIME_SCALE = 0f;
    public const int EVENT_FLAG_TRUE = 1;

    // Crafting Constants
    public const float CRAFTING_TIME_MULTIPLIER = 60f;

    // Puzzle 1
    public const string PUZZLE1_CORRECT_ANSWER = "12:12:12";

    // Puzzle 3
    public const string CHEMICAL_CORRECT_FLASK_A = "Flask A";
    public const string CHEMICAL_CORRECT_FLASK_F = "Flask F";
    public const int REQUIRED_BATTERY_COUNT = 3;
    public const int INITIAL_BATTERY_COUNT = 0;
    public const int INITIAL_DROPDOWN_VALUE = 0;

    //Audio Constants
    public const float DEFAULT_BGM_VOLUME = 1f;
    public const float DEFAULT_SFX_VOLUME = 1f;


    // Achievement Constants
    public const int ACHIEVEMENT_RESET_COUNT = 0;

    // Condition Constants
    public const float CONDITION_FADE_DURATION = 0.5f;
    public const float CONDITION_MIN_VALUE = 0f;
    public const float TEST_DAMAGE_VALUE = 20f;

    // Shader Constants
    public const float DISSOLVE_TIME_MULTIPLIER = 0.5f;
    public const float DISSOLVE_OFFSET_BASE = 1.6f;
    public const float DISSOLVE_OFFSET_SUBTRACT = 0.5f;
    public const float DISSOLVE_DEFAULT_VALUE = 0f;
    public const string DISSOLVE_PROPERTY = "_Dissolve";
    public const string DISSOLVE_DIRECTION_PROPERTY = "_DissolveDirection";
    public const string DISSOLVE_OFFSET_PROPERTY = "_DissolveOffest";

    // Screen Constants
    public const float SCREEN_CENTER_X_RATIO = 0.5f;
    public const float SCREEN_CENTER_Y_RATIO = 0.5f;

    // PlayerPrefs Keys
    public const string PREF_KEY_BGM_VOLUME = "BGMVolume";
    public const string PREF_KEY_SFX_VOLUME = "SFXVolume";

    // Interaction Text Constants
    public const string INTERACT_TEXT_DOOR_OPEN = "열기";
    public const string INTERACT_TEXT_DOOR_CLOSE = "닫기";
    public const string INTERACT_TEXT_BATTERY_PICKUP = "배터리 줍기 (E)";
    public const string INTERACT_TEXT_PURIFICATION_START = "정화 퍼즐 시작 (E)";
    public const string INTERACT_TEXT_PUZZLE_START = "퍼즐 시작 (E)";
    public const string INTERACT_TEXT_TIME_REVERSE = "시간 되돌리기 (E)";
    public const string INTERACT_TEXT_PRESS_E_FORMAT = "Press the [E] key: {0}\n{1}";

    // Debug Messages
    public const string DEBUG_BATTERY_COLLECTED = "배터리 습득 완료, 현재 개수: ";
    public const string DEBUG_BATTERY_INSUFFICIENT = "배터리가 부족합니다.";
    public const string DEBUG_PURIFICATION_ACTIVATED = "정화 장치 활성화됨";
    public const string DEBUG_CORRECT_COMBINATION = "정답입니다! 오염 제거 중...";
    public const string DEBUG_WRONG_COMBINATION = "잘못된 조합입니다. 다시 시도하세요.";
    public const string DEBUG_POLLUTION_REMOVED = "오염 물질이 제거되었습니다!";
    public const string DEBUG_TIME_REVERSE_START = "시간 역행 시작";
    public const string DEBUG_ALREADY_PAST_STATE = "이미 과거 상태입니다.";
    public const string DEBUG_PUZZLE_CORRECT = "정답!";
    public const string DEBUG_PUZZLE_WRONG = "오답.";
    public const string DEBUG_CONSOLE_INTERACT = "콘솔과 상호작용했습니다.";
    public const string DEBUG_TIME_REVERSE_INTERACT = "콘솔: 시간 역행 상호작용 시작";
    public const string DEBUG_TARGET_REACHED = "목표 도달";
    public const string DEBUG_LOOP_COUNT = "루프";
    public const string DEBUG_RESTART = "재시작";
    public const string DEBUG_CRAFTING_IMPOSSIBLE = "제작 불가능";
    public const string DEBUG_CRAFTING_COMPLETE = "제작 완료: ";
    public const string DEBUG_ACHIEVEMENT_UNLOCK = "해금";
    public const string DEBUG_ITEM_ACQUIRED = "아이템 획득";
    public const string DEBUG_ON_EQUIP_ITEM = "OnEquipItem";

    // File Path Constants
    public const string DATA_PATH = "Assets/Data/";
    public const string SCRIPTABLE_OBJECTS_PATH = "ScriptableObjects";
    public const string JSON_EXTENSION = "Data.json";

    // Tag Constants
    public const string TAG_UI = "UI";

    // Canvas Constants
    public const string CANVAS_GAME_OBJECT_NAME = "Canvas";

    // Material Constants
    public const string SHADER_DISSOLVE_DIRECTION_METALLIC = "Shader Graphs/Dissolve_Direction_Metallic";
    public const string MATERIAL_PATH_DISSOLVE = "Assets/Resources/Materials/Shader Graphs_Dissolve_Dissolve_Metallic.mat";
    public const string MATERIAL_NAME_DISSOLVE_DIRECTION = "Shader Graphs_Dissolve_Dissolve_Direction_Metallic";

    // Empty String Constants
    public const string EMPTY_STRING = "";
}
