using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum turnState
{
    ENTER,
    SET,
    BATTLE,
    SHIFT,
    END
}

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem instances;

    public static bool battleStyleSet = false,
                       bothSwitch = false;
                    
    public static int turn;

    public static FieldStatus currentFieldStatus = FieldStatus.NULL;  

    public const float waittime = 1.2f;

    #region TYPE CONSTANT
    public const string GROUND = "Ground",
                        WATER = "Water",
                        GHOST = "Ghost",
                        BUG = "Bug",
                        FIGHTING = "Fighting",
                        PSYCHIC = "Psychic",
                        GRASS = "Grass",
                        DARK = "Dark",
                        NORMAL = "Normal",
                        POISON = "Poison",
                        ELECTRIC = "Electric",
                        STEEL = "Steel",
                        ROCK = "Rock",
                        DRAGON = "Dragon",
                        FLYING = "Flying",
                        FIRE = "Fire",
                        ICE = "Ice",
                        FAIRY = "Fairy";
    #endregion

    [System.Serializable]
    public class TypeData
    {
        public string typeName;
        public string[] weakAgainst,
                        inneffectiveAgainst,
                        superEffectiveAgainst;
    }

    [TextArea(4, 32)]
    public string[] effectivenessPrompt = new string[3];

    public List<TypeData> TypingSystem;

    [HideInInspector]
    public CapturedPokemonData opponentPokemon, playerPokemon;

    public TrainerData playerData;//MAKE PRIVATE LATER
    public TrainerData opponentData;

    private HealthUI opponentHealth, playerHealth;
    private MovesData opMoves, pMoves;
    private turnState state, prevState;
    private GameObject playerh, options, opponenth;
    private Dictionary<int, (int, string)> countdownDictionary = new Dictionary<int, (int, string)>();
    
    private bool ExecuteCoroutine, onSwitch, onUseMoveDial;
    private int megaindex, statusLinger, cc = 0;

    void Awake()
        => instances = this;

    void Start()
    {   
        turn = 0;

        opponentHealth = GameObject.Find("healthOP").GetComponent<HealthUI>();
        playerHealth = GameObject.Find("healthP").GetComponent<HealthUI>();
        playerh = GameObject.Find("PlayerHealthBar");
        opponenth = GameObject.Find("OpponentHealthBar");
        options = GameObject.Find("OptionParent");

        for (int i = 0; i < playerData.TrainerTeamList.Count; i++)
        {
            playerData.TrainerTeamList[i].pokemon = StatSystem.instances.GetActualData(playerData, i);
            playerData.TrainerTeamList[i].pokemon.health = playerData.TrainerTeamList[i].pokemon.HP; ///DELETE THIS AFTER OVERWORLD IS CREATED
            playerData.TrainerTeamList[i].pokemon.fainted = false; ///DELETE LATER
        }

        for (int i = 0; i < opponentData.TrainerTeamList.Count; i++)
        {
            opponentData.TrainerTeamList[i].pokemon = StatSystem.instances.GetActualData(opponentData, i);
            opponentData.TrainerTeamList[i].pokemon.health = opponentData.TrainerTeamList[i].pokemon.HP; 
            opponentData.TrainerTeamList[i].pokemon.fainted = false;
        }

        playerPokemon = playerData.TrainerTeamList[0].pokemon;
        opponentPokemon = opponentData.TrainerTeamList[0].pokemon;

        playerHealth.pData = playerPokemon;
        opponentHealth.pData = opponentPokemon;

        playerPokemon.status = StatusAilments.Null;
        opponentPokemon.status = StatusAilments.Null;
    }

    public void ChangeState(turnState s)
    {
        prevState = state;
        state = s;

        switch (state)
        {
            case turnState.ENTER : { TestStart(); } break;
            case turnState.SET : { SetState(); } break;
            case turnState.BATTLE : { BattleState(); } break;
            case turnState.SHIFT : { ShiftState(); } break;
            case turnState.END : {  } break;
        }
    }

    public TrainerData GetTrainerData(int type)
        { return type == 1 ? playerData : opponentData; }

    public void SetMegaBoolean(int t)
    { 
        if (!playerPokemon.haveAlternateForm || playerPokemon.onAltForm)
            return;

        if (megaindex > 0) { t = 0; }

        playerHealth.DoMegaEffect(t == 1 ? true : false);
        AudioController.instances.PlaySfx("aButton");
        megaindex = t;
    }

    public void ResetMegaBoolean()
        { megaindex = 0; playerHealth.DoMegaEffect(false); }

    public void AddMoves(int id, int moveId)
    {
        switch (id)
        {
            case 0: { pMoves = playerPokemon.moveset[moveId]; } break;
            case 1: { opMoves = opponentPokemon.moveset[moveId]; } break;
        }
    }

    void TestStart()
    {
        BattleAnimationController.instances.PlayScene("BattleIntro");
        Invoke("SetState", BattleAnimationController.instances.getAnimationTime("BattleIntro"));
    }

    void SetState()
    {
        turn += 1;

        TextSystem.instance.StartDialogue($"What will {playerPokemon.nickname} do?");

        LeanTween.move(playerh.GetComponent<RectTransform>(), new Vector3(0.0f, 112.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeOutQuad);

        LeanTween.move(opponenth.GetComponent<RectTransform>(), new Vector3(0.0f, -40.0f, 0f), 0.5f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.move(options.GetComponent<RectTransform>(), new Vector3(0.0f, 20.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeOutQuad);
    }

    void BattleState()
    {
        if (!onSwitch)
        {
            LeanTween.move(playerh.GetComponent<RectTransform>(), new Vector3(0.0f, 42f, 0.0f), 0.5f).setEase(LeanTweenType.easeOutQuad);
            LeanTween.move(options.GetComponent<RectTransform>(), new Vector3(0.0f, -100.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeOutQuad);
        }

        if (!ExecuteCoroutine && megaindex == 0)
            { StartCoroutine(DamageState()); } else 
        if (megaindex == 1)
            { StartCoroutine(playerMegaEvolve(megaindex)); }
    }

    void ShiftState()
    {
        TextSystem.instance.StartDialogue($"{opponentData.trainerName} is about to send out {opponentData.TrainerTeamList[1].pokemon.nickname}, would you like to switch your Pokemon ?");
        BattleOptionsController.instances.ShowsPokemonButton(true);
    }

    IEnumerator DamageState()
    {
        ExecuteCoroutine = true;
        OpponentAI.instances.OpponentSetMoves();
        yield return new WaitForSeconds(waittime*2f);

        if (playerPokemon.speed > opponentPokemon.speed)
        {
#region //STEP 1 Player turn to attack

            if (pMoves != null)
            {
    #region /Check for status
                (bool b, string s) bs = RandomNumberGenerator.instances.CheckAccuracy(pMoves.accuracy, playerPokemon.status);
                if (checkEvent(playerPokemon)) 
                { StartCoroutine(startEvent(playerPokemon, pMoves, 1, bs.b)); yield return new WaitForSeconds(waittime); } else 
                { TextSystem.instance.StartDialogue($"{playerPokemon.nickname} used {pMoves.movesName}!"); } 

                if (!bs.b)
                {
                    yield return new WaitForSeconds(waittime);
                    if (bs.s != "")
                        { TextSystem.instance.StartDialogue($"{playerPokemon.nickname} {bs.s}!"); }

                    if (playerPokemon.status == StatusAilments.Confused)
                    {
                        yield return new WaitForSeconds(waittime+.6f);
                        TextSystem.instance.StartDialogue("it hurt itself in its confusion!");
                        playerHealth.DoDamage(((((2* playerPokemon.level / 5)+ 2)* 40f * 40f/playerPokemon.defense) / 50) + 2);
                    }

                    yield return new WaitForSeconds(waittime);
    #endregion
                } 
                else 
                {
    #region /Battle !
                    BattleAnimationController.instances.PlayPlayerAnimation(pMoves.movesName);
                    yield return new WaitForSeconds(BattleAnimationController.instances.getAnimationTime(pMoves.movesName)-.1f); 

                    if (pMoves.movesCategory.ToUpper() != moveCategory.STATUS.ToString()) 
                    { 
                        DamageStatusText(pMoves, opponentPokemon.baseData);
                        opponentHealth.DoDamage(DamageCalculation(pMoves, playerPokemon, opponentPokemon));
                    }

                    yield return new WaitForSeconds(waittime);   
                }
    #endregion

    #region /Check if Pokemon is Fainted
                if (opponentHealth.GetHealth() == 0)
                {
                    TextSystem.instance.StartDialogue($"The foe's {opponentPokemon.nickname} fainted!");
                    yield return new WaitForSeconds(2f);

                    EndDamageStep();

                    if (playerPokemon.status != StatusAilments.Null && opponentPokemon.status != StatusAilments.Null) 
                    {
                        playerHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); 
                        opponentHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime);
                    } else 
                    if (playerPokemon.status != StatusAilments.Null) { playerHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); } else
                    if (opponentPokemon.status != StatusAilments.Null) { opponentHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); }

                    yield break;
                }
    #endregion

    #region /Check if opposite pokemon got status effect
                if (StatusSystem.instances.CheckMoves(pMoves.movesName).d && StatusSystem.instances.CheckMoves(pMoves.movesName).s != opponentPokemon.status)
                {
                    StatusAilments statusailment = StatusSystem.instances.CheckMoves(pMoves.movesName).s;
                    opponentPokemon.status = statusailment;

                    opponentHealth.SetStatusCountdown(StatusSystem.instances.CheckMoves(pMoves.movesName).a);
                    opponentHealth.SetStatusImage();
                    opponentHealth.AnimateStatusEff(statusailment);
                    StatusSystem.instances.StatusEffect(opponentPokemon, opponentPokemon.status);
                    TextSystem.instance.StartDialogue($"The foe's {opponentPokemon.nickname} got {statusailment.ToString()}!");
                    yield return new WaitForSeconds(0.75f);
                }
            }
    #endregion

#endregion

        yield return new WaitForSeconds(waittime*2f);

#region //STEP 2 Opponent turn to attack

            if (opMoves != null)
            {
    #region /Check for status
                (bool b, string s) bs = RandomNumberGenerator.instances.CheckAccuracy(opMoves.accuracy, opponentPokemon.status);
                if (checkEvent(opponentPokemon)) 
                { StartCoroutine(startEvent(opponentPokemon, opMoves, 2, bs.b)); yield return new WaitForSeconds(waittime); } else 
                { TextSystem.instance.StartDialogue($"The foe's {opponentPokemon.nickname} used {opMoves.movesName}!"); }

                if (!bs.b)
                {
                    yield return new WaitForSeconds(waittime); 
                    if (bs.s != "")
                        { TextSystem.instance.StartDialogue($"The foe's {opponentPokemon.nickname} {bs.s}!"); }

                    if (opponentPokemon.status == StatusAilments.Confused)
                    {
                        yield return new WaitForSeconds(waittime+.6f);
                        TextSystem.instance.StartDialogue("it hurt itself in its confusion!");
                        opponentHealth.DoDamage(((((2* opponentPokemon.level / 5)+ 2)* 40f * 40f/opponentPokemon.defense) / 50) + 2);
                    }

                    yield return new WaitForSeconds(2f);
                    EndDamageStep();

                    if (playerPokemon.status != StatusAilments.Null && opponentPokemon.status != StatusAilments.Null) 
                    {
                        playerHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); 
                        opponentHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime);
                    } else 
                    if (playerPokemon.status != StatusAilments.Null) { playerHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); } else
                    if (opponentPokemon.status != StatusAilments.Null) { opponentHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); }                 

                    yield return new WaitForSeconds(2f);
                    ChangeState(turnState.SET);
                    yield break;
                }
    #endregion

    #region /Battle !
                BattleAnimationController.instances.PlayEnemyAnimation(opMoves.movesName);
                yield return new WaitForSeconds(BattleAnimationController.instances.getAnimationTime(opMoves.movesName)-.1f); 

                if (opMoves.movesCategory.ToUpper() != moveCategory.STATUS.ToString())
                { 
                    DamageStatusText(opMoves, playerPokemon.baseData);
                    playerHealth.DoDamage(DamageCalculation(opMoves, opponentPokemon, playerPokemon)); 
                }

                yield return new WaitForSeconds(waittime);

    #endregion

    #region /Check if pokemon is fainted
                if (playerHealth.GetHealth() == 0)
                {
                    TextSystem.instance.StartDialogue($"{playerPokemon.nickname} fainted!");
                    yield return new WaitForSeconds(1.4f);

                    EndDamageStep();

                    if (playerPokemon.status != StatusAilments.Null && opponentPokemon.status != StatusAilments.Null) 
                    {
                        playerHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); 
                        opponentHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime);
                    } else 
                    if (playerPokemon.status != StatusAilments.Null) { playerHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); } else
                    if (opponentPokemon.status != StatusAilments.Null) { opponentHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); }

                    yield break;
                }
    #endregion

    #region /Check if opposite pokemon got status effect
                if (StatusSystem.instances.CheckMoves(opMoves.movesName).d && StatusSystem.instances.CheckMoves(opMoves.movesName).s != playerPokemon.status)
                {
                    StatusAilments statusailment = StatusSystem.instances.CheckMoves(opMoves.movesName).s;
                    playerPokemon.status = statusailment;

                    playerHealth.SetStatusCountdown(StatusSystem.instances.CheckMoves(opMoves.movesName).a);
                    playerHealth.SetStatusImage();
                    playerHealth.AnimateStatusEff(statusailment);
                    StatusSystem.instances.StatusEffect(playerPokemon, playerPokemon.status);
                    TextSystem.instance.StartDialogue($"{playerPokemon.nickname} got {statusailment.ToString()}!");
                    yield return new WaitForSeconds(0.75f);
                }
            }
    #endregion

