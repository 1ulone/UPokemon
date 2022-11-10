using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSystem : MonoBehaviour
{
    public static StatSystem instances;

    void Awake()
    {
        instances = this;
    }

    public CapturedPokemonData GetActualData(TrainerData actualData, int i)
    {
        CapturedPokemonData d = actualData.TrainerTeamList[i].pokemon;
        int r = Random.Range(1, 3);

        if (d.instanced)
        {
            if (d.onAltForm)
                SwitchAlternateData(d);
            return d;
        }

        if (d.level == 0) { Debug.LogError($"Go Set {d.nickname} Level first"); }

        d.nickname = d.baseData.PokemonName;
        d.HP = d.baseData.HP + statCalculator(d.baseData.HP, d.level);
        d.attack = d.baseData.attack + statCalculator(d.baseData.attack, d.level);
        d.defense = d.baseData.defense + statCalculator(d.baseData.defense, d.level);
        d.specialattack = d.baseData.specialattack + statCalculator(d.baseData.specialattack, d.level);
        d.specialdefense = d.baseData.specialdefense + statCalculator(d.baseData.specialdefense, d.level);
        d.speed = d.baseData.speed + statCalculator(d.baseData.speed, d.level);

        d.health = d.HP;
        d.totalstats = d.HP + d.attack + d.defense + d.specialattack + d.specialdefense + d.speed;

        if (d.baseData.ability[1] != null) { d.ability = r == 1 ? d.baseData.ability[0] : d.baseData.ability[1]; } else 
        { d.ability = d.baseData.ability[0]; }

        d.instanced = true;        

        return d;
    }

    public void SwitchAlternateData(CapturedPokemonData data)
    {
        if (!data.haveAlternateForm)
            return;

        PokemonData tempdata = data.baseData;

        data.baseData = data.altData;
        data.altData = tempdata;

        data.instanced = false;
        data.onAltForm = !data.onAltForm;
    }

    float statCalculator(float a, float b)
        { return Mathf.Round((a / 50.0f) * b);  }

    // public void AddLevel(PokemonData data)
    // {
    //     data
    //     data.HP += (data.HP/50);
    //     data.attack += (data.attack/50);
    //     data.
    // }
}
