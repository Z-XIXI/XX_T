using System.Collections.Generic;

public class GameUIController
{
    public void Init()
    {
        InitAllControll();
    }
    public void Delete()
    {
        DeleteAllControll();
    }
    public void InitAllControll()
    {
        InitController(MainUICtrl.Instance);
    }
    public void DeleteAllControll()
    {
        //TODO
    }

    private List<IController> _controllers = new List<IController>();

    private void InitController(IController ctrl)
    {
        _controllers.Add(ctrl);
        ctrl.Initialize();
    }

    private static GameUIController instance;
    public static GameUIController Instance
    {
        get
        {
            if (null == instance)
                instance = new GameUIController();
            return instance;
        }
    }
}
