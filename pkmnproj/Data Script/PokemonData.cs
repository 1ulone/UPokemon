using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pokemon Data", menuName = "Data/Pokemon Data")]
public class PokemonData : ScriptableObject
{
    public int SinnohDex;
    public int NationalDex;
    public int Level = 5;
    public string PokemonName;
    public float health;

    [Header("Base Stats")]
    public int HP;
    public int attack;
    public int defense;
    public int specialattack;
    public int specialdefense;
    public int speed;
    public int totalstats;

    [Header("Type and Ability")]
    public string[] type = new string[2];
    public string[] ability = new string [2];
    public string hiddenAbility;

    [Header("Pokedex Info")]
    public decimal mass;
    public string evWorth;
    public int expv;
    public int hatchStep;
    public string[] eggGroup = new string[2];
}
