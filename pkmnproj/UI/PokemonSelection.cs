using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonSelection : MonoBehaviour
{
    public int idOrder;

    public Text uiName, uiLevel, uicurhp, uimaxhp;
    public Image icon, uihealth;
    public Sprite originalSprite, originalSelectSprite;
    public Sprite faintedSprite, selectedfaintedSprite;

    [HideInInspector]
    public CapturedPokemonData pokemonData;

    private TrainerData playerData;
    private HealthUI playerHealth;
    private Button b;
    private float maxFill;

    void Start()
    {
        playerData = BattleSystem.instances.GetTrainerData(1);
        playerHealth = GameObject.Find("healthP").GetComponent<HealthUI>();
        b = this.transform.Find("button").GetComponent<Button>();

        RefreshInfo();
    }

    public void RefreshInfo()
    {
        pokemonData = playerData.TrainerTeamList[idOrder].pokemon;

        uiName.text = pokemonData.nickname;
        uiLevel.text = pokemonData.level.ToString();

        icon.sprite = BattleSpriteController.instances.GetPokemonIcon(uiName.text);

        if (uicurhp != null)
        {
            uicurhp.text = pokemonData.health.ToString("F0");
            uimaxhp.text = pokemonData.HP.ToString("F0");
            LeanTween.move(icon.gameObject.GetComponent<RectTransform>(), new Vector3(-96.0f, 22.0f, 0.0f), 0.1f).setLoopPingPong();
        } else 
        { LeanTween.move(icon.gameObject.GetComponent<RectTransform>(), new Vector3(-134.0f, 10.0f, 0.0f), 0.1f).setLoopPingPong(); }
        
        uihealth.fillAmount = pokemonData.health/pokemonData.HP;
        maxFill = uihealth.fillAmount;
    }

    void Update()
    { 
        if (uicurhp != null) { uicurhp.text = pokemonData.health.ToString("F0"); }
        uihealth.fillAmount = pokemonData.health/pokemonData.HP;

        if (pokemonData.fainted)
        {
            SpriteState s = new SpriteState();

            this.gameObject.GetComponent<Image>().sprite = faintedSprite;
            s.pressedSprite = selectedfaintedSprite;
            s.selectedSprite = selectedfaintedSprite;
            b.spriteState = s;

        } else
        if (!pokemonData.fainted) 
        {
            SpriteState s = new SpriteState();
            b.enabled = true;
            
            this.gameObject.GetComponent<Image>().sprite = originalSprite;
            s.pressedSprite = originalSelectSprite;
            s.selectedSprite = originalSelectSprite;
            b.spriteState = s;
        }

        if (uihealth.fillAmount >  maxFill/2f) { uihealth.color = HealthUI.nnormal; }
        if (uihealth.fillAmount <= maxFill/2f && uihealth.fillAmount > maxFill/4f) { uihealth.color = HealthUI.llow; } else 
        if (uihealth.fillAmount <= maxFill/4f && uihealth.fillAmount != 0) { uihealth.color = HealthUI.ccritical; }
    }

    public void SelectPokemon()
    {
        if (!PokemonMenuController.instances.onSaved) { PokemonMenuController.instances.saveTempData(this); } else 
        if (PokemonMenuController.instances.onSaved) { SelectFirstPokemon(); }
    }

    public void SelectFirstPokemon()
    {
        if (PokemonMenuController.instances.onSaved) { PokemonMenuController.instances.SwitchPokemon(this); } else 
        if (!PokemonMenuController.instances.onSaved) { SelectPokemon(); }
    }

    public void SelectTargetPokemon()
        => BagController.instances.SaveTargetPokemon(this);   

    public void DisableButton()
    {
        if (pokemonData.fainted)
            b.enabled = false;
    }
}
