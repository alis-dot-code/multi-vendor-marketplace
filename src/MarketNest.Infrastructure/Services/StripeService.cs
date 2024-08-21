using Stripe;
using MarketNest.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MarketNest.Infrastructure.Services;

public class StripeService : IStripeService
{
    private readonly IConfiguration _config;

    public StripeService(IConfiguration config)
    {
        _config = config;
        StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
    }

    public async Task<string> CreateConnectedAccount(string email, string businessName)
    {
        var options = new AccountCreateOptions
        {
            Type = "express",
            Email = email,
            BusinessType = "individual",
            Capabilities = new AccountCapabilitiesOptions
            {
                CardPayments = new AccountCapabilitiesCardPaymentsOptions { Requested = true },
                Transfers = new AccountCapabilitiesTransfersOptions { Requested = true }
            },
            Metadata = new Dictionary<string, string>
            {
                { "business_name", businessName }
            }
        };

        var service = new AccountService();
        var account = await service.CreateAsync(options);
        return account.Id;
    }

    public async Task<string> CreateOnboardingLink(string accountId, string returnUrl)
    {
        var options = new AccountLinkCreateOptions
        {
            Account = accountId,
            RefreshUrl = returnUrl,
            ReturnUrl = returnUrl,
            Type = "account_onboarding"
        };

        var service = new AccountLinkService();
        var link = await service.CreateAsync(options);
        return link.Url;
    }

    public async Task<bool> CheckOnboardingStatus(string accountId)
    {
        var service = new AccountService();
        var account = await service.GetAsync(accountId);
        return account.ChargesEnabled && account.PayoutsEnabled;
    }

    public async Task<(string ClientSecret, string PaymentIntentId)> CreatePaymentIntent(
        int amountCents,
        string currency,
        string vendorStripeAccountId,
        int platformFeeCents,
        Dictionary<string, string> metadata)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = amountCents,
            Currency = currency,
            ApplicationFeeAmount = platformFeeCents,
            TransferData = new PaymentIntentTransferDataOptions
            {
                Destination = vendorStripeAccountId
            },
            Metadata = metadata
        };

        var service = new PaymentIntentService();
        var intent = await service.CreateAsync(options);
        return (intent.ClientSecret, intent.Id);
    }

    public async Task<string> ProcessRefund(string paymentIntentId, int? amountCents)
    {
        var options = new RefundCreateOptions
        {
            PaymentIntent = paymentIntentId,
            Amount = amountCents
        };

        var service = new RefundService();
        var refund = await service.CreateAsync(options);
        return refund.Id;
    }
}
