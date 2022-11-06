using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFollowParent : MonoBehaviour
{
	[SerializeField] Text parT;
	Text t;

	void Start()
		=> t = GetComponent<Text>();

	void Update()
	{
		if (t.text != parT.text)
			t.text = parT.text;
	}
}
