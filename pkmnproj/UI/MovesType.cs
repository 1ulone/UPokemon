using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum moveCategory
{
    PHYSICAL,
    SPECIAL,
    STATUS
}

public class MovesType : MonoBehaviour
{
    public class moves
    {
        public string tag, type, category;
        public int pp, power;
        public float accuracy;
    }

    [Header("0 = moves name, 1 = pp current, 2 = pp max")]
    public Text[] uiText; 
    public int movesOptId;

    //[HideInInspector]
    public MovesData data;

    private moves m;

    void Start()
    {
        RefreshMove();
    }

    public void RefreshMove()
    {
        data = MovesetSetter.instances.SetPlayerMoves(movesOptId-1);

        if (data != null)
        {
            m = new moves();
            m.tag = data.movesName;
            m.type = data.typeName;
            m.pp = data.maxPowerPoints;
            m.category = data.movesCategory;
            m.power = data.power;
            m.accuracy = data.accuracy;

            uiText[0].text = m.tag;
            uiText[1].text = m.pp.ToString();
            uiText[2].text = data.maxPowerPoints.ToString();

            this.gameObject.GetComponent<Image>().sprite = SpriteTypeController.instances.GetSprite(m.type.ToUpper());
        }
    }

    public void UseMove()
    {
        AudioController.instances.PlaySfx("aButton");

        m.pp--;
        BattleOptionsController.instances.ResetFightAnim();
        
        BattleSystem.instances.AddMoves(0, movesOptId-1);
        BattleSystem.instances.ChangeState(turnState.BATTLE);
    }
}
