using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FieldStatus
{
	NULL, 
    Sunny,
    ExtremeSunny,
    Hail,
    DiamondDust,
    Raining,
    HeavyRain,
    Sandstorm,
    Fog,
    StrongWinds,
    TrickRoom,
    MagicRoom,
    WonderRoom,
    ElectricTerrain,
    GrassyTerrain,
    MistyTerrain,
    PsychicTerrain
}

public enum moveType
{
    StatusChange,
    ChangeField,
    Implanting,
    Steal,
    Disable,
    Protect,
    Transform
}

[System.Serializable]
public class statusMove
{
    public MovesData data;
    public moveType type;
    public string specificTarget;
}

public class StatusMoveController : MonoBehaviour
{
    public static StatusMoveController instances;

    public List<statusMove> moveList;   
    private Dictionary<string, (moveType, string, MovesData)> moveDictionary = new Dictionary<string, (moveType, string, MovesData)>();

    void Awake()
        => instances = this;
    
    void Start()
    {
        foreach(statusMove sm in moveList)
            moveDictionary.Add(sm.data.movesName.ToUpper(), (sm.type, sm.specificTarget.ToUpper(), sm.data));
    }

    public void DoStatusMove(string tag, CapturedPokemonData target, CapturedPokemonData secondtarget = null)
    {
        if (!moveDictionary.ContainsKey(tag.ToUpper()))
            return;
        
        switch (moveDictionary[tag.ToUpper()].Item1)
        {
			case moveType.StatusChange : { ChangeStatus(target, moveDictionary[tag.ToUpper()].Item3); } break;
            case moveType.ChangeField  : { EnvironmentStatus(moveDictionary[tag.ToUpper()].Item3.fieldstatus); } break;
            case moveType.Implanting   : { ImplantMove(target); } break;
            case moveType.Steal        : { StealItem(target); } break;
            case moveType.Disable      : { DisableTarget(target); } break;
            case moveType.Protect      : { Protect(target); } break;
            case moveType.Transform    : { TransformCopy(target, secondtarget); } break;
        }
    }
		
	void ChangeStatus(CapturedPokemonData target, MovesData move)
	{
		int stage = move.statStage;
		float mult = stage<0 ? 2/(Mathf.Abs(stage)+2) : (stage+2)/2;
		string targetstat = move.stattochange;

		switch(targetstat.ToLower())
		{
			case "attack": { target.attack *= mult; } break;
			case "defense": { target.defense *= mult; } break;
			case "specialattack": { target.specialattack *= mult; } break;
			case "specialdefense": { target.specialdefense *= mult; } break;
			case "speed": { target.speed *= mult; } break;
		}
	}

    void EnvironmentStatus(FieldStatus fieldToChange)
    {
    	if (fieldToChange == BattleSystem.currentFieldStatus)
			return;

		BattleSystem.currentFieldStatus = fieldToChange;
		BattleSystem.instances.AddCountdownTag("Environment", Random.Range(3, 6));
    }

    void ImplantMove(CapturedPokemonData target)
    {
		target.status = StatusAilments.Implanted;
    }

    void StealItem(CapturedPokemonData target)
    {
        // target.helditem = 
    }

    void DisableTarget(CapturedPokemonData target)
    {
        if (target.lastusedmoves == -1)
            return;
        
        MovesetSetter.instances.AddNullMoves(target.nickname, target.moveset[target.lastusedmoves]);
        target.moveset[target.lastusedmoves] = MovesetSetter.nulldata;
        target.lastusedmoves = -1;
        BattleSystem.instances.AddCountdownTag("Disable", Random.Range(3, 6));
    }

    void Protect(CapturedPokemonData target)
    {
        
    }

    void TransformCopy(CapturedPokemonData target, CapturedPokemonData secondtarget)
    {
        HealthUI[] ui = GameObject.FindObjectsOfType<HealthUI>();

        target.baseData = secondtarget.baseData;
        target.instanced = false;

        StatSystem.instances.GetActualData(BattleSystem.instances.GetTrainerData(1), 0);
        foreach(HealthUI i in ui)
            i.RefreshPokemon();
    }
}
