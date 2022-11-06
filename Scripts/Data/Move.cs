using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move 
{
	MoveData basedata { get; set; }
	int pp { get; set; }

	public Move(MoveData data)
	{
		basedata = data;
		pp = basedata.pp;
	}
}
