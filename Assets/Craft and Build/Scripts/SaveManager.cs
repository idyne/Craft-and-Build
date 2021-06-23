using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    public static void SaveData(this Player player)
    {
        SavePlayerData(player.Data);
    }
    public static void SavePlayerData(PlayerData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.save";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveData(this Construction construction)
    {
        SaveConstructionData(construction.Data);
    }

    public static void SaveConstructionData(ConstructionData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/construction_" + data.ID.ToString() + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void LoadData(this Player player)
    {
        player.Data = LoadPlayerData();
        if (player.Data == null)
            player.Data = new PlayerData();
    }

    public static PlayerData LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/player.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = (PlayerData)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        return null;
    }

    public static void LoadData(this Construction construction)
    {
        construction.Data = LoadConstructionData(construction.ConstructionID);
        if (construction.Data == null)
            construction.Data = new ConstructionData(construction.ConstructionID);
    }
    public static ConstructionData LoadConstructionData(int ID)
    {
        string path = Application.persistentDataPath + "/construction_" + ID.ToString() + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ConstructionData data = (ConstructionData)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        return null;
    }
}
