using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

[System.Serializable]
public class battleAnimation
{
    public string tag;
    public PlayableAsset animation;
}

[CreateAssetMenu(fileName = "New Animation Collection", menuName = "Collection/Animation Collection")]
public class AnimationCollection : ScriptableObject
{
    public Dictionary<string, PlayableAsset> collection = new Dictionary<string, PlayableAsset>();   
    public bool instanced = false;
}
