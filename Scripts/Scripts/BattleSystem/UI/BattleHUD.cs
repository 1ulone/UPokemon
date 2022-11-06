using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
	[SerializeField] Text nameT;
	[SerializeField] Text levelT;
	[SerializeField] HealthUI hp;

	public void SetPokemon(Pokemon p)
	{
		nameT.text = p.basedata.Pokemonname.ToUpper();
		levelT.text = p.level.ToString();
		hp.SetHP((float) p.hp / p.MaxHP);
	}
}


