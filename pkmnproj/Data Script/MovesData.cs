using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Moves Data", menuName = "Data/Moves Data")]
public class MovesData : ScriptableObject
{
    [Header("data")]
    public string movesName;
    public string typeName;
    public string movesCategory;
    public string contestType;
    public int maxPowerPoints;
    public int id;

    [Header("stats")]
    public int power;
    public float accuracy; 

    [Header("Optional")]
    public bool changefield = false;
    public bool targetstat = false;
    public bool implantingmove = false;

    [HideInInspector]
    public FieldStatus fieldstatus;
    [HideInInspector]
    public string stattochange;
    [HideInInspector]
    public int statStage;
    [HideInInspector]
    public int implantTurn;
}

[CustomEditor(typeof(MovesData))]
public class MovesDataEditor : Editor
{
    MovesData data;

    void OnEnable()
        => data = (MovesData)target;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (data.changefield)
            data.fieldstatus = (FieldStatus)EditorGUILayout.EnumPopup("Field to change",data.fieldstatus);
        
        if (data.targetstat)
        { 
        	data.stattochange = EditorGUILayout.TextField("target stat", data.stattochange);
        	data.statStage = EditorGUILayout.IntField("Stat Stages", data.statStage);
        }
        
        if (data.implantingmove)
            data.implantTurn = EditorGUILayout.IntField("implant turn", data.implantTurn);
    }
}
