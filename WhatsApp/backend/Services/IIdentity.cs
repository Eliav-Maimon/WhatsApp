namespace backend.Services
{
    public interface IIdentity<T> where T : IEntity
    {
        // object GetToken(T entity, JwtSettings jwtConfig);
    }
}