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
    public string currentName;
    public List <AllHighscore> allHighscore = new List<AllHighscore>();
    public List <AllNames> names = new List<AllNames>();
    public int amountLevel;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < amountLevel; i++)
        {

            CreateDirectory(i);
            if (Exists("SaveFiles", i))
            {
                NamesByteSavefile saveFile = LoadNames(i);
                names.Add((AllNames)ByteArrayToObject(saveFile.saveFile));
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

                        if(o < allHighscore[i].levelPlayed.Count)
                        {
                            Highscore temp = LoadScore(names[i].names[o], i);
                            allHighscore[i].levelPlayed[o] = temp.level;
                        }
                        else
                        {
                            Highscore temp = LoadScore(names[i].names[o], i);
                            allHighscore[i].levelPlayed.Add(temp.level);
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

    public bool AddScore(Highscore score, string name,int level)
    {
        if (allHighscore[level] != null)
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
                if(i < allHighscore[level].levelPlayed.Count)
                {
                    allHighscore[level].levelPlayed[i] = level+1;
                }
                else
                {
                    allHighscore[level].levelPlayed.Add(level+1);
                }
            }
            if (exists)
            {
                if(allHighscore[level].scores[index].score > score.score)
                {
                    return false;
                }
                else
                {
                    allHighscore[level].scores[index] = score;
                    return true;
                }
            }
            else
            {
                allHighscore[level].scores.Add(score);
                allHighscore[level].names.Add(name);
                return true;
            }
        }
        return false;
    }

    public int LastLevel()
    {
        int lvl = 0;
        for (int i = 0; i < allHighscore.Count; i++)
        {
            int test = allHighscore[i].names.IndexOf(currentName);

                if(test>= 0)
            {
                if(allHighscore[i].levelPlayed.Count != 0)
                {
                    lvl = allHighscore[i].levelPlayed[test];
                }

            }

        }
        lvl++;
        return lvl;
    }
    private bool Exists(string name, int level)
    {
        level++;
        return File.Exists(Application.persistentDataPath +"/Level"+ level.ToString() + "/SavedGame_" + name + ".xml");
    }

    private void SaveScore(Highscore tS,string name,int level, int lastLevel)
    {
        level++;
        tS.level = lastLevel;
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

    public void SaveLastName(string newName)
    {
        LastName name = new LastName();
        name.name = newName;
        var serializer = new XmlSerializer(typeof(LastName));
        using (var stream = new FileStream(Application.persistentDataPath + "/LN.xml", FileMode.Create))
        {
            serializer.Serialize(stream, name);
        }
    }

    public string LoadLastName()
    {
        if(File.Exists(Application.persistentDataPath + "/LN.xml"))
        {
            var serializer = new XmlSerializer(typeof(LastName));
            using (var stream = new FileStream(Application.persistentDataPath + "/LN.xml", FileMode.Open))
            {
                LastName save = serializer.Deserialize(stream) as LastName;
                    return save.name;
            }
        }
        return "";
        
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
                SaveScore(allHighscore[i].scores[o], allHighscore[i].names[o],i,allHighscore[i].levelPlayed[o]);
            }
            SaveNames(saveFile,i);
        }
        
    }

    public void ResetSaves()
    {
        for (int i = 0; i < amountLevel; i++)
        {
                Directory.Delete(Application.persistentDataPath + "/Level" + (i + 1).ToString(), true);
        }
        allHighscore.Clear();
        Awake();
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
