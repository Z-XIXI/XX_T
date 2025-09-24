// MVC���Ľӿ�
public interface IModel { }
public interface IView 
{
    void Open();
    bool IsOpen();
    void Close();
}

public interface IController 
{
    IModel Model { get; }
    IView View { get; }
}