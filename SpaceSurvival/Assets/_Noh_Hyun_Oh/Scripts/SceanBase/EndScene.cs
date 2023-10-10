using UnityEngine;
public class EndScene : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown) 
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit( 0 );

#endif
        }
    }
}