#endregion

        } else 
        if (playerPokemon.speed < opponentPokemon.speed)
        {

#region //STEP 1 Opponent turn to attack

            if (opMoves != null)
            {  
    #region /Check for status
                (bool b, string s) bs = RandomNumberGenerator.instances.CheckAccuracy(opMoves.accuracy, opponentPokemon.status);
                if (checkEvent(opponentPokemon)) 
                { StartCoroutine(startEvent(opponentPokemon, opMoves, 2, bs.b)); yield return new WaitForSeconds(waittime); } else 
                { TextSystem.instance.StartDialogue($"The foe's {opponentPokemon.nickname} used {opMoves.movesName}!"); }
                
                if (!bs.b)
                {
                    yield return new WaitForSeconds(waittime); 
                    if (bs.s != "")
                        { TextSystem.instance.StartDialogue($"The foe's {opponentPokemon.nickname} {bs.s} !"); }

                    if (opponentPokemon.status == StatusAilments.Confused)
                    {
                        yield return new WaitForSeconds(waittime+.8f);
                        TextSystem.instance.StartDialogue("it hurt itself in its confusion!");
                        opponentHealth.DoDamage(((((2* opponentPokemon.level / 5)+ 2)* 40f * 40f/opponentPokemon.defense) / 50) + 2);
                    }

                    yield return new WaitForSeconds(2f);
    
    #endregion
                }
                else 
                {
    #region /Battle !
                    BattleAnimationController.instances.PlayEnemyAnimation(opMoves.movesName);
                    yield return new WaitForSeconds(BattleAnimationController.instances.getAnimationTime(opMoves.movesName)-.1f);

                    if (opMoves.movesCategory.ToUpper() != moveCategory.STATUS.ToString()) 
                    { 
                        DamageStatusText(opMoves, playerPokemon.baseData);
                        playerHealth.DoDamage(DamageCalculation(opMoves, opponentPokemon, playerPokemon)); 
                    }

                    yield return new WaitForSeconds(waittime);
                }
    #endregion

    #region /Check if pokemon fainted
                if (playerHealth.GetHealth() == 0)
                {
                    TextSystem.instance.StartDialogue($"{playerPokemon.nickname} fainted!");
                    yield return new WaitForSeconds(1.4f);

                    EndDamageStep();

                    if (playerPokemon.status != StatusAilments.Null && opponentPokemon.status != StatusAilments.Null) 
                    {
                        playerHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); 
                        opponentHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime);
                    } else 
                    if (playerPokemon.status != StatusAilments.Null) { playerHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); } else
                    if (opponentPokemon.status != StatusAilments.Null) { opponentHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); }

                    yield break;
                }
    #endregion

    #region /Check if opposite pokemon got status effect
                if (StatusSystem.instances.CheckMoves(opMoves.movesName).d && StatusSystem.instances.CheckMoves(opMoves.movesName).s != playerPokemon.status)
                {
                    StatusAilments statusailment = StatusSystem.instances.CheckMoves(opMoves.movesName).s;
                    playerPokemon.status = statusailment;

                    playerHealth.SetStatusCountdown(StatusSystem.instances.CheckMoves(opMoves.movesName).a);
                    playerHealth.SetStatusImage();
                    playerHealth.AnimateStatusEff(statusailment);
                    StatusSystem.instances.StatusEffect(playerPokemon, playerPokemon.status);
                    TextSystem.instance.StartDialogue($"{playerPokemon.nickname} got {statusailment.ToString()}!");
                    yield return new WaitForSeconds(0.75f);
                }
            }
    #endregion

