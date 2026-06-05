using BitirmeProjem.Domain.Entities;

namespace BitirmeProjem.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
