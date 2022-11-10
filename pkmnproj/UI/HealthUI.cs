using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public static Color nnormal, llow, ccritical;

    public Color normal, low, critical;
    public Text lvl, uiname, curh, maxh;
    public string spritetag;
    public Image statusCond;

    [HideInInspector]
    public CapturedPokemonData pData;

    private Image healthbar;
    private SpriteRenderer pokemonSprite;
    private Animator pokemonAnim, statusAnim;
    private Vector3 startpos;
    private Transform _light;

    private float maxFill, toHealth, currHealth, maxHealth, differenceHealth, defspdanim;
    private bool doDamage, ExecuteCoroutine, onstart;
    private int t, statusLingerCd;

    void Start()
    {
        nnormal = normal;
        llow = low;
        ccritical = critical;

        t = spritetag == "PlayerPokemon" ? 1 : 2;
        onstart = false;

        healthbar = GetComponent<Image>();
        pokemonSprite = GameObject.Find(spritetag).GetComponent<SpriteRenderer>();
        _light = pokemonSprite.gameObject.transform.Find("Lightsource").transform;
        pokemonAnim = pokemonSprite.gameObject.GetComponent<Animator>();
        statusAnim = pokemonSprite.gameObject.transform.Find("statuseff").GetComponent<Animator>();

        startpos = pokemonSprite.transform.localPosition;
        defspdanim = pokemonAnim.speed;
        
        if (pData != null)
        {
            RefreshPokemon();

            if (t == 2) { Invoke("DoEvent", 2.1f);onstart = true; }
                    
            healthbar.fillAmount = currHealth/ maxHealth;
        }
    }

    void DoEvent()
    {
        string n = pData.baseData.PokemonName;
        if (pData.onAltForm)
            n = $"Mega {pData.baseData.PokemonName}";

        AudioController.instances.PlaySfx(n);

        if (BattleSpriteController.instances.haveLightsource(pData.nickname, spritetag == "PlayerPokemon" ? 1 : 2))
        {
            LightComponent l = BattleSpriteController.instances.GetLightComponent(n, t);
            Light lightsource = _light.GetComponent<Light>();

            _light.localPosition = Vector3.zero;
            lightsource.enabled = true;
            lightsource.color = l.color;
            lightsource.range = l.range;
            lightsource.intensity = l.intensity;
            _light.localPosition += l.position;
        } else { _light.GetComponent<Light>().enabled = false; }
    }

    void Update()
    {
        currHealth = pData.health;
        healthbar.fillAmount = currHealth/ maxHealth;

        if (curh != null)
            curh.text = ((int)pData.health).ToString();

        if (doDamage)
        {
            pData.health = Mathf.Lerp(pData.health, toHealth, Time.deltaTime* (Mathf.Abs(differenceHealth)* (differenceHealth > 10 ? 0.075f : 2.0f)));

            if (pData.health == toHealth || currHealth <= 0) 
                { doDamage = false; }
        }

        if (currHealth <= 0)
            currHealth = 0;

        if (healthbar.fillAmount >  maxFill/2f) { healthbar.color = normal; }
        if (healthbar.fillAmount <= maxFill/2f && healthbar.fillAmount > maxFill/4f) { healthbar.color = low; } else 
        if (healthbar.fillAmount <= maxFill/4f && healthbar.fillAmount != 0) { healthbar.color = critical; } else 
        if (Mathf.Round(healthbar.fillAmount * 100.0f) * 0.01f <= 0 && !ExecuteCoroutine) 
        { 
            ExecuteCoroutine = true; 
            StartCoroutine(playerfaintedCoroutine()); 
        }

        if (pokemonSprite.color != Color.white)
            LeanTween.color(pokemonSprite.gameObject, Color.white, 0.4f).setEase(LeanTweenType.easeInQuad);
    }

    IEnumerator playerfaintedCoroutine()
    {
        yield return new WaitForSeconds(1);
        AudioController.instances.PlaySfx(pData.baseData.PokemonName);

        yield return new WaitForSeconds(1.2f);
        LeanTween.moveY(pokemonSprite.gameObject, -5.0f, 1.25f).setEase(LeanTweenType.easeOutExpo);
        AudioController.instances.PlaySfx("Fainted");

        pData.fainted = true;
        if (t == 1)
        {
            FadeController.instances.SetFade("fadein");
            PokemonMenuController.instances.ActivateMenu();
        } else 
        if (t == 2)
        {
            if (!BattleSystem.battleStyleSet)
            { BattleSystem.instances.ChangeState(turnState.SHIFT); } else
            if (BattleSystem.battleStyleSet) 
            { OpponentAI.instances.OpponentSwitchPokemon(1); }
        }

        yield return new WaitForSeconds(1.25f);
        _light.GetComponent<Light>().enabled = false;
    }

    void ResetSpritePositon()
    {
        pokemonSprite.transform.localPosition = startpos;
    }

    void SpriteAnimationController()
    {
        switch (pData.status)
        {
            case StatusAilments.Null: { pokemonAnim.speed = defspdanim; pokemonSprite.color = Color.white; } break;
            case StatusAilments.Poisoned: { pokemonAnim.speed = defspdanim; pokemonSprite.color = ColorInstances.i.GetCollectionColor("poisoned"); } break;
            case StatusAilments.BadlyPoison: { pokemonAnim.speed = defspdanim; pokemonSprite.color = ColorInstances.i.GetCollectionColor("poisoned"); } break;
            case StatusAilments.Paralyzed: { pokemonAnim.speed = defspdanim/2f; pokemonSprite.color = ColorInstances.i.GetCollectionColor("paralyzed"); } break;
            case StatusAilments.Burned: { pokemonAnim.speed = defspdanim; pokemonSprite.color = ColorInstances.i.GetCollectionColor("burned"); } break;
            case StatusAilments.Frozen: { pokemonAnim.speed = 0; pokemonSprite.color = ColorInstances.i.GetCollectionColor("frozen"); } break;
            case StatusAilments.Sleep: { pokemonAnim.speed = defspdanim/4f; pokemonSprite.color = Color.white; } break;
        }
    }

    public void RefreshPokemon()
    {
        doDamage = false;
        pData = MovesetSetter.instances.SetMoves((BattleSystem.instances.GetTrainerData(t)));
        bool onmega = pData.onAltForm;

        maxFill = healthbar.fillAmount;
        currHealth = pData.health;
        maxHealth = pData.HP;

        lvl.text = pData.level.ToString();
        uiname.text = pData.nickname.ToUpper();

        pokemonSprite.sprite = BattleSpriteController.instances.SetPokemonSprite(onmega ? $"Mega {pData.baseData.PokemonName}" : pData.baseData.PokemonName, t);
        pokemonAnim.runtimeAnimatorController = BattleSpriteController.instances.SetPokemonAnim(onmega ? $"Mega {pData.baseData.PokemonName}" : pData.baseData.PokemonName, t);
        ResetSpritePositon();

        if (t == 1)
        {
            curh.text = ((int)currHealth).ToString();
            maxh.text = maxHealth.ToString("F0");
            Invoke("DoEvent", 1.4f);
        } else 
        if (t == 2 && onstart)
        {
            Invoke("DoEvent", 1.4f);
        }
    }

    public void ResetSprite()
    {
        pokemonSprite.sprite = null;
    }

    public void DoDamage(float dmg)
    {
        doDamage = true;
        toHealth = currHealth - dmg;
        differenceHealth = currHealth - toHealth;
        StartCoroutine(hitEff());
    }

    public void DoMegaEffect(bool active)
    {
        if (t != 1)
            return;
        
        GameObject mlc = GameObject.Find("MegaLightComponent").transform.Find("MegaLight").gameObject;
        if (active)  { mlc.SetActive(true); } else 
        if (!active) { mlc.SetActive(false); }
    }

    public void SetStatusImage()
    {
        if (StatusSystem.instances.getStatusSprite(pData.status) == null)
            return;

        statusCond.enabled = true;
        statusCond.sprite = StatusSystem.instances.getStatusSprite(pData.status);
    }

    public float GetHealth()
    {  return Mathf.Round(healthbar.fillAmount * 100.0f) * 0.01f; }

    public int GetExactHealth()
    { return (int)currHealth; }

    public IEnumerator hitEff()
    {
        for (int i = 0; i < Random.Range(8, 12); i++)
        {
            pokemonSprite.enabled = true;
            yield return new WaitForSecondsRealtime(0.075f);

            pokemonSprite.enabled = false;
            yield return new WaitForSecondsRealtime(0.05f);

            pokemonSprite.enabled = true;
        }      
    }

    public void SetStatusCountdown(int i)
        => statusLingerCd = i; 

    public void DecreaseStatusCountdown()
    {
        if (statusLingerCd <= 0)
            return;

        StatusSystem.instances.EndTurnEffect(pData, pData.status, this);
        statusLingerCd--;

        if (statusLingerCd <= 0) 
        { 
            string txt = "";

            switch (pData.status)
            {
                case StatusAilments.Burned : { txt = $"{pData.nickname} is no longer burned!"; } break;
                case StatusAilments.Paralyzed : { txt = $"{pData.nickname} is no longer paralyzed!"; pData.speed = pData.baseData.speed; } break;
                case StatusAilments.Confused : { txt = $"{pData.nickname} is no longer confused!"; } break;
                case StatusAilments.Frozen : { txt = $"{pData.nickname} is no longer frozen!"; } break;
                case StatusAilments.Poisoned : { txt = $"{pData.nickname} is no longer poisoned"; } break;
            }

            TextSystem.instance.StartDialogue(txt);
            statusCond.sprite = null;
            statusCond.enabled = false;
            pData.status = StatusAilments.Null;
            SpriteAnimationController();
        }
    }

    public void AnimateStatusEff(StatusAilments status)
    {
        SpriteAnimationController();

        switch (status)
        {
            case StatusAilments.Burned: { statusAnim.Play("burned"); AudioController.instances.PlaySfx("burned"); } break;
            case StatusAilments.Confused: { statusAnim.Play("confused"); AudioController.instances.PlaySfx("confused"); } break;
            case StatusAilments.Frozen: { statusAnim.Play("frozen"); AudioController.instances.PlaySfx("frozen"); } break;
            case StatusAilments.Poisoned: { statusAnim.Play("poisoned"); AudioController.instances.PlaySfx("poisoned"); } break;
        }
    }
}   
