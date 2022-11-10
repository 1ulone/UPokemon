using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Captured Pokemon Data", menuName = "Data/Captured Pokemon Data")]
public class CapturedPokemonData : ScriptableObject
{
    public string nickname;
    public PokemonData baseData;

    public int level;
    public float health;

    public float HP;
    public float attack;
    public float defense;
    public float specialattack;
    public float specialdefense;
    public float speed;
    public float totalstats;

    public StatusAilments status;

    public bool instanced;
    public bool fainted;
    public bool haveAlternateForm = false;

    public string ability;

    public MovesData[] moveset = new MovesData[4];

    [HideInInspector]
    public PokemonData altData;
    [HideInInspector]
    public bool onAltForm = false;
    [HideInInspector]
    public bool canMega = false;
    [HideInInspector]
    public int badlypoisoncounter = 0;
    [HideInInspector]
    public int lastusedmoves = -1;
}

[CustomEditor(typeof(CapturedPokemonData))]
public class CapturedPokemonDataEditor : Editor
{
    CapturedPokemonData data;

    void OnEnable()
    {
        data = (CapturedPokemonData)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (data.haveAlternateForm)
        {
            data.altData = EditorGUILayout.ObjectField("alternate data", data.altData, typeof(PokemonData), true) as PokemonData;
            data.onAltForm = EditorGUILayout.Toggle("On Alternate Form", data.onAltForm);
            data.canMega = EditorGUILayout.Toggle("Can Mega", data.canMega);
        }
    }
}
