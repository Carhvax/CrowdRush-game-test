public interface IIOService {
    void Save();
    void Load();
}

public class EasySaveIOService : IIOService {
    private const string Key = "PlayerProfile";
    
    private readonly ProfileModel _model;

    public EasySaveIOService(ProfileModel model) {
        _model = model;
    }

    public void Save() {
        var state = _model.State;
        
        ES3.Save(Key, state.Serialize());
    }
    
    public void Load() {
        if (ES3.KeyExists(Key)) {
            var data = ES3.Load<string>(Key);

            if (!data.IsNullOrEmpty()) {
                _model.State = data.Deserialize<ProfileState>();
            }
        }
    }
}
