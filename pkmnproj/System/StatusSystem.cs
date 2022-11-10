using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusAilments
{
    Null,
    Burned,
    Frozen,
    Paralyzed,
    Poisoned,
    BadlyPoison,
    Sleep,
    Volatile,
    Confused,
    Bound,
    Implanted
}

public class StatusSystem : MonoBehaviour
{
    public static StatusSystem instances;

    [System.Serializable]
    public class statusMove
    {
        public string movesName;
        public StatusAilments movesType;
        public float  accuracy;
    }

    [System.Serializable]
    public class statusSprite
    {
        public StatusAilments tag;
        public Sprite sprite;   
    }

    public List<statusSprite> statusSpriteList;
    public List<statusMove> statusMoveList;

    private Dictionary<StatusAilments, Sprite> statusSpriteDictionary = new Dictionary<StatusAilments, Sprite>();
    private Dictionary<string, statusMove> statusMoveDictionary = new Dictionary<string, statusMove>();

    void Awake()
        => instances = this;

    void Start()
    {
        foreach(statusSprite ss in statusSpriteList)
            statusSpriteDictionary.Add(ss.tag, ss.sprite);

        foreach(statusMove sm in statusMoveList)
            statusMoveDictionary.Add(sm.movesName.ToUpper(), sm);
    }

    public (bool d, StatusAilments s, int a) CheckMoves(string tag)
    {
        if (!statusMoveDictionary.ContainsKey(tag.ToUpper()))
            return (false, StatusAilments.Null, 0);

        bool res = false;
        int r = RandomNumberGenerator.instances.CheckGetStatus(statusMoveDictionary[tag.ToUpper()].accuracy);
        StatusAilments mt = statusMoveDictionary[tag.ToUpper()].movesType;

        if (r > 0)
            { res = true; }

        return (res, mt, r);
    }

    public Sprite getStatusSprite(StatusAilments tag)
    {
        if (!statusSpriteDictionary.ContainsKey(tag))
            return null;

        return statusSpriteDictionary[tag];
    }

    public void StatusEffect(CapturedPokemonData data, StatusAilments status)
    {
        switch (status)
        {
            case StatusAilments.Null: { return; }
            case StatusAilments.Paralyzed: { data.speed *= 0.5f; } break;
        }
    }

    public void EndTurnEffect(CapturedPokemonData data, StatusAilments status, HealthUI h)
    {
        switch (status)
        {
            case StatusAilments.Null: { return; }
            case StatusAilments.Burned: { h.DoDamage(data.HP * (1f/16.0f)); TextSystem.instance.StartDialogue($"{data.nickname} is hurt by burn!"); h.AnimateStatusEff(status); } break;
            case StatusAilments.Poisoned: { h.DoDamage(data.HP * (1f/8.0f)); TextSystem.instance.StartDialogue($"{data.nickname} is hurt by poison!"); h.AnimateStatusEff(status); } break;
            case StatusAilments.BadlyPoison: { data.badlypoisoncounter++; h.DoDamage(data.HP * (data.badlypoisoncounter/8.0f)); h.AnimateStatusEff(StatusAilments.Poisoned); } break;
//            case StatusAilments.Implanted: { h.DoDamage(); }
        }
    }
    
}
