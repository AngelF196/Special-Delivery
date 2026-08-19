using UnityEngine;
using System.IO;  // Needed for working with files!! (reading and writing files to be exact)
using MessagePack;

public static class FileSaveSystem  // This class can't be a component of a GameObject
{
    public static void SaveFileData(PlayerMove player)
    {
        string savePath = Application.persistentDataPath + "/file1.sdf";  // Apparently, the extension can be whatever I want since I'm using binary formatting, so I'll use .sdf (Special Delivery File)
        FileStream stream = new FileStream(savePath, FileMode.Create);
        
        FileSaveData data = new FileSaveData(player);
        MessagePackSerializer.Serialize(stream, data);  // Converts the saved file data into bytes to write into the file
        stream.Close();
    }

    public static FileSaveData LoadFileData()
    {
        string savePath = Application.persistentDataPath + "/file1.sdf";

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
