// MVC���Ľӿ�
public interface IModel { }
public interface IView 
{
    void Open();
}

public interface IController 
{
    IModel Model { get; }
    IView View { get; }
    void Initialize();
}