#endregion

        yield return new WaitForSeconds(waittime*2f);

#region //STEP 2 Player turn to attack

            if (pMoves != null)
            {
    #region /Check for status
                (bool b, string s) bs = RandomNumberGenerator.instances.CheckAccuracy(pMoves.accuracy, playerPokemon.status);
                if (checkEvent(playerPokemon)) 
                { StartCoroutine(startEvent(playerPokemon, pMoves, 1, bs.b)); yield return new WaitForSeconds(waittime); } else 
                { TextSystem.instance.StartDialogue($"{playerPokemon.nickname} used {pMoves.movesName}!"); }

                if (!bs.b)
                {   
                    yield return new WaitForSeconds(waittime); 
                    if (bs.s != "")
                        { TextSystem.instance.StartDialogue($"{playerPokemon.nickname} {bs.s}!"); }

                    if (playerPokemon.status == StatusAilments.Confused)
                    {
                        yield return new WaitForSeconds(waittime+.6f);
                        TextSystem.instance.StartDialogue("it hurt itself in its confusion!");
                        playerHealth.DoDamage(((((2* playerPokemon.level / 5)+ 2)* 40f * 40f/playerPokemon.defense) / 50) + 2);
                    }
                    
                    yield return new WaitForSeconds(1.4f);
                    EndDamageStep();

                    if (playerPokemon.status != StatusAilments.Null && opponentPokemon.status != StatusAilments.Null) 
                    {
                        playerHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); 
                        opponentHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime);
                    } else 
                    if (playerPokemon.status != StatusAilments.Null) { playerHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); } else
                    if (opponentPokemon.status != StatusAilments.Null) { opponentHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); }               

                    yield return new WaitForSeconds(2f);
                    ChangeState(turnState.SET);
                    yield break;
                }
    #endregion

    #region /Battle !
                BattleAnimationController.instances.PlayPlayerAnimation(pMoves.movesName);
                yield return new WaitForSeconds(BattleAnimationController.instances.getAnimationTime(pMoves.movesName)-.1f); 

                if (pMoves.movesCategory.ToUpper() != moveCategory.STATUS.ToString()) 
                { 
                    DamageStatusText(pMoves, opponentPokemon.baseData);
                    opponentHealth.DoDamage(DamageCalculation(pMoves, playerPokemon, opponentPokemon)); 
                }

                yield return new WaitForSeconds(waittime);

    #endregion

    #region /Check if pokemon fainted
                if (opponentHealth.GetHealth() == 0)
                {
                    TextSystem.instance.StartDialogue($"{opponentPokemon.nickname} fainted!");
                    yield return new WaitForSeconds(1.4f);

                    EndDamageStep();

                    if (playerPokemon.status != StatusAilments.Null && opponentPokemon.status != StatusAilments.Null) 
                    {
                        playerHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); 
                        opponentHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime);
                    } else 
                    if (playerPokemon.status != StatusAilments.Null) { playerHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); } else
                    if (opponentPokemon.status != StatusAilments.Null) { opponentHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); }

                    yield break;
                }
    #endregion

    #region /Check if opposite pokemon got status effect
                if (StatusSystem.instances.CheckMoves(pMoves.movesName).d && StatusSystem.instances.CheckMoves(pMoves.movesName).s != opponentPokemon.status)
                {
                    StatusAilments statusailment = StatusSystem.instances.CheckMoves(pMoves.movesName).s;
                    opponentPokemon.status = statusailment;

                    opponentHealth.SetStatusCountdown(StatusSystem.instances.CheckMoves(pMoves.movesName).a);
                    opponentHealth.SetStatusImage();
                    opponentHealth.AnimateStatusEff(statusailment);
                    StatusSystem.instances.StatusEffect(opponentPokemon, opponentPokemon.status);
                    TextSystem.instance.StartDialogue($"The foe's {opponentPokemon.nickname} got {statusailment.ToString()}!");
                    yield return new WaitForSeconds(0.75f);
                }
            }
        }    
    #endregion
    
