using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
	[SerializeField] GameObject health;

	public void SetHP(float hpnorm)
		=> health.GetComponent<Image>().fillAmount = hpnorm;
}
