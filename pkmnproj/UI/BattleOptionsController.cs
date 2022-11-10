using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum OptionState
{
    FIGHT,
    POKEMON,
    BAG, 
    NULL
}

public class BattleOptionsController : MonoBehaviour
{
    public static BattleOptionsController instances;

    OptionState state;
    private GameObject fightMenuAnim, megabut, switchPokemonButton;

    bool onMega;

    void Start()
    {
        instances = this;
        state = OptionState.NULL;
        fightMenuAnim = GameObject.Find("FightMenu");
        megabut = GameObject.Find("MegaMenu");
        switchPokemonButton = GameObject.Find("yesnopar");
        onMega = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            switch (state)
            {
                case OptionState.FIGHT: { ResetFightAnim();BattleSystem.instances.ResetMegaBoolean(); } break;
                case OptionState.POKEMON: { ResetPokemonAnim(); } break;
                case OptionState.BAG: { ResetBagAnim(); } break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
            Mega();
    }

    public void Mega()
    {
        BattleSystem.instances.SetMegaBoolean(1);
        MegaButtonAnimation(true);
    }

    public void Fight()
    {
        state = OptionState.FIGHT;
        LeanTween.move(fightMenuAnim.GetComponent<RectTransform>(), new Vector3(0.0f, 0.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeOutQuint);
        AudioController.instances.PlaySfx("aButton");

        if (BattleSystem.instances.GetTrainerData(1).TrainerTeamList[0].pokemon.canMega && !BattleSystem.instances.GetTrainerData(1).TrainerTeamList[0].pokemon.onAltForm)
        {
            onMega = false;
            megabut.GetComponent<Animator>().Play("Megabuttonnotselected"); 
            LeanTween.move(megabut.GetComponent<RectTransform>(), new Vector3(-272.0f, -60.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeOutQuad);
        }
    }

    public void Pokemon()
    {
        state = OptionState.POKEMON;

        AudioController.instances.PlaySfx("aButton");
        FadeController.instances.SetFade("fadein");
        PokemonMenuController.instances.ActivateMenu();
        ShowsPokemonButton(false);
    }

    public void Bag()
    {
        state = OptionState.BAG;
        
        AudioController.instances.PlaySfx("aButton");
        FadeController.instances.SetFade("fadein");
        BagController.instances.ActivateMenu();
    }

    public void Run()
    {
        TextSystem.instance.StartDialogue("Cannot escape!");
        AudioController.instances.PlaySfx("aButton");
    }

    public void ResetFightAnim()
    {
        LeanTween.move(megabut.GetComponent<RectTransform>(), new Vector3(-560.0f, -60.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.move(fightMenuAnim.GetComponent<RectTransform>(), new Vector3(0.0f, -100.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeInQuint);
        MegaButtonAnimation(false);
        state = OptionState.NULL;
    }
    
    public void ResetPokemonAnim()
    {
        FadeController.instances.SetFade("fadein");
        PokemonMenuController.instances.ActivateMenu();
        state = OptionState.NULL;
    }

    public void ResetBagAnim()
    {
        FadeController.instances.SetFade("fadein");
        BagController.instances.ActivateMenu();
        state = OptionState.NULL;
    }

    public void AcceptSwitch()
    {
        state = OptionState.POKEMON;

        BattleSystem.bothSwitch = true;
        AudioController.instances.PlaySfx("aButton");
        FadeController.instances.SetFade("fadein");
        PokemonMenuController.instances.ActivateMenu();
        ShowsPokemonButton(false);
    }

    public void CancelSwitch()
    {
        OpponentAI.instances.OpponentSwitchPokemon(1);
        ShowsPokemonButton(false);
        state = OptionState.NULL;
    }

    public void ShowsPokemonButton(bool show)
    {
        if (show)
        { LeanTween.move(switchPokemonButton.GetComponent<RectTransform>(), new Vector3(0.0f, 0.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeOutQuad); } else 
        if (!show)
        { LeanTween.move(switchPokemonButton.GetComponent<RectTransform>(), new Vector3(0.0f, -120.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeOutQuad); }
    }

    void MegaButtonAnimation(bool doAudio)
    {
        onMega = !onMega;

        if (onMega) 
        { 
            megabut.GetComponent<Animator>().Play("Megabuttonselected"); 
            if (doAudio) {AudioController.instances.PlaySfx("megabutton"); }
        } else 
        if (!onMega) 
        { 
            megabut.GetComponent<Animator>().Play("Megabuttonnotselected"); 
            if (doAudio) {AudioController.instances.PlaySfx("deselectmegabutton");}
        }
    }
}
