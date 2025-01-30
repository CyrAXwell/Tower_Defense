using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _palyButton;
    [SerializeField] private Button _quitGameButton;

    public void Initialize()
    {
        Show();
        
        _palyButton.onClick.AddListener(() => { OnPlayButton(); });  
        _quitGameButton.onClick.AddListener(() => { Application.Quit(); }); 
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnPlayButton()
    {
        Hide();
        GameManager.Instance.UnpasueGame();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
