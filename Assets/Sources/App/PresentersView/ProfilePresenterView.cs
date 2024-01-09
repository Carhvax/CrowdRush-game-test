using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ProfilePresenterView : MonoBehaviour, ILayoutPresenter {
    
    [SerializeField] private TMP_Text _best;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private TMP_Text _level;
    
    [SerializeField] private Image _playerProgress;
    
    public void OnShowLayout() {}

    public void UpdateBestScore(int score) {
        if(_best) SetBestScore(_best, score);
    }

    protected abstract void SetBestScore(TMP_Text field,int score);

    public void UpdateCurrentScore(int score) {
        if(_score) SetCurrentScore(_score, score);
    }
    
    protected abstract void SetCurrentScore(TMP_Text field, int score);

    public void UpdateProgress(float progress) {
        if(_playerProgress) SetProgress(_playerProgress, progress);
    }
    
    public void UpdateLevel(int level) {
        if(_level) SetLevel(_level, level);
    }
    
    protected abstract void SetProgress(Image image, float progress);
    
    protected abstract void SetLevel(TMP_Text field, int level);
    
    public void OnHideLayout() {}
}