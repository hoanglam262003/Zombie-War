using UnityEngine;

[System.Serializable]
public class DifficultyLevelData
{
    [Header("Difficulty")]

    [Tooltip("Difficulty min to apply this level.")]
    public float difficultyThreshold;

    [Header("Spawn")]

    public float spawnInterval = 3f;

    [Header("Zombie Amount")]

    public int smallZombiesAmount;
    public int bigZombiesAmount;
}