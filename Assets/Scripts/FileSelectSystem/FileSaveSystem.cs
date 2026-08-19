using UnityEngine;
using System.IO;  // Needed for working with files!! (reading and writing files to be exact)

public static class FileSaveSystem  // This class can't be a component of a GameObject
{
    public static void SaveFileData(PlayerMove player)
    {
        // BinaryFormatter formatter = new BinaryFormatter();
        string savePath = Application.persistentDataPath + "/file1.sdf";  // Apparently, the extension can be whatever I want since I'm using binary formatting, so I'll use .sdf (Special Delivery File)
        FileStream stream = new FileStream(savePath, FileMode.Create);

        FileSaveData data = new FileSaveData(player);
        // formatter.Serialize(stream, data);  // Using binary formatter, save the data object into file stream which contains the save path
        stream.Close();
    }

    public static FileSaveData LoadFileData()
    {
        string savePath = Application.persistentDataPath + "/file1.sdf";

        if (File.Exists(savePath))
        {
            // BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            // FileSaveData fileSaveData = (FileSaveData) formatter.Deserialize(stream);  // Deserialize returns a plain object, which is why the casting is needed
            stream.Close();
            return null;
        }
        else
        {
            throw new FileNotFoundException("A save file doesn't exist at the following path: ", savePath);
        }
    }
}
