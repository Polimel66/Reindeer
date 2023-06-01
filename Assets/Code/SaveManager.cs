using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager
{
    public static string LastCheckPointName { get; private set; } = null;
    private static string PreviousCheckPointName = "";
    public static List<int> TaskNumbersToSet = new List<int>();
    public static List<string> CollectedSmells = new List<string>();
    public static List<string> CollectedTaskTriggers = new List<string>();
    public static void SaveGame(bool isExit)
    {
        if (LastCheckPointName != null)
        {
            PlayerPrefs.SetString("LastCheckPoint", LastCheckPointName);
            //PlayerPrefs.Save();
            if (!LastCheckPointName.Equals(PreviousCheckPointName))
            {
                PreviousCheckPointName = LastCheckPointName;
                if (CollectedSmells.Count != 0)
                {
                    var smells = CollectedSmells[0];
                    for (var i = 1; i < CollectedSmells.Count; i++)
                    {
                        smells += ";" + CollectedSmells[i];
                    }
                    PlayerPrefs.SetString("CollectedSmells", smells);
                }
                if (CollectedTaskTriggers.Count != 0)
                {
                    var tasks = CollectedTaskTriggers[0];
                    for (var i = 1; i < CollectedTaskTriggers.Count; i++)
                    {
                        tasks += ";" + CollectedTaskTriggers[i];
                    }
                    PlayerPrefs.SetString("CollectedTaskTriggers", tasks);
                }
                if (TaskNumbersToSet.Count != 0)
                {
                    var numbers = TaskNumbersToSet[0].ToString();
                    for (var i = 1; i < TaskNumbersToSet.Count; i++)
                    {
                        numbers += " " + TaskNumbersToSet[i].ToString();
                    }
                    PlayerPrefs.SetString("TaskNumbersToSet", numbers);

                }
            }
        }
        if (isExit)
        {
            CollectedSmells.Clear();
            CollectedTaskTriggers.Clear();
            TaskNumbersToSet.Clear();
            LastCheckPointName = null;
            PreviousCheckPointName = "";
        }
        
        PlayerPrefs.Save();
    }

    public static void LoadGame()
    {
        if (PlayerPrefs.HasKey("LastCheckPoint"))
        {
            LastCheckPointName = PlayerPrefs.GetString("LastCheckPoint");
            PreviousCheckPointName = LastCheckPointName;
        }
        else
        {
            //LastCheckPointName = "Map1CheckPoint0";
        }
        if (PlayerPrefs.HasKey("CollectedSmells"))
        {
            var smells = PlayerPrefs.GetString("CollectedSmells").Split(";");
            foreach(var smell in smells)
            {
                CollectedSmells.Add(smell);
            }
        }
        if (PlayerPrefs.HasKey("CollectedTaskTriggers"))
        {
            var tasks = PlayerPrefs.GetString("CollectedTaskTriggers").Split(";");
            foreach (var task in tasks)
            {
                CollectedTaskTriggers.Add(task);
            }
        }
        if (PlayerPrefs.HasKey("TaskNumbersToSet"))
        {
            var numbers = PlayerPrefs.GetString("TaskNumbersToSet").Split();
            foreach(var number in numbers)
            {
                TaskNumbersToSet.Add(int.Parse(number));
            }
        }
    }

    public static void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
        CollectedSmells.Clear();
        CollectedTaskTriggers.Clear();
        TaskNumbersToSet.Clear();
        LastCheckPointName = null;
        PreviousCheckPointName = "";
    }

    public static bool isAnySaves()
    {
        return PlayerPrefs.HasKey("LastCheckPoint");
    }

    public static void SetLastCheckPointName(string name)
    {
        if (name != null && !name.Equals(""))
        {
            LastCheckPointName = name;
            SaveGame(false);
        }
    }

    public static void AddSmell(string smell)
    {
        if (smell != null && !smell.Equals(""))
        {
            CollectedSmells.Add(smell);
        }
    }

    public static void AddTask(string task)
    {
        if (task != null && !task.Equals(""))
        {
            CollectedTaskTriggers.Add(task);
        }
    }

    public static void AddTaskNumberToSet(int number)
    {
        if (number > 0)
        {
            TaskNumbersToSet.Add(number);
        }
    }
}
