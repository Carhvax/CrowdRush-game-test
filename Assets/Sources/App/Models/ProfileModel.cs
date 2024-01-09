public class ProfileModel : IAppModel {
    
    public IObservableValue<int> BestScore { get; } = new ObservableValue<int>(0);
    public IObservableValue<int> Score { get; } = new ObservableValue<int>(0);
    
    public IObservableValue<int> Experience { get; } = new ObservableValue<int>(0);
    public IObservableValue<int> Level { get; } = new ObservableValue<int>(1);
    public IObservableValue<float> Progress { get; } = new ObservableValue<float>(0);
    public IObservableValue<bool> Win { get; } = new ObservableValue<bool>(false);

    public ProfileState State {
        
        get => new ProfileState() {
            level = Level.Value,
            bestScore = BestScore.Value,
            experience = Experience.Value,
            progress = Progress.Value,
        };
        set {
            Level.Value = value.level;
            BestScore.Value = value.bestScore;
            Experience.Value = value.experience;
            Progress.Value = value.progress;
        }
    }

    public void ResetScore() {
        Score.Value = 0;
        Win.Value = false;
    }
    
    public void AddScore() {
        Score.Value++;
    }

    public void AffectScore() {
        if (Score.Value > BestScore.Value)
            BestScore.Value = Score.Value;
        
        var value = Experience.Value;
        value += Score.Value;

        while(value / 100f >= 1) {
            Level.Value++;
            value -= 100;
        }

        Experience.Value = value;
        Progress.Value = value / 100f;
    }
    
    public void WinGame() {
        Score.Value *= 2;
        Win.Value = true;
    }
}

public struct ProfileState {
    public int bestScore;
    public int experience;
    public int level;
    public float progress;
}