#endregion

        yield return new WaitForSeconds(2f);
        EndDamageStep();

        if (playerPokemon.status != StatusAilments.Null && opponentPokemon.status != StatusAilments.Null) 
        {
            playerHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); 
            opponentHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime);
        } else 
        if (playerPokemon.status != StatusAilments.Null) { playerHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); } else
        if (opponentPokemon.status != StatusAilments.Null) { opponentHealth.DecreaseStatusCountdown(); yield return new WaitForSeconds(waittime); }

        yield return new WaitForSeconds(2f);
        ChangeState(turnState.SET);
    }

    void EndDamageStep()
    {
        onSwitch = false;
        ExecuteCoroutine = false;
        pMoves = null;
        opMoves = null;
    }

    void DamageStatusText(MovesData d, PokemonData targetP)
    {
        float effLog = CheckTypeEffectiveness(d.typeName, targetP.type[0], targetP.type[1]);

        switch (effLog)
        {
            case 0: { TextSystem.instance.StartDialogue($"{effectivenessPrompt[2]}{targetP.PokemonName}..."); } break;
            case 1: { AudioController.instances.PlaySfx("Hit Normal"); } break;
            case 0.5f : 
            {
                AudioController.instances.PlaySfx("Hit Weak");
                TextSystem.instance.StartDialogue(effectivenessPrompt[1]); 
            } break;
            case 2: 
            {
                AudioController.instances.PlaySfx("Hit Super Effective");
                TextSystem.instance.StartDialogue(effectivenessPrompt[0]);
            } break;
        }
    }

    float DamageCalculation(MovesData m, CapturedPokemonData d, CapturedPokemonData td)
    {
        float stab = (d.baseData.type[0] == m.typeName || d.baseData.type[1] == m.typeName ? 1.5f : 1f);
        float rand = Random.Range(0.85f, 1f);
        float ad = (m.movesCategory == moveCategory.PHYSICAL.ToString() ? ((float)d.attack / (float)td.defense) : ((float)d.specialattack / (float)td.specialdefense));
        float typeEff = CheckTypeEffectiveness(m.typeName, td.baseData.type[0], td.baseData.type[1]);

        float dmg = ( ( ( ((2 * d.level) / 5 + 2) * m.power * ad ) / 50f ) + 2 ) * rand * stab * typeEff;
        return dmg;
    }

    float CheckTypeEffectiveness(string type1, string deftype1, string deftype2)
    {
        if (deftype2 == null)
            { deftype2 = "Empty"; }

        float res = 1;
        bool onWeak = false, onInneffective = false, onSuperEffective = false;

        for (int i = 0; i < TypingSystem.Count; i++)
        {
            if (TypingSystem[i].typeName == type1)
            {
                string[] weakagainst = TypingSystem[i].weakAgainst,
                         inneffective = TypingSystem[i].inneffectiveAgainst,
                         supereffective = TypingSystem[i].superEffectiveAgainst;

                for (int n = 0; n < inneffective.Length; n++)
                {
                    if (inneffective[n] == deftype1 || inneffective[n] == deftype2)
                        { onInneffective = true; }
                }

                for (int w = 0; w < weakagainst.Length; w++)
                {
                    if (weakagainst[w] == deftype1 || weakagainst[w] == deftype2) 
                        { onWeak = true; }
                }

                for (int s = 0; s < supereffective.Length; s++)
                {
                    if (supereffective[s] == deftype1 || supereffective[s] == deftype2)
                        { onSuperEffective = true; }
                }

                if (!onInneffective)
                {
                    if (onWeak && onSuperEffective) { res = 1f; return res; } else 
                    if (onWeak && !onSuperEffective) { res = 0.5f; } else 
                    if (!onWeak && onSuperEffective) { res = 2f; return res; } else 
                    {
                        res = 1f;
                        return res;
                    }
                } else {
                    res = 0f;
                    return res;
                }
                
            }
        }

        return res;
    }

    public IEnumerator SwitchingPokemon(int type, int firstOrder, int secondOrder)
    {
        if (type == 1)
        {
            bool f = playerPokemon.fainted;
            MovesType[] m = GameObject.FindObjectsOfType<MovesType>();

            LeanTween.move(playerh.GetComponent<RectTransform>(), new Vector3(240.0f, 42.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeInQuad);
            LeanTween.move(options.GetComponent<RectTransform>(), new Vector3(0.0f, -100.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeInQuad);

            onSwitch = true;
            var tempFirst = playerData.TrainerTeamList[firstOrder];

            playerData.TrainerTeamList[firstOrder] = playerData.TrainerTeamList[secondOrder];
            playerData.TrainerTeamList[secondOrder] = tempFirst;

            yield return new WaitForSeconds(BattleSystem.waittime*2.2f);

            playerHealth.ResetSprite();
            playerPokemon = playerData.TrainerTeamList[0].pokemon;

            if (!f) 
            { 
                BattleAnimationController.instances.PlayScene("playerRecall");
                TextSystem.instance.StartDialogue($"{playerPokemon.nickname}, that's enough! switch out!"); 
                yield return new WaitForSeconds(BattleAnimationController.instances.getAnimationTime("playerRecall"));
            }

            foreach(MovesType t in m) { t.RefreshMove(); }
            BattleAnimationController.instances.PlayScene("playerSwitchPokemon");
            playerHealth.RefreshPokemon();
            TextSystem.instance.StartDialogue($"Go! {playerPokemon.nickname}");
            LeanTween.move(playerh.GetComponent<RectTransform>(), new Vector3(0.0f, 42.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeInQuad);
            yield return new WaitForSeconds(BattleAnimationController.instances.getAnimationTime("playerSwitchPokemon")/2f);

            pMoves = null;
            onSwitch = false;
            if (bothSwitch) { OpponentAI.instances.OpponentSwitchPokemon(1); } else 
            if (!bothSwitch)
            {
                if (!f) { ChangeState(turnState.BATTLE); } else 
                if (f) { ChangeState(turnState.SET); }
            }
        } else 
        if (type == 2)
        {
            bool f = opponentPokemon.fainted;
            var tempFirst = opponentData.TrainerTeamList[firstOrder];

            onSwitch = true;

            opponentData.TrainerTeamList[firstOrder] = opponentData.TrainerTeamList[secondOrder];
            opponentData.TrainerTeamList[secondOrder] = tempFirst;

            LeanTween.move(opponenth.GetComponent<RectTransform>(), new Vector3(-240.0f, -40.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeInQuad);
            LeanTween.move(options.GetComponent<RectTransform>(), new Vector3(0.0f, -100.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeInQuad);

            yield return new WaitForSeconds(BattleSystem.waittime*2f);

            opponentHealth.ResetSprite();
            opponentPokemon = opponentData.TrainerTeamList[0].pokemon;
            if (!f)
            { 
                ///RECALL ANIMATION
                TextSystem.instance.StartDialogue($"Foe's {opponentData.trainerName} is switching out {opponentPokemon.nickname}!"); 
                yield return new WaitForSeconds(BattleSystem.waittime-.2f);
            }

            BattleAnimationController.instances.PlayScene($"{opponentData.trainerName}SwitchPokemon");
            opponentHealth.RefreshPokemon();
            TextSystem.instance.StartDialogue($"Foe's {opponentData.trainerName} sent out {opponentPokemon.nickname}!");
            LeanTween.move(opponenth.GetComponent<RectTransform>(), new Vector3(0.0f, -40.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeInQuad);
            yield return new WaitForSeconds(BattleAnimationController.instances.getAnimationTime($"{opponentData.trainerName}SwitchPokemon"));

            opMoves = null;
            onSwitch = false;
            bothSwitch = false;
            if (!f) { ChangeState(turnState.BATTLE); } else 
            if (f) { ChangeState(turnState.SET); }
        } 
    }

    public IEnumerator ItemPhase(string texttotype)
    {
        FadeController.instances.SetFade("fadein");
        yield return new WaitForSeconds(BattleSystem.waittime*2.2f);

        TextSystem.instance.StartDialogue(texttotype);
        BagController.instances.doRecovery = true;
        AudioController.instances.PlaySfx("Healing");
        yield return new WaitForSeconds(BattleSystem.waittime-.35f);

        ChangeState(turnState.BATTLE);
    }

    public IEnumerator playerMegaEvolve(int type)
    {   
        GameObject megabg = GameObject.Find("megabg");

        AudioController.instances.PlaySfx("Megapopup");
        TextSystem.instance.StartDialogue($"{playerData.trainerName} keystone is reacting to {playerPokemon.nickname} Mega Stone!");

        LeanTween.move(playerh.GetComponent<RectTransform>(), new Vector3(0.0f, 42f, 0.0f), 0.5f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.move(options.GetComponent<RectTransform>(), new Vector3(0.0f, -100.0f, 0.0f), 0.5f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.moveLocal(megabg, new Vector3(0.0f, 0.0f, 7.0f), 0.1f);
        yield return new WaitForSeconds(BattleSystem.waittime*1.2f);

        LeanTween.moveLocal(megabg, new Vector3(16.0f, 2.0f, 7.0f), 0.05f);
        BattleAnimationController.instances.PlayScene("megaEvolution");
        yield return new WaitForSeconds(1.48f);

        ResetMegaBoolean();
        StatSystem.instances.SwitchAlternateData(type == 1 ? playerPokemon : opponentPokemon);
        playerPokemon = StatSystem.instances.GetActualData(playerData, 0);
        playerHealth.RefreshPokemon();
        yield return new WaitForSeconds(BattleSystem.waittime*2f);

        TextSystem.instance.StartDialogue($"{playerPokemon.nickname} Mega evolved !");
        StartCoroutine(DamageState());
    }

    bool checkEvent(CapturedPokemonData data)
    {
        if (data.status == StatusAilments.Confused || data.status == StatusAilments.Frozen)
            { return true; } else { return false; }
    }

    IEnumerator startEvent(CapturedPokemonData pdata, MovesData mdata, int i, bool dotxt)
    {
        if (pdata.status == StatusAilments.Confused)
        {
            TextSystem.instance.StartDialogue($"{pdata.nickname} is Confused!");
            if (i == 1) { playerHealth.AnimateStatusEff(pdata.status); } else 
            if (i == 2) { opponentHealth.AnimateStatusEff(pdata.status); }

            yield return new WaitForSeconds(waittime*2f);
            if (dotxt)
                TextSystem.instance.StartDialogue(i == 1 ? $"{pdata.nickname} used {mdata.movesName}" : $"The foe's {pdata.nickname} used {mdata.movesName}");
        } else
        if (pdata.status == StatusAilments.Frozen)
        {
            if (i == 1) { playerHealth.AnimateStatusEff(pdata.status); } else 
            if (i == 2) { opponentHealth.AnimateStatusEff(pdata.status); }
        }
    }

    public void AddCountdownTag(string tag, int turn)
	{
		countdownDictionary.Add(cc++, (turn, tag));
	}

	void Countdown()
	{
		for (int i = 0; i < countdownDictionary.Count; i++)
		{
			int ni = countdownDictionary[i].Item1 - turn;
			
			if (ni <= 0)
				EndCountdown(countdownDictionary[i].Item2);
		}
	}

	void EndCountdown(string tag)
	{
		switch(tag.ToUpper())
		{
			case "DISABLE": { MovesetSetter.instances.ResetMoves(playerData, opponentData); }  break;
			case "ENVIRONMENT": { currentFieldStatus = FieldStatus.NULL; TextSystem.instance.StartDialogue("The weather conditions has come return to normal"); } break; 
		}
	}
}
