using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSpriteController : MonoBehaviour
{
    public static BattleSpriteController instances;

    public SpriteCollection playerCollection,
                            opponentCollection;

    private Dictionary<string, (Sprite, RuntimeAnimatorController, bool)> p_battlespriteDictionary = new Dictionary<string, (Sprite, RuntimeAnimatorController, bool)>();
    private Dictionary<string, Sprite> p_iconDictionary = new Dictionary<string, Sprite>(); 
    private Dictionary<string, LightComponent> p_lightspriteDictionary = new Dictionary<string, LightComponent>();

    
    private Dictionary<string, (Sprite, RuntimeAnimatorController, bool)> o_battlespriteDictionary = new Dictionary<string, (Sprite, RuntimeAnimatorController, bool)>();
    private Dictionary<string, LightComponent> o_lightspriteDictionary = new Dictionary<string, LightComponent>();

    void Start()
    {
        instances = this;

        foreach(BattleSprite bs in playerCollection.battlespriteCollection)
        {
            p_battlespriteDictionary.Add(bs.tag, (bs.pokemonSprite, bs.anim, bs.needLightSource));
            p_iconDictionary.Add(bs.tag, bs.pokemonIcon);
            p_lightspriteDictionary.Add(bs.tag, bs.lightComponent);
        }

        foreach(BattleSprite bs in opponentCollection.battlespriteCollection)
        {
            o_battlespriteDictionary.Add(bs.tag, (bs.pokemonSprite, bs.anim, bs.needLightSource));
            o_lightspriteDictionary.Add(bs.tag, bs.lightComponent);
        }
    }

    public Sprite SetPokemonSprite(string tag, int type)
    {
        if (type == 1) { if (!p_battlespriteDictionary.ContainsKey(tag)) return null; } else
        if (type == 2) { if (!o_battlespriteDictionary.ContainsKey(tag)) return null; }

        return type == 1 ? p_battlespriteDictionary[tag].Item1 : o_battlespriteDictionary[tag].Item1;
    }

    public RuntimeAnimatorController SetPokemonAnim(string tag, int type)
    {
        if (type == 1) { if (!p_battlespriteDictionary.ContainsKey(tag)) return null; } else
        if (type == 2) { if (!o_battlespriteDictionary.ContainsKey(tag)) return null; }
            
        return type == 1 ? p_battlespriteDictionary[tag].Item2 : o_battlespriteDictionary[tag].Item2;
    }

    public bool haveLightsource(string tag, int type)
    {
        if (type == 1) { if (!p_battlespriteDictionary.ContainsKey(tag)) return false; } else
        if (type == 2) { if (!o_battlespriteDictionary.ContainsKey(tag)) return false; }

        return type == 1 ? p_battlespriteDictionary[tag].Item3 : o_battlespriteDictionary[tag].Item3;
    }

    public LightComponent GetLightComponent(string tag, int type)
    {
        if (type == 1) { if (!p_lightspriteDictionary.ContainsKey(tag)) return null; } else 
        if (type == 2) { if (!o_lightspriteDictionary.ContainsKey(tag)) return null; } 

        return type == 1 ? p_lightspriteDictionary[tag] : o_lightspriteDictionary[tag];
    }

    public Sprite GetPokemonIcon(string tag)
    {
        if (!p_iconDictionary.ContainsKey(tag))
            return null;

        return p_iconDictionary[tag];
    }
}
