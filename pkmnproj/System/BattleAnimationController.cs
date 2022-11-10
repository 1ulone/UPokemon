using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class BattleAnimationController : MonoBehaviour
{
    public static BattleAnimationController instances;

    public AnimationCollection playerCollection,   
                               opponentCollection,
                               sceneCollection;

    public List<battleAnimation> playerAnimationLists;
    public List<battleAnimation> opponentAnimationLists;
    public List<battleAnimation> sceneAnimationLists;

    private PlayableDirector director;

    void Start()
    {
        instances = this;
        director = GetComponent<PlayableDirector>();

     
        foreach (battleAnimation ba in sceneAnimationLists)
            sceneCollection.collection.Add(ba.tag.ToUpper(), ba.animation);

        sceneCollection.instanced = true;
    
        foreach (battleAnimation ba in playerAnimationLists)
            playerCollection.collection.Add(ba.tag.ToUpper(), ba.animation);
        
        playerCollection.instanced = true;
    
        foreach (battleAnimation ba in opponentAnimationLists)
            opponentCollection.collection.Add(ba.tag.ToUpper(), ba.animation);
        
        opponentCollection.instanced = true;
        

        BattleSystem.instances.ChangeState(turnState.ENTER);
    }

    public void PlayScene(string tag)
    {
        string t = tag.ToUpper();
        if (!sceneCollection.collection.ContainsKey(t))
            return;

        director.playableAsset = sceneCollection.collection[t];
        director.Play();
    }

    public void PlayPlayerAnimation(string tag)
    {
        string t = tag.ToUpper();
        if (!playerCollection.collection.ContainsKey(t))
            return;
        
        director.playableAsset = playerCollection.collection[t];
        director.Play();
    }

    public void PlayEnemyAnimation(string tag)
    {
        string t = tag.ToUpper();
        if (!opponentCollection.collection.ContainsKey(t))
            return;
        
        director.playableAsset = opponentCollection.collection[t];
        director.Play();
    }

    public float getAnimationTime(string tag)
    {
        if (director.playableAsset.name.ToUpper() != tag.ToUpper())
            return 1f;
            
        return (float)director.playableAsset.duration;
    }
}
