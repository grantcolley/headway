namespace Headway.Core.Interface
{
    public interface IAppCache
    {
        T Get<T>(string key);
        bool Remove(string key);
        void Set(string key, object value);
    }
}
