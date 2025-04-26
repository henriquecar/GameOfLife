namespace GameOfLife.Common.Mapper
{
    public interface IMapper<T1, T2>
    {
        T2 To(T1 entity);
        T1 From(T2 entity);
    }
}