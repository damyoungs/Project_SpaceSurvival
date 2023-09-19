using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// 타이틀에서 사용할 버튼 게임시작 화면 정해지면 수정
/// </summary>
public class NewGameButton: MonoBehaviour
{
    [SerializeField]
    EnumList.SceneName sceneName;
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadScene_NewGame);
    }
    public void LoadScene_NewGame()
    {
        // LoadingScene.SceneLoading(sceneName);
        SceneManager.LoadScene("Ship_");
    }
}
