using UnityEngine;
public class EndScene : MonoBehaviour
{
    private void Awake()
    {
        InputSystemController.InputSystem.Common.AnyKey.performed += (_) =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit( 0 );

#endif
        };
    }
}