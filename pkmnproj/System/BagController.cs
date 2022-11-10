using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagController : MonoBehaviour
{
    public static BagController instances;

    public GameObject prefab;
    public GameObject menu;
    public GameObject[] pokemonslots;
    public Image itemIcon, itemonMouse;

    public Transform viewport;

    public List<ItemData> itemList;

    private TrainerData data;
    private PokemonSelection ps;
    private ItemSelection it;
    private  Dictionary<string, GameObject> haveItem = new Dictionary<string, GameObject>();
    private Dictionary<string, ItemData> itemDictionary = new Dictionary<string, ItemData>();

    private float healthto;
    private bool onActive;

    [HideInInspector]
    public bool doRecovery;

    void Awake()
        => instances = this;

    void Start()
    {
        itemonMouse.gameObject.SetActive(false);
        onActive = false;
        doRecovery = false;
        data = BattleSystem.instances.GetTrainerData(1);

        foreach (ItemData i in itemList)
            itemDictionary.Add(i.itemName, i);

        foreach(GameObject g in pokemonslots)
            g.SetActive(false);

        for (int i = 0; i < data.TrainerTeamList.Count; i++)
            pokemonslots[i].SetActive(true);

        BagController.instances.AddItem("Potion");
    }

    public void ActivateMenu()
    {
        onActive = !onActive;
        ResetOnMouseSprite();
        Invoke("InvokingMenu", 1);
    }

    void InvokingMenu()
        => menu.SetActive(onActive);

    void CheckItemType()
    {
        string type = it.data.itemType.ToUpper();

        switch (type)
        {
            case "RECOVERY": { UseItemRecovery(); } break;
            default: {TextSystem.instance.StartDialogue("Can't use that here");} break;
        }
    }

    void Update()
    {
        if (itemonMouse.gameObject.activeSelf)
            itemonMouse.rectTransform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
            ResetOnMouseSprite();

        if (doRecovery)
        {
            ps.pokemonData.health = Mathf.Lerp(ps.pokemonData.health, healthto, Time.deltaTime * (Mathf.Abs(it.data.healamount)*0.25f));

            if (ps.pokemonData.health == healthto || ps.pokemonData.health >= ps.pokemonData.HP)
                { doRecovery = false; }
        }
    }

    void ResetOnMouseSprite()
        => itemonMouse.gameObject.SetActive(false);

    public void AddItem(string itemtag)
    {
        if (haveItem.ContainsKey(itemtag))
        {
            GameObject ni = haveItem[itemtag];
            ItemSelection s = ni.GetComponent<ItemSelection>();
            ItemData d = itemDictionary[itemtag];

            // s.data = d;
            s.RefreshInfo();
            d.amount++;

        } else {
            GameObject ni = Instantiate(prefab, viewport);
            ItemSelection s = ni.GetComponent<ItemSelection>();
            ItemData d = itemDictionary[itemtag];

            ni.GetComponent<RectTransform>().transform.localPosition -= new Vector3(0.0f, 32.0f*haveItem.Count-1, 0.0f);
            s.data = d;
            ni.name = s.data.itemName;
            s.data.amount++;
            s.RefreshInfo();

            haveItem.Add(s.data.itemName, ni);
        }
    }

    public void SaveTargetPokemon(PokemonSelection ps)
    { 
        if (it == null)
            return;

        this.ps = ps;
        CheckItemType();
    }

    public void GetTargetItem(ItemSelection it)
    {
        this.it = it;
        itemonMouse.gameObject.SetActive(true);
        itemonMouse.sprite = it.data.icon;
        itemIcon.sprite = it.data.icon;
    }

    public void UseItemRecovery()
    {   
        if (ps.pokemonData.health >= ps.pokemonData.HP)
        {
            TextSystem.instance.StartDialogue($"{ps.pokemonData.nickname} health is already full!");
            return;   
        }

        ResetOnMouseSprite();
        
        it.data.amount--;
        it.RefreshInfo();
        healthto = ps.pokemonData.health + it.data.healamount;
        ActivateMenu();

        StartCoroutine(BattleSystem.instances.ItemPhase($"{data.trainerName} uses {it.data.itemName} on {ps.pokemonData.nickname}"));
    }
}
