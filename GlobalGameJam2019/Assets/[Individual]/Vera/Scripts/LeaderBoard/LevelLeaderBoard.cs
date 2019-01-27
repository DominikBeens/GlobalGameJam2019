using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LevelLeaderBoard
{
    public List<int> score = new List<int>();
    public List<string> playerName = new List<string>();

    public void Sort(List<Highscore> _score, List<string> names)
    {
        if (_score.Count != 0 || names.Count != 0)
        {
            List<int> tempScore = new List<int>();
            List<string> tempNames = new List<string>();
            for (int i = 0; i < names.Count; i++)
            {
                tempNames.Add(names[i]);
            }
            for (int i = 0; i < _score.Count; i++)
            {
                tempScore.Add(_score[i].score);
            }
            List<int> sortedScore = new List<int>();
            List<string> sortedName = new List<string>();

            
            while (tempScore.Count != 0 || sortedScore.Count == 10)
            {
                int highest = 0;
                int index = 0;
                for (int i = 0; i < tempScore.Count; i++)
                {
                    if (tempScore[i] > highest)
                    {
                        index = i;
                        highest = tempScore[i];
                    }
                }
                sortedScore.Add(tempScore[index]);
                sortedName.Add(tempNames[index]);
                tempScore.RemoveAt(index);
                tempNames.RemoveAt(index);
            }
            score = sortedScore;
            playerName = sortedName;
        }
    }
}
