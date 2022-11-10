using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Trainer Data", menuName = "Data/Trainer Data")]
public class TrainerData : ScriptableObject
{
    [System.Serializable]
    public class TrainerTeam
    {
        public CapturedPokemonData pokemon;
        public MovesData[] moveset = new MovesData[4];
    }

    [Header("Base Data")]
    public string trainerName;
    public List<TrainerTeam> TrainerTeamList;
}
