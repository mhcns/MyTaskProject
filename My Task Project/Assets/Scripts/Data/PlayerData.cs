using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public int slotId;
    public string[] item;
}

[System.Serializable]
public class SaveData
{
    public List<InventorySlot> inventory;
}

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/save.json";

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("Saved in: " + path);
    }

    public static SaveData Load()
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("File not found, saving a new one");
            return new SaveData();
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }
}
