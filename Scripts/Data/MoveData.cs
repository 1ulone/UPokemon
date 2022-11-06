using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Move", menuName = "Data/Move")]
public class MoveData : ScriptableObject
{
	[SerializeField] string Movename;

	[TextArea]
	[SerializeField] string Description;

	[SerializeField] int PP;
	[SerializeField] int Accuracy;
	[SerializeField] int Power;
	[SerializeField] pokemonType Type;

	public string movename { get { return Movename; } }
	public string description { get { return Description; } }
	public int pp { get { return PP; } }
	public int accuracy { get { return Accuracy; } }
	public int power { get { return Power; } }
	public pokemonType type { get { return Type; } }
}
