using TMPro;
using UnityEngine.UI;

public class FullSizeProfileView : ProfilePresenterView {

    protected override void SetBestScore(TMP_Text field, int score) => field.text = $"Best score: {score}";
    protected override void SetCurrentScore(TMP_Text field, int score) => field.text = $"Score: {score}";
    protected override void SetProgress(Image image, float progress) => image.fillAmount = progress;
    protected override void SetLevel(TMP_Text field, int level) => field.text = level.ToString();
}
