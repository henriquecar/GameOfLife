namespace GameOfLife.Common.Mapper
{
    /// <summary>
    /// Defines a bidirectional mapper between two types.
    /// Provides a contract for converting objects in both directions.
    /// </summary>
    /// <typeparam name="T1">The source or internal representation type.</typeparam>
    /// <typeparam name="T2">The target or external/transport representation type.</typeparam>
    public interface IMapper<T1, T2>
    {
        /// <summary>
        /// Maps an object of type <typeparamref name="T1"/> to <typeparamref name="T2"/>.
        /// </summary>
        /// <param name="entity">The source object to map.</param>
        /// <returns>The mapped object of type <typeparamref name="T2"/>.</returns>
        T2 To(T1 entity);

        /// <summary>
        /// Maps an object of type <typeparamref name="T2"/> to <typeparamref name="T1"/>.
        /// </summary>
        /// <param name="entity">The source object to map.</param>
        /// <returns>The mapped object of type <typeparamref name="T1"/>.</returns>
        T1 From(T2 entity);
    }
}
