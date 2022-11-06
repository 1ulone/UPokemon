using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreBattleSystem : MonoBehaviour
{
	[SerializeField] BattleUnit playerUnit;
	[SerializeField] BattleUnit enemyUnit;
	BattleHUD playerHud, enemyHud;

	void Start()
	{
		playerHud = GameObject.FindGameObjectWithTag("playerhud").GetComponent<BattleHUD>();
		enemyHud = GameObject.FindGameObjectWithTag("enemyhud").GetComponent<BattleHUD>();
		BattleSetup();
	}

	void BattleSetup()
	{
		playerUnit.Setup();
		enemyUnit.Setup();
		playerHud.SetPokemon(playerUnit.pokemon);
		enemyHud.SetPokemon(enemyUnit.pokemon);
	}
}
