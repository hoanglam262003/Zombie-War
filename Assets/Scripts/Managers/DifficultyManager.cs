using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    [Header("Difficulty")]

    [SerializeField]
    private float difficultyIncreasePerSecond = 0.15f;

    [SerializeField]
    private float maxDifficulty = 20f;

    public float Difficulty { get; private set; } = 1f;

    public float ElapsedTime { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        ElapsedTime += Time.deltaTime;

        Difficulty +=
            difficultyIncreasePerSecond * Time.deltaTime;

        Difficulty = Mathf.Min(
            Difficulty,
            maxDifficulty);
    }
}