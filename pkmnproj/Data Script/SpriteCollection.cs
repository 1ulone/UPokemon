using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleSprite
{
    public string tag;
    public Sprite pokemonSprite;
    public Sprite pokemonIcon;
    public RuntimeAnimatorController anim;
    public bool needLightSource;

    public LightComponent lightComponent;
}

[System.Serializable]
public class LightComponent
{
    public Color color;
    public float intensity;
    public float range;
    public Vector3 position;
}

[CreateAssetMenu(fileName = "New Sprite Collection", menuName = "Collection/Sprite Collection")]
public class SpriteCollection : ScriptableObject
{
    

    public List<BattleSprite> battlespriteCollection;
}
