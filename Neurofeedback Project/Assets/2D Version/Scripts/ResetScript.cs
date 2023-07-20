using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScript : MonoBehaviour
{
    [SerializeField]
    private IntSO coins, theme;

    public UnlockableMatrix unlockableMatrix;
    private string unlockMatrixPath;

    private void Start()
    {
        unlockMatrixPath = $"{Application.persistentDataPath}/UnlockMatrix.json";

        if (File.Exists(unlockMatrixPath))
        {
            string json = File.ReadAllText(unlockMatrixPath);
            unlockableMatrix = JsonUtility.FromJson<UnlockableMatrix>(json);
        }
    }

    public void resetGame()
    {
        coins.Value = 0;
        theme.Value = 0;
        unlockableMatrix.ownsSpaceTheme = false;
        saveJson();
        SceneManager.LoadScene(0);
    }

    private void saveJson()
    {
        string json = JsonUtility.ToJson(unlockableMatrix);
        File.WriteAllText(unlockMatrixPath, json);
    }
}
