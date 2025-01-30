using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestButton : MonoBehaviour
{
    [SerializeField] private Button _button; 
    
    private void Awake()
    {
        _button.onClick.AddListener(() => {SceneManager.LoadScene(1, LoadSceneMode.Single);} );
    }
}
