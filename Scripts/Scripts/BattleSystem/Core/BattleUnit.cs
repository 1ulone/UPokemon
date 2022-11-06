using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
	[SerializeField] PokemonConstantData basedat;
	[SerializeField] int level;
	[SerializeField] bool isPlayerUnit;

	public Pokemon pokemon { get; set; }

	public void Setup()
	{
		pokemon = new Pokemon(basedat, level);
		if (isPlayerUnit)
			GetComponent<Animator>().runtimeAnimatorController = pokemon.basedata.backSprite;
		else 
			GetComponent<Animator>().runtimeAnimatorController = pokemon.basedata.frontSprite;
	}
}
