using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesetSetter : MonoBehaviour
{
    public static MovesetSetter instances;
    public static MovesData nulldata;

    public MovesData nulldatapre;
    private Dictionary<string, MovesData> nullMovesList = new Dictionary<string, MovesData>();

    void Awake()
    {
        instances = this;
        nulldata = nulldatapre;
    }

    public CapturedPokemonData SetMoves(TrainerData data)
    {
        MovesData[] setMoveset = data.TrainerTeamList[0].moveset;
        CapturedPokemonData setPokemon = data.TrainerTeamList[0].pokemon;

        for (int i = 0; i < setMoveset.Length; i++)
            setPokemon.moveset[i] = setMoveset[i];

        return setPokemon;
    }

    public MovesData SetPlayerMoves(int id)
    {
        CapturedPokemonData data = SetMoves(BattleSystem.instances.playerData);

        if (!data.moveset[id])
            return null;

        return data.moveset[id];
    }

    public void ResetMoves(TrainerData pdata, TrainerData odata)
    {
        if (nullMovesList.Count <= 0)
            return;

		int ind = 0;

        foreach(string s in nullMovesList.Keys)
        {
			if (pdata.TrainerTeamList[0].pokemon.nickname.ToUpper() == s.ToUpper() && odata.TrainerTeamList[0].pokemon.nickname.ToUpper() == s.ToUpper())
				{ ind = 3; }
            if (pdata.TrainerTeamList[0].pokemon.nickname.ToUpper() == s.ToUpper())
                { ind = 1; } else 
            if (odata.TrainerTeamList[0].pokemon.nickname.ToUpper() == s.ToUpper())
                { ind = 2; }
        }

        if (ind == 1)
        {
            string n = pdata.TrainerTeamList[0].pokemon.nickname;

            for (int i = 0; i < pdata.TrainerTeamList[0].moveset.Length; i++)
            {    
                if (pdata.TrainerTeamList[0].moveset[i] == nulldata)
                {
                    pdata.TrainerTeamList[0].moveset[i] = nullMovesList[n];
                    nullMovesList.Remove(n);   
                }
            }
        } else
        if (ind == 2) 
        {
            string n = odata.TrainerTeamList[0].pokemon.nickname;

            for (int i = 0; i < odata.TrainerTeamList[0].moveset.Length; i++)
            {
                if (odata.TrainerTeamList[0].moveset[i] == nulldata)
                {
                    odata.TrainerTeamList[0].moveset[i] = nullMovesList[n];
                    nullMovesList.Remove(n);
                }
            }
        } else 
        if (ind == 3)
        {
			string np = pdata.TrainerTeamList[0].pokemon.nickname;
			string no = pdata.TrainerTeamList[0].pokemon.nickname;

			for (int i = 0; i < pdata.TrainerTeamList[0].moveset.Length; i++)
			{
				if (pdata.TrainerTeamList[0].moveset[i] == nulldata)
				{
					pdata.TrainerTeamList[0].moveset[i] = nulldata;
					nullMovesList.Remove(np);
				}
			}

			for (int i = 0; i < odata.TrainerTeamList[0].moveset.Length; i++)
			{
				if (odata.TrainerTeamList[0].moveset[i] == nulldata)
				{
					odata.TrainerTeamList[0].moveset[i] = nulldata;
					nullMovesList.Remove(no);
				}
			}
		}
        
    }

    public void AddNullMoves(string tag, MovesData data)
        => nullMovesList.Add(tag, data);
}
