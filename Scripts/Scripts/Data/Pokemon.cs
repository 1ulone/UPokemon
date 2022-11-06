using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
	public PokemonConstantData basedata { get; set; }
	public int level { get; set; } 
	
	public int hp { get; set; }
	public List<Move> moves { get; set; } 

	public Pokemon(PokemonConstantData _base, int _level)
	{
		basedata = _base;
		level = _level;
		hp = MaxHP;
		moves = new List<Move>();
/*
		foreach (var m in basedata.learnablesmove)
		{
			if (m.level <= level)
				moves.Add(new Move(m.move));

			if (moves.Count >= 4)
				break;
		}
		*/
	}

	public int Attack { get { return Mathf.FloorToInt((basedata.attack* level)/ 100f) + 5; } }
	public int Defense { get { return Mathf.FloorToInt((basedata.defense* level)/ 100f) + 5; } }
	public int spAttack { get { return Mathf.FloorToInt((basedata.specialattack* level)/ 100f) + 5; } }
	public int spDefense { get { return Mathf.FloorToInt((basedata.specialdefense* level)/ 100f) + 5; } }
	public int Speed { get { return Mathf.FloorToInt((basedata.speed* level)/ 100f) + 5; } }
	public int MaxHP { get { return Mathf.FloorToInt((basedata.maxhp* level)/ 100f) + 10; } }
}
