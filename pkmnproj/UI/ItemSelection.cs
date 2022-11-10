using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour
{
    public Text uititle, uiamount;
    public ItemData data;

    void Start()
    {
        uititle.text = data.itemName;
        uiamount.text = $"X {data.amount.ToString("F0")}";
    }

    public void OnPressed()
        => BagController.instances.GetTargetItem(this);

    public void RefreshInfo()
        => uiamount.text = $"X {data.amount.ToString("F0")}";
}
