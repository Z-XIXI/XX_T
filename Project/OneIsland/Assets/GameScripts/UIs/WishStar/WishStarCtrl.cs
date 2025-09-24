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
    /// 许愿得到物品
    /// </summary>
    public void WishToGet(string itemName)
    {

    }
}