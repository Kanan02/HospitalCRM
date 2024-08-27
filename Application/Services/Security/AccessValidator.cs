using Application.Constants;
using Application.Helpers;
using Application.Interfaces.IServices.Security;

namespace Application.Services.Security
{
    public class AccessValidator : IAccessValidator
    {
        private ISecurityService _securityService { get; }

        public AccessValidator(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public void SetUserIdByRole<TEntity>(ref TEntity entity, string userIdPropName) where TEntity : class =>
            SetByProp(ref entity, _securityService.GetCurrUserId(), userIdPropName);

        public void SetByProp<TEntity, TVal>(ref TEntity entity, TVal val, string propName) where TEntity : class
        {

            var roles = _securityService.GetCurrRoles();

            if (roles.Any(r => r == RoleConstant.Admin))
                return;

            if (entity == null)
                entity = Activator.CreateInstance<TEntity>();

            ReflectionHelper.SetPropValue(entity, val, propName);

        }


        public bool IsAdmin() => _securityService.IsHaveRole(RoleConstant.Admin);
        public Guid GetCurrUserId() => _securityService.GetCurrUserId();

    }
}
