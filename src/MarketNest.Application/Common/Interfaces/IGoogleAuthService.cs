namespace MarketNest.Application.Common.Interfaces;

public interface IGoogleAuthService
{
    Task<(string Email, string Subject, string GivenName, string FamilyName, bool EmailVerified)> ValidateIdTokenAsync(string idToken);
}
