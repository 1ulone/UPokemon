using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon Data", menuName = "Data/Pokemon")]
public class PokemonConstantData : ScriptableObject
{
	[SerializeField] public string Pokemonname;

	[TextArea]
	[SerializeField] public string description;
	[SerializeField] public int sinnohDex;
	[SerializeField] public int nationalDex;

	[SerializeField] public pokemonType type1;
	[SerializeField] public pokemonType type2;

	[SerializeField] public string ability1;
	[SerializeField] public string ability2;

	[Header("Appearance")]
	[SerializeField] public RuntimeAnimatorController frontSprite;
	[SerializeField] public RuntimeAnimatorController backSprite;
	[SerializeField] public RuntimeAnimatorController shFrontSprite;
	[SerializeField] public RuntimeAnimatorController shBackSprite;
	
	[SerializeField] public bool dolight;
	[SerializeField] public bool canmega;

	[Header("Base Stats")]
	[SerializeField] public int maxhp;
	[SerializeField] public int attack;
	[SerializeField] public int defense;
	[SerializeField] public int specialattack;
	[SerializeField] public int specialdefense;
	[SerializeField] public int speed;
	[SerializeField] public int totalstats;

	[SerializeField] public List<learnableMove> learnablesmove;
/*
	public string pname { get { return Pokemonname; } }
	public string description { get { return Description; } }
	public int sinnohdex { get { return SinnohDex; } }
	public int nationaldex { get { return NationalDex; } }
	public pokemonType type1 { get { return Type1; } }
	public pokemonType type2 { get { return Type2; } }	
	public RuntimeAnimatorController frontSprite { get { return fSprite; } }
	public RuntimeAnimatorController backSprite { get { return bSprite; } }
	public RuntimeAnimatorController shFrontSprite { get { return sfSprite; } }
	public RuntimeAnimatorController shBackSprite { get { return sbSprite; } }
	public bool dolight { get { return DoLighting; } }
	public bool canmega { get { return CanMegaEvolve; } }
	public int maxhp { get { return Maxhp; } }
	public int attack { get { return Attack; } }
	public int defense { get { return Defense; } } 
	public int specialattack { get { return Specialattack; } }
	public int specialdefense { get { return Specialdefense; } }
	public int speed { get { return Speed; } }
	public List<learnableMove> learnablesmove { get { return LearnableMove; } }
*/
}

[System.Serializable]
public class learnableMove
{
	public MoveData move;
	public int level;
}

public enum pokemonType
{
	none,
	bug,
	dark,
	dragon,
	electric,
	fairy,
	fighting,
	fire,
	flying,
	ghost,
	grass,
	ground,
	ice,
	normal,
	poison,
	psychic,
	rock,
	steel,
	water
}
