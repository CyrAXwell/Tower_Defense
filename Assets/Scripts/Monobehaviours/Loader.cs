using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
