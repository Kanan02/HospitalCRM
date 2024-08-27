namespace Application.Interfaces.IServices.Security
{
    public interface IAccessValidator
    {
        void SetUserIdByRole<TEntity>(ref TEntity entity, string userIdPropName) where TEntity : class;
        void SetByProp<TEntity, TVal>(ref TEntity entity, TVal val, string propName) where TEntity : class;
        Guid GetCurrUserId();
    }
}
