using UnityEngine;

public enum GameState
{
    Start,        // Trạng thái bắt đầu
    InProgress,   // Trạng thái đang chơi
    GameOver,     // Trạng thái kết thúc
    Victory       // Trạng thái thắng
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public WaveSpawner waveSpawner;
    public UnitMovement UMovement;
    public Unit Unit;

    // GameState
    public GameState currentState;
    public int currentWave = 0;
    public int maxWave = 10;              // Số wave tối đa để thắng

    // Unit spawning
    public GameObject unitPrefab;         // Prefab của Unit sẽ được spawn
    public Transform unitSpawnPoint;      // Vị trí spawn của Unit
    public int maxUnits = 5;             // Giới hạn số lượng Unit có thể spawn
    public float spawnRadius = 2f;        // Bán kính kiểm tra vùng spawn (để tránh đè lên nhau)
    public LayerMask unitLayerMask;       // Layer của Unit để tránh va chạm
    private int currentUnitCount = 0;     // Số lượng Unit đã spawn
    public int zombieKillCount = 0;  // Số lượng zombie đã giết
    public int requiredKillsForSpawn = 10; // Số lượng zombie cần giết để spawn Unit
    public int maxWaveToSpawnUnits = 5;  // Số wave cần đạt được để spawn thêm Unit
    public int killneedtowin = 10;
    [SerializeField] public int zombiesKillScore = 0;
    // UI
    public GameObject victoryUI;          // UI hiển thị khi thắng
    public GameObject gameOverUI;         // UI hiển thị khi game over
    public GameObject ButtonReplay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Bắt đầu game
    private void Start()
    {
        Init();
        SetGameState(GameState.Start);
    }

    public void Init()
    {
        if (waveSpawner != null)
        {
            waveSpawner.Init();
        }
        if (UMovement != null)
        {
            UMovement.Init();
        }
        if (Unit != null)
        {
            Unit.Init();
        }
    }

    private void Update()
    {
        if (currentState == GameState.InProgress)
        {

            CheckGameOverCondition();
            WaveCtroller();
        }
        CheckWinCondition();
        CheckUnit();
        CheckWin();
        UnitSelectionManager.Instance.allUnitsList.RemoveAll(unit => unit == null);
    }


