namespace Headway.Core.Interface
{
    public interface IServiceResult<T>
    {
        bool IsSuccess { get; set; }
        string Message { get; set; }
        T Result { get; set; }
    }
}
