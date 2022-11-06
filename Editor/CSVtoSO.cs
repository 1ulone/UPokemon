using UnityEngine;
using UnityEditor;
using System.IO;

public class CSVtoSO
{
    private static string GEN4CSVpath = "/Editor/PokedexSheetGen1-4.csv";
    private static string GEN4MovesPath = "/Editor/movesGen1-4.csv";

    [MenuItem("Utilities/Generate Data")]
    public static void GenerateData()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + GEN4CSVpath);

        foreach(string s in allLines)
        {
            string[] splitData = s.Split(';');
            int sPokedex, hStep;
            int.TryParse(splitData[0], out sPokedex);
            int.TryParse(splitData[18], out hStep);

            PokemonConstantData pData = ScriptableObject.CreateInstance<PokemonConstantData>();
            pData.sinnohDex = sPokedex;
            pData.nationalDex = int.Parse(splitData[1]);
            pData.Pokemonname = splitData[2];

            pData.maxhp = int.Parse(splitData[3]);
            pData.attack = int.Parse(splitData[4]);
            pData.defense = int.Parse(splitData[5]);
            pData.specialattack = int.Parse(splitData[6]);
            pData.specialdefense = int.Parse(splitData[7]);
            pData.speed = int.Parse(splitData[8]);
            pData.totalstats = int.Parse(splitData[9]);

            pData.type1 = Enum.TryParse<pokemonType>(splitData[10], out pokemonType);
            pData.type2 = splitData[11];
            pData.ability1 = splitData[12];
            pData.ability2 = splitData[13];
            pData.hiddenAbility = splitData[14];

            pData.mass = decimal.Parse(splitData[15]);
            pData.evWorth = splitData[16];
            pData.expv = int.Parse(splitData[17]);
            pData.hatchStep = hStep;
            pData.eggGroup[0] = splitData[19];
            pData.eggGroup[1] = splitData[20];

            AssetDatabase.CreateAsset(pData, $"Assets/Data/Pokemon/{pData.NationalDex}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate MovesData")]
    public static void GenerateMovesData()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + GEN4MovesPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(';');
            MovesData mData = ScriptableObject.CreateInstance<MovesData>();

            int pp, pow, acc;
            int.TryParse(splitData[5], out pp);
            int.TryParse(splitData[6], out pow);
            int.TryParse(splitData[7], out acc);

            mData.id = int.Parse(splitData[0]);

            mData.movesName = splitData[1];
            mData.typeName = splitData[2];
            mData.movesCategory = splitData[3];
            mData.contestType = splitData[4];

            mData.maxPowerPoints = pp;
            mData.power = pow;
            mData.accuracy = acc;

            AssetDatabase.CreateAsset(mData, $"Assets/Data/Moves/{mData.id}_{mData.movesName}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}
