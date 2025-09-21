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
    }
}
