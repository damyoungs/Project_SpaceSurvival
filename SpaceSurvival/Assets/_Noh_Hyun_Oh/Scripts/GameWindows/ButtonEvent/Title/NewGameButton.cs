using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// Ÿ��Ʋ���� ����� ��ư ���ӽ��� ȭ�� �������� ����
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
