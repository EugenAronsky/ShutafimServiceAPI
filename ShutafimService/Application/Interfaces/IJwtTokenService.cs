﻿namespace ShutafimService.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(Guid userId, string phoneNumber);

    }
}
