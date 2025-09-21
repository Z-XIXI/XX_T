public class WishStarModel : SingletonClass<WishStarModel>, IModel
{
    public WishStarModel()
    { 

    }
    public WishStarPanelType GetWishStarShowPanel()
    {
        return WishStarPanelType.Normal;
    }
}