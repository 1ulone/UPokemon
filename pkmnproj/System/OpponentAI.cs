using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentAI : MonoBehaviour
{
    public static OpponentAI instances;

    void Start()
    {
        instances = this;
    }

    public void OpponentSetMoves()
    {
        CapturedPokemonData currentPokemon = BattleSystem.instances.opponentPokemon;
        int r = Random.Range(0, currentPokemon.moveset.Length);

        BattleSystem.instances.AddMoves(1, 1);
    }

    public void OpponentSwitchPokemon(int toSwitch)
    {
        CheckTrainerTeam[] ct = GameObject.FindObjectsOfType<CheckTrainerTeam>();

        foreach(CheckTrainerTeam c in ct)
            c.RefreshInfo();

        StartCoroutine(BattleSystem.instances.SwitchingPokemon(2, toSwitch, 0));
    }
}