    public void OnUnitKilled(GameObject unit)
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(unit);
        if (UnitSelectionManager.Instance.allUnitsList.Count == 0)
        {
            SetGameState(GameState.GameOver);
        }
    }

    public void CheckWin()
    {
        if (killneedtowin <= zombiesKillScore)
        {
            SetGameState(GameState.Victory);
        }
    }

    private void CheckWinCondition()
    {
        if (currentWave >= maxWave)
        {
            SetGameState(GameState.Victory);
        }
    }

    private void CheckGameOverCondition()
    {
        if (UnitSelectionManager.Instance.allUnitsList.Count == 0)
        {
            SetGameState(GameState.GameOver);
        }
    }

    public void SetGameState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.Start:
                currentWave = 1;
                ButtonReplay.SetActive(false);  // Ẩn nút replay khi bắt đầu lại
                gameOverUI.SetActive(false);    // Ẩn UI GameOver khi bắt đầu lại
                victoryUI.SetActive(false);    // Ẩn UI Victory khi bắt đầu lại
                Time.timeScale = 1;            // Đảm bảo thời gian không bị dừng
                break;

            case GameState.InProgress:
                ButtonReplay.SetActive(false);  // Ẩn nút replay khi game đang chơi
                gameOverUI.SetActive(false);    // Ẩn UI GameOver khi game đang chơi
                victoryUI.SetActive(false);    // Ẩn UI Victory khi game đang chơi
                Time.timeScale = 1;            // Đảm bảo thời gian không bị dừng
                break;

            case GameState.GameOver:
                Time.timeScale = 1;            // Đảm bảo thời gian không bị dừng
                ButtonReplay.SetActive(true);  // Hiển thị nút replay khi game over
                gameOverUI.SetActive(true);    // Hiển thị UI GameOver
                break;

            case GameState.Victory:
                Time.timeScale = 1;            // Đảm bảo thời gian không bị dừng
                ButtonReplay.SetActive(true);  // Hiển thị nút replay khi thắng
                victoryUI.SetActive(true);    // Hiển thị UI Victory
                break;
        }

    }

    public void OnZombieKilled()
    {
        zombiesKillScore++;
        UIManager.instance.UpdateKillScore(zombiesKillScore);
        zombieKillCount++;
        if (zombieKillCount >= requiredKillsForSpawn)
        {
            if (currentUnitCount < maxUnits)
            {
                SpawnUnit();
                zombieKillCount = 0;
            }
            else
            {
                Debug.Log("Max number of units reached. No more units can be spawned.");
            }
        }
        else
        {
            Debug.Log("need more kill");
        }
    }
    public void WaveCtroller()
    {
        if (currentWave > maxWaveToSpawnUnits)
        {
            maxUnits += 2;
            // Tăng số lượng Unit có thể spawn khi wave đạt mốc nhất định
            requiredKillsForSpawn += 5; // Có thể tăng thêm số lượng yêu cầu giết zombie
        }
    }

    private void SpawnUnit()
    {
        Vector3 spawnPosition = unitSpawnPoint.position;

        // Kiểm tra vị trí spawn để tránh đè lên các Unit khác
        bool positionValid = false;
        int attempts = 0;

        while (!positionValid && attempts < 10)
        {
            Collider[] colliders = Physics.OverlapSphere(spawnPosition, spawnRadius, unitLayerMask);

            if (colliders.Length == 0)
            {
                positionValid = true;
            }
            else
            {
                spawnPosition = unitSpawnPoint.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
            }

            attempts++;
        }

        if (positionValid)
        {
            GameObject newUnit = Instantiate(unitPrefab, spawnPosition, unitSpawnPoint.rotation);
            UnitMovement UMovement = newUnit.GetComponent<UnitMovement>();
            Unit Unit = newUnit.GetComponent<Unit>();
            UMovement.Init();
            Unit.Init();
            //UnitSelectionManager.Instance.allUnitsList.Add(newUnit); // Thêm Unit mới vào danh sách của UnitSelectionManager

            currentUnitCount++;



            Debug.Log("Unit spawned at " + spawnPosition);
        }
        else
        {
            Debug.LogWarning("Unable to find a valid spawn position after several attempts.");
        }
    }

    public void StartNewWave()
    {
        currentWave++;
        SetGameState(GameState.InProgress);  // Chuyển sang trạng thái đang chơi
        Debug.Log("Starting wave " + currentWave);
    }
    public void CheckUnit()
    {
        if (unitSpawnPoint == null)
        {
            foreach (var unit in UnitSelectionManager.Instance.allUnitsList)
            {
                if (unit != null && unit.activeInHierarchy)
                {
                    unitSpawnPoint = unit.transform;
                    break;
                }
            }
        }
        if (UMovement == null)
        {
            foreach (var unit in UnitSelectionManager.Instance.allUnitsList)
            {
                if (unit != null && unit.activeInHierarchy)
                {
                    UMovement = unit.GetComponent<UnitMovement>();
                    break;
                }
            }
        }
        if (Unit == null)
        {
            foreach (var unit in UnitSelectionManager.Instance.allUnitsList)
            {
                if (unit != null && unit.activeInHierarchy)
                {
                    Unit = unit.GetComponent<Unit>();
                    break;
                }
            }
        }
        if (unitPrefab == null)
        {
            foreach (var unit in UnitSelectionManager.Instance.allUnitsList)
            {
                if (unit != null && unit.activeInHierarchy)
                {
                    unitPrefab = unit;
                    break;
                }
            }
        }

    }
}
