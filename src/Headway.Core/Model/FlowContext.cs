namespace Headway.Core.Model
{
    public class FlowContext<T> : Flow
    {
        public T Context { get; set; }
    }
}
