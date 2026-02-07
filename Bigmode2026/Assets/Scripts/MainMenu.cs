using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // exists so that we can add some kind of animation before the next scene loads
    [SerializeField, Min(0.01f)] float loadDelay;  

    IEnumerator PlayAfterDelayCoroutine() 
    {
        yield return new WaitForSeconds(loadDelay);

        // loads the scene asynchronously to prevent stutter
        _ = SceneManager.LoadSceneAsync("GameScene");
        yield return null;
    }

    public void Play() 
    {
        StartCoroutine(PlayAfterDelayCoroutine());
        MusicManager.Instance.PlayMusic("sigma");

    }

    public void Quit()
    {
        // stops the exe application
        Application.Quit();

#if UNITY_EDITOR
        // stops the in-editor application
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
