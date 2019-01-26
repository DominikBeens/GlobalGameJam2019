using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager instance;
    private string tempName;
    [SerializeField] private List< AllHighscore> allHighscore = new List<AllHighscore>();
    [SerializeField] private List<AllNames> names = new List<AllNames>();
    [SerializeField] private int amountLevel;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        for (int i = 0; i < amountLevel; i++)
        {

            CreateDirectory(i);
            if (Exists("SaveFiles", i))
            {
                NamesByteSavefile saveFile = LoadNames(i);
                print(saveFile);
                byte[] test = saveFile.saveFile;
                print(test.Length);
                AllNames tes = (AllNames) ByteArrayToObject(test);
                print(tes);
                names.Add(tes);
                print(LoadNames(i));
                if (i < allHighscore.Count)
                {
                    if (allHighscore[i] == null)
                    {
                        allHighscore[i] = new AllHighscore();
                    }
                }
                else
                {
                    allHighscore.Add(new AllHighscore());
                }

                if(names[i] == null)
                {
                    names[i] = new AllNames();
                }
                allHighscore[i].names = names[i].names;

                for (int o = 0; o < names[i].names.Count; o++)
                {
                    if (Exists(names[i].names[o],i))
                    {
                        if (o < allHighscore[i].scores.Count)
                        {
                            allHighscore[i].scores[o] = LoadScore(names[i].names[o],i);
                        }
                        else
                        {
                            allHighscore[i].scores.Add(LoadScore(names[i].names[o],i));
                        }
                    }
                }
            }
            else
            {
                if (i < allHighscore.Count)
                {
                    if (allHighscore[i] == null)
                    {
                        allHighscore[i] = new AllHighscore();
                    }
                }
                else
                {
                    allHighscore.Add(new AllHighscore());
                }
                if (i < names.Count)
                {
                    names[i] = new AllNames();
                }
                else
                {
                    names.Add(new AllNames());
                }
                
            }
        }

    }

    private void CreateDirectory(int level)
    {
        level++;
        if(!File.Exists(Application.persistentDataPath + "/Level" + level.ToString()))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Level" + level.ToString());
        }
    }

    public void AddScore(Highscore score, string name,int level)
    {
        if (allHighscore != null)
        {
            bool exists = false;
            int index = -1;
            for (int i = 0; i < allHighscore[level].names.Count; i++)
            {
                if(allHighscore[level].names[i] == name)
                {
                    exists = true;
                    index = i;
                }
            }
            if (exists)
            {
                if(allHighscore[level].scores[index].score > score.score)
                {
                    return;
                }
                else
                {
                    allHighscore[level].scores[index] = score;
                }
            }
            else
            {
                allHighscore[level].scores.Add(score);
                allHighscore[level].names.Add(name);
            }
        }
    }
    private bool Exists(string name, int level)
    {
        level++;
        return File.Exists(Application.persistentDataPath +"/Level"+ level.ToString() + "/SavedGame_" + name + ".xml");
    }

    private void SaveScore(Highscore tS,string name,int level)
    {
        level++;
        ByteSave toSave = new ByteSave();
        toSave.save = ObjectToByteArray(tS);
        var serializer = new XmlSerializer(typeof(ByteSave));
        using (var stream = new FileStream(Application.persistentDataPath + "/Level" + level.ToString() + "/SavedGame_" + name + ".xml", FileMode.Create))
        {
            serializer.Serialize(stream, toSave);
        }
    }


    private Highscore LoadScore(string name, int level)
    {
        level++;
        var serializer = new XmlSerializer(typeof(ByteSave));
        using (var stream = new FileStream(Application.persistentDataPath + "/Level" + level.ToString() + "/SavedGame_" + name + ".xml", FileMode.Open))
        {
            ByteSave save = serializer.Deserialize(stream) as ByteSave;
            Highscore score = (Highscore)ByteArrayToObject(save.save);
            return score;
        }
    }

    private void SaveNames(NamesByteSavefile tS, int level)
    {
        level++;
        var serializer = new XmlSerializer(typeof(NamesByteSavefile));
        using (var stream = new FileStream(Application.persistentDataPath + "/Level" + level.ToString() + "/SavedGame_SaveFiles.xml", FileMode.Create))
        {
            serializer.Serialize(stream, tS);
        }
    }

    private NamesByteSavefile LoadNames(int level)
    {
        level++;
        var serializer = new XmlSerializer(typeof(NamesByteSavefile));
        using (var stream = new FileStream(Application.persistentDataPath + "/Level" + level.ToString() + "/SavedGame_SaveFiles.xml", FileMode.Open))
        {
            NamesByteSavefile save = serializer.Deserialize(stream) as NamesByteSavefile;
            return save;
        }
    }

    private void OnApplicationQuit()
    {
        NamesByteSavefile saveFile = new NamesByteSavefile();
        for (int i = 0; i < amountLevel; i++)
        {
            names[i].names = allHighscore[i].names;
            saveFile.saveFile = ObjectToByteArray(names[i]);

            for (int o = 0; o < allHighscore[i].names.Count; o++)
            {
                SaveScore(allHighscore[i].scores[o], allHighscore[i].names[o],i);
            }
            SaveNames(saveFile,i);
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
