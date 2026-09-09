using UnityEngine;
using System.IO;

[System.Serializable]
public class GameSaveData
{
    public float totalMoney = 1000f;
    public string selectedCar = "Starter";
    public int[] unlockedCars = new int[5];
    public int[][] carUpgrades = new int[5][];
    public Color[] carColors = new Color[5];
    public float maxSpeed = 0f;
    public float totalDistance = 0f;
    public float totalMoneyEarned = 0f;
}

public class SaveSystem : MonoBehaviour
{
    private string savePath;
    public GameSaveData saveData = new GameSaveData();

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/gamesave.json";
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to: " + savePath);
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            saveData = JsonUtility.FromJson<GameSaveData>(json);
            Debug.Log("Game loaded from: " + savePath);
        }
        else
        {
            Debug.Log("No save file found. Starting new game.");
            InitializeNewGame();
        }
    }

    private void InitializeNewGame()
    {
        saveData = new GameSaveData();
        saveData.unlockedCars[0] = 1; // Starter car unlocked
        SaveGame();
    }

    public void DeleteSaveData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            InitializeNewGame();
        }
    }
}
