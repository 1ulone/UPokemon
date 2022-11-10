using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string itemType = "RECOVERY";
    public Sprite icon;
    public string flavorText;
    public int amount;

    [HideInInspector]
    public float healamount;
    [HideInInspector]
    public float catchrate;
}

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    ItemData data;

    void OnEnable()
    {
        data = (ItemData)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        switch (data.itemType.ToUpper())
        {
            case "RECOVERY":
            {
                data.healamount = EditorGUILayout.FloatField("Item heal amount", 20.0f);
            } break;

            case "POKEBALL":
            {
                data.catchrate = EditorGUILayout.FloatField("Pokeball catch rate", 75.0f);
            } break;
        }
    }
}
