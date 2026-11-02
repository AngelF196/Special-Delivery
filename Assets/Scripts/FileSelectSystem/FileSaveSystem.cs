using UnityEngine;
using System.IO;  // Needed for working with files!! (reading and writing files to be exact)
using MessagePack;

public static class FileSaveSystem  // This class can't be a component of a GameObject
{
    public enum ActiveFile
    {
        NONE = 0, FILE1 = 1, FILE2 = 2, FILE3 = 3
    }

    public static void SaveFileData(PlayerMove player, LevelManager levelManager)
    {
        string savePath = Application.persistentDataPath + "/file1.sdf";  // Apparently, the extension can be whatever I want since I'm using binary formatting, so I'll use .sdf (Special Delivery File)
        // string savePath = "C:/SaveFileTests/file1.sdf";
        FileStream stream = new FileStream(savePath, FileMode.Create);
        
        FileSaveData data = new FileSaveData(player, levelManager);
        MessagePackSerializer.Serialize(stream, data);  // Converts the saved file data into bytes to write into the file
        string jsonSaveData = MessagePackSerializer.ConvertToJson(MessagePackSerializer.Serialize(data) );
        stream.Close();
        Debug.Log($"File saved to {savePath}");
        Debug.Log("Save file data in JSON: " + jsonSaveData);
    }

    public static FileSaveData LoadFileData()
    {
        string savePath = Application.persistentDataPath + "/file1.sdf";
        // string savePath = "C:/SaveFileTests/file1.sdf";

        if (File.Exists(savePath))
        {
            FileStream stream = new FileStream(savePath, FileMode.Open);

            FileSaveData fileSaveData = MessagePackSerializer.Deserialize<FileSaveData>(stream);  // Reads data from the save file, while converting it into a FileSaveData object
            stream.Close();
            return fileSaveData;
        }
        else
        {
            Debug.LogError("A save file doesn't exist at the following path: " + savePath);
            return null;
        }
    }
}
