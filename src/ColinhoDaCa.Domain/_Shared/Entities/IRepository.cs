using System.Linq.Expressions;

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

    /// <summary>
    /// Obtém todas as entidades assincronamente.
    /// </summary>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém a lista de entidades.</returns>
    Task<List<TEntity>> GetAllAsync();

    /// <summary>
    /// Obtém uma entidade com a chave especificada assincronamente.
    /// </summary>
    /// <param name="id">A chave da entidade.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém a entidade encontrada.</returns>
    Task<TEntity> GetAsync(long key);

    /// <summary>
    /// Obtém uma entidade que corresponda ao predicado especificado assincronamente.
    /// </summary>
    /// <param name="predicate">O predicado usado para corresponder à entidade.</param>
    /// <param name="includeDetails">Flag indicando se deve incluir detalhes relacionados ou não.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém a entidade encontrada.</returns>
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true);

    /// <summary>
    /// Procura uma entidade que corresponda ao predicado especificado assincronamente.
    /// </summary>
    /// <param name="predicate">O predicado usado para corresponder à entidade.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém a entidade encontrada ou null se não encontrada.</returns>
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Atualiza uma entidade.
    /// </summary>
    /// <param name="entity">A entidade a ser atualizada.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Atualiza várias entidades.
    /// </summary>
    /// <param name="entities">A coleção de entidades a serem atualizadas.</param>
    void Update(IEnumerable<TEntity> entities);

    /// <summary>
    /// Exclui uma entidade.
    /// </summary>
    /// <param name="entity">A entidade a ser excluída.</param>
    void Delete(TEntity entity);

    /// <summary>
    /// Exclui várias entidades.
    /// </summary>
    /// <param name="entities">A coleção de entidades a serem excluídas.</param>
    void Delete(IEnumerable<TEntity> entities);
}