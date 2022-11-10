using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckTrainerTeam : MonoBehaviour
{
    public Sprite normalCondition, faintedCondition, statusCondition, emptyCondition;

    public Image[] teamLists;
    public int type;

    private TrainerData data;

    void Start()
    {
        data = BattleSystem.instances.GetTrainerData(type);

        foreach(Image s in teamLists)
            s.sprite = emptyCondition; 

        for (int i = 0; i < data.TrainerTeamList.Count; i++)
        {
            if (!data.TrainerTeamList[i].pokemon.fainted) { teamLists[i].sprite = normalCondition; } else 
            if (data.TrainerTeamList[i].pokemon.fainted) { teamLists[i].sprite = faintedCondition; }
        }
    }

    public void RefreshInfo()
    {
        for (int i = 0; i < data.TrainerTeamList.Count; i++)
        {
            if (!data.TrainerTeamList[i].pokemon.fainted) { teamLists[i].sprite = normalCondition; } else 
            if (data.TrainerTeamList[i].pokemon.fainted) { teamLists[i].sprite = faintedCondition; }
        }
    }
}
