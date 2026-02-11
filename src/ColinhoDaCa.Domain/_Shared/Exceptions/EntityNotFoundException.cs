using System.Net;

namespace ColinhoDaCa.Domain._Shared.Exceptions;

public class EntityNotFoundException : BaseException
{
    /// <summary>
    /// Inicia uma nova instância da classe <see cref="EntityNotFoundException"/>.
    /// </summary>
    public EntityNotFoundException()
    {
    }

    /// <summary>
    /// Inicia uma nova instância da classe <see cref="EntityNotFoundException"/>.
    /// </summary>
    public EntityNotFoundException(Type entityType)
        : this(entityType, null, null)
    {
    }

    /// <summary>
    /// Inicia uma nova instância da classe <see cref="EntityNotFoundException"/>.
    /// </summary>
    public EntityNotFoundException(Type entityType, object id)
        : this(entityType, id, null)
    {
    }

    /// <summary>
    /// Inicia uma nova instância da classe <see cref="EntityNotFoundException"/>.
    /// </summary>
    public EntityNotFoundException(Type entityType, object? id, Exception? innerException)
        : base(
            id == null
                ? $"There is no such an entity given id. Entity type: {entityType.FullName}"
                : $"There is no such an entity. Entity type: {entityType.FullName}, id: {id}",
            innerException)
    {
        EntityType = entityType;
        Id = id;
    }

    /// <summary>
    /// Inicia uma nova instância da classe <see cref="EntityNotFoundException"/>.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public EntityNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Inicia uma nova instância da classe <see cref="EntityNotFoundException"/>.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">Inner exception.</param>
    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Obtém ou define o tipo da entidade.
    /// </summary>
    public Type EntityType { get; set; } = null!;

    /// <summary>
    /// Obtém ou define o id da entidade.
    /// </summary>
    public object? Id { get; set; } = null!;

    public override int HttpStatusCode => 404;
}