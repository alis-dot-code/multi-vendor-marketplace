namespace MarketNest.Application.Common.Interfaces;

public interface IStripeService
{
    Task<string> CreateConnectedAccount(string email, string businessName);
    Task<string> CreateOnboardingLink(string accountId, string returnUrl);
    Task<bool> CheckOnboardingStatus(string accountId);
    Task<(string ClientSecret, string PaymentIntentId)> CreatePaymentIntent(
        int amountCents, 
        string currency, 
        string vendorStripeAccountId, 
        int platformFeeCents, 
        Dictionary<string, string> metadata);
    Task<string> ProcessRefund(string paymentIntentId, int? amountCents);
}
