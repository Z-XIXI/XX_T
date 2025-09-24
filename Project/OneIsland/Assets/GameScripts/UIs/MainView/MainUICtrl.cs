public class MainUICtrl : SingletonClass<MainUICtrl>, IController
{
    private IModel _model;
    private IView _mainRoleView;
    private IView _mainOperaListView;
    public IModel Model => _model;
    public IView View => _mainRoleView;

    public MainUICtrl()
    {
        _model = MainUIModel.Instance;
        _mainRoleView = new MainRoleView();
        _mainOperaListView = new MainOperaListView();

        EventSystem.Instance.AddListener<GameDataValueType, float, float>(GlobalEvents.GameDtatChange, OnGameDataValueChange);
    }

    public void OnGameDataValueChange<T1, T2, T3>(T1 eventType, T2 newValue, T3 oldValue)
    {
        
    }
}
