namespace ColinhoDaCa.Domain._Shared.Entities;

public interface IRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Insere uma entidade assincronamente.
    /// </summary>
    /// <param name="entity">A entidade a ser inserida.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task InsertAsync(TEntity entity);

    /// <summary>
    /// Insere várias entidades assincronamente.
    /// </summary>
    /// <param name="entities">A coleção de entidades a serem inseridas.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task InsertAsync(IEnumerable<TEntity> entities);
}