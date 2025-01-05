using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int monsterCount = 0;

    public Transform gameTextUI;
    public Transform gameButtonUI;
    
    private SceneManager sceneManager;
    void Start()
    {
        Time.timeScale = 1;
        
        Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

        foreach (GameObject rootObject in rootObjects)
        {
            if (rootObject.CompareTag("MeleeMonster") || rootObject.CompareTag("RangeMonster"))
                monsterCount++;
        }
        
        gameTextUI.gameObject.SetActive(false);
        gameButtonUI.gameObject.SetActive(false);
        
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
    }

    public void SubMonsterCount()
    {
        monsterCount--;

        if (monsterCount <= 0)
        {
            StartCoroutine(GameClear());
        }
    }

    public void PlayerDead()
    {
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1.5f);
        
        Time.timeScale = 0;
        gameTextUI.gameObject.SetActive(true);
        gameTextUI.GetComponent<TMP_Text>().SetText("Game Over..");
        
        gameButtonUI.gameObject.SetActive(true);
        gameButtonUI.GetComponent<Button>().onClick.AddListener(sceneManager.ChangeToMainTitle);
    }
    
    private IEnumerator GameClear()
    {
        yield return new WaitForSeconds(0.5f);
        
        Time.timeScale = 0;
        gameTextUI.gameObject.SetActive(true);
        gameTextUI.GetComponent<TMP_Text>().SetText("Game Clear!");
        
        gameButtonUI.gameObject.SetActive(true);
        gameButtonUI.GetComponent<Button>().onClick.AddListener(sceneManager.ChangeToMainTitle);
    }
}