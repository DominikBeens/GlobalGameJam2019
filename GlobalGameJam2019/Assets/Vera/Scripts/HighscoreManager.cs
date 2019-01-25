using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager instance;
    private string tempName;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void Save(Highscore tS)
    {
        SaveScore(tS);
    }

    [KeyCommand(KeyCode.L, PressType.KeyPressType.Down)]
    void Load()
    {
        Highscore test = LoadSave();
        print(test.score);
    }

    private void SaveScore(Highscore tS)
    {
        ByteSave toSave = new ByteSave();
        toSave.save = ObjectToByteArray(tS);
        var serializer = new XmlSerializer(typeof(ByteSave));
        using (var stream = new System.IO.FileStream(Application.persistentDataPath + "/SavedGame_" + tempName + ".xml", FileMode.Create))
        {
            serializer.Serialize(stream, toSave);
        }
    }


    private Highscore LoadSave()
    {
        var serializer = new XmlSerializer(typeof(ByteSave));
        using (var stream = new System.IO.FileStream(Application.persistentDataPath + "/SavedGame_" + tempName + ".xml", FileMode.Open))
        {
            ByteSave save = serializer.Deserialize(stream) as ByteSave;
            Highscore score = (Highscore)ByteArrayToObject(save.save);
            return score;
        }
    }


    private byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
        {
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    private object ByteArrayToObject(byte[] bytes)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();
        memStream.Write(bytes, 0, bytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        object obj = binForm.Deserialize(memStream);

        return obj;
    }
}
