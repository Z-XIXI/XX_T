// MVCºËÐÄ½Ó¿Ú
public interface IModel { }
public interface IView 
{
    void Open();
    bool IsOpen();
}

public interface IController 
{
    IModel Model { get; }
    IView View { get; }
}