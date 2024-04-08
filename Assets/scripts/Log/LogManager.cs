using UnityEngine;
using System.IO;

public class LogManager : MonoBehaviour
{
    private static LogManager instance;
    private StreamWriter writer;
    private string logFilePath;

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

    private void OnDestroy()
    {
        if (writer != null)
        {
            writer.Close();
        }
    }
}
