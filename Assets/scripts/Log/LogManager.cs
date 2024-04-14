using UnityEngine;
using System.IO;

public class LogManager : MonoBehaviour
{
    private static LogManager instance;
    private StreamWriter writer;
    private string logFilePath;
    private int enemiesKilled = 0;
    private int reachedStage = 1;

    public static LogManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LogManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    instance = obj.AddComponent<LogManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        logFilePath = Application.dataPath + "/log.txt";
        writer = new StreamWriter(logFilePath, true);
    }

    public void Log(string message)
    {
        writer.WriteLine(message);
        writer.Flush();
    }

    public void AddEnemyKilled()
    {
        enemiesKilled++;
        if(enemiesKilled == 1)
        {
            EventManager.TriggerEvent("firstMonsterKill", null);
        } else if(enemiesKilled == 5)
        {
            EventManager.TriggerEvent("kill5", null);
        } else if(enemiesKilled == 10)
        {
            EventManager.TriggerEvent("kill10", null);
        } else if(enemiesKilled == 15)
        {
            EventManager.TriggerEvent("kill15", null);
        }
    }

    public void AddStageReached()
    {
        reachedStage++;
    }

    public void WriteFinalLogs()
    {
        Log("Gegner getötet " + enemiesKilled);
        Log("Erreichte Stage " + reachedStage);
    }

    private void OnDestroy()
    {
        if (writer != null)
        {
            writer.Close();
        }
    }
}
