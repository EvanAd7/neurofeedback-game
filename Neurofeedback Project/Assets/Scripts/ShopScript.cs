using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    [SerializeField]
    private IntSO coins, theme;

    public UnlockableMatrix unlockableMatrix;
    private string unlockMatrixPath;

    public Text coinsText;
    public Text carButtontText;
    public Text spaceButtonText;

    // Start is called before the first frame update
    void Start()
    {
        coinsText.text = coins.Value.ToString();

        unlockMatrixPath = $"{Application.persistentDataPath}/UnlockMatrix.json";

        if (File.Exists(unlockMatrixPath))
        {
            string json = File.ReadAllText(unlockMatrixPath);
            unlockableMatrix = JsonUtility.FromJson<UnlockableMatrix>(json);
        }
        
        if (theme.Value == 0 & unlockableMatrix.ownsSpaceTheme)
        {
            spaceButtonText.text = "Equip";
            carButtontText.text = "Equipped";
        }
        else if (theme.Value == 1)
        {
            spaceButtonText.text = "Equipped";
            carButtontText.text = "Equip";
        }

    }

    //method to equip new theme
    public void equipTheme(string themeName)
    {
        if (themeName.Equals("car"))
        {
            theme.Value = 0;
            carButtontText.text = "Equipped";
            if (unlockableMatrix.ownsSpaceTheme)
            {
                spaceButtonText.text = "Equip";
            }
        }
        if (themeName.Equals("space"))
        {
            if (unlockableMatrix.ownsSpaceTheme)
            {
                theme.Value = 1;
                spaceButtonText.text = "Equipped";
                carButtontText.text = "Equip";
            }
            else if (coins.Value >= 1000 & !unlockableMatrix.ownsSpaceTheme)
            {
                coins.Value -= 1000;
                coinsText.text = coins.Value.ToString();
                theme.Value = 1;
                unlockableMatrix.ownsSpaceTheme = true;
                saveJson();
                spaceButtonText.text = "Equipped";
                carButtontText.text = "Equip";
            }
        }
    }

    private void saveJson()
    {
        string json = JsonUtility.ToJson(unlockableMatrix);
        File.WriteAllText(unlockMatrixPath, json);
    }

    //method to start race on button click
    public void startRace()
    {
        SceneManager.LoadScene(1);
    }
}
