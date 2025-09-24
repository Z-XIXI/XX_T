public class WishStarCtrl : SingletonClass<WishStarCtrl>, IController
{
    private IModel _model;
    private IView _wishStarView;
    public IModel Model => _model;
    public IView View => _wishStarView;

    public WishStarCtrl()
    {
        _model = WishStarModel.Instance;
        _wishStarView = new WishStarView();
    }
    /// <summary>
    /// ��Ը�õ���Ʒ
    /// </summary>
    public void WishToGet(string itemName)
    {

    }
}