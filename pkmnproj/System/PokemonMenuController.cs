using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonMenuController : MonoBehaviour
{
    public static PokemonMenuController instances;

    public GameObject[] slot;
    public GameObject menu;

    private TrainerData trainerData;
    private PokemonSelection pSelection;
    bool d;

    [HideInInspector]
    public bool onSaved;

    void Start()
    {
        d = false;
        instances = this;
        trainerData = BattleSystem.instances.playerData;

        for (int i = 0; i < trainerData.TrainerTeamList.Count; i++)
        {
            slot[i].SetActive(true);
        }
    }

    public void ActivateMenu()
    {
        d = !d;
        Invoke("invokedMenu", 1);
    }

    void invokedMenu()
    {
        menu.SetActive(d);

        if (d == true)
            TextSystem.instance.StartDialogue("Choose a Pokemon");
    }

    public void saveTempData(PokemonSelection p)
    {
        onSaved = true;

        pSelection = p;
    }

    public void SwitchPokemon(PokemonSelection secondTarget)
    {
        if (pSelection.gameObject.name == secondTarget.gameObject.name || pSelection.idOrder != 0 && secondTarget.idOrder != 0)
            return;

        CheckTrainerTeam[] ct = GameObject.FindObjectsOfType<CheckTrainerTeam>();
        StartCoroutine(BattleSystem.instances.SwitchingPokemon(1, pSelection.idOrder, secondTarget.idOrder));

        foreach(CheckTrainerTeam c in ct)
            c.RefreshInfo();
        pSelection.RefreshInfo();
        secondTarget.RefreshInfo(); 

        FadeController.instances.SetFade("fadein");
        pSelection.DisableButton();
        d = false;
        onSaved = false;
        Invoke("invokedMenu", 1);
    }
}
