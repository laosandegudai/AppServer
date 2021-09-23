namespace ASC.Common.Caching
{
    public interface ICacheWrapped<T>
    {
        public T WrapIn();
    }
}
