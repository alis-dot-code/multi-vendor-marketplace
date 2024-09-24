using Microsoft.AspNetCore.Mvc;
using Stripe;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Domain.Enums;

[ApiController]
[Route("api/v1/webhooks")]
public class WebhooksController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _config;

    public WebhooksController(IUnitOfWork uow, IConfiguration config)
    {
        _uow = uow;
        _config = config;
    }

    [HttpPost("stripe")]
    public async Task<IActionResult> Stripe()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var sig = Request.Headers["Stripe-Signature"].ToString();
        var secret = _config["StripeSettings:WebhookSecret"];
        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(json, sig, secret);
        }
        catch (Exception)
        {
            return BadRequest();
        }

        switch (stripeEvent.Type)
        {
            case "payment_intent.succeeded":
                var pi = stripeEvent.Data.Object as PaymentIntent;
                if (pi != null)
                {
                    var payment = await _uow.Payments.GetByStripePaymentIntentIdAsync(pi.Id);
                        if (payment != null)
                        {
                            payment.Status = PaymentStatus.Succeeded;
                            payment.StripeChargeId = null;
                            _uow.Payments.Update(payment);
                            await _uow.SaveChangesAsync();
                        }
                }
                break;
            case "charge.refunded":
                var ch = stripeEvent.Data.Object as Charge;
                if (ch != null && ch.PaymentIntentId != null)
                {
                    var payment = await _uow.Payments.GetByStripePaymentIntentIdAsync(ch.PaymentIntentId);
                    if (payment != null)
                    {
                        payment.Status = PaymentStatus.Refunded;
                        payment.RefundAmountCents = (int)ch.AmountRefunded;
                        _uow.Payments.Update(payment);
                        await _uow.SaveChangesAsync();
                    }
                }
                break;
            case "account.updated":
                var acct = stripeEvent.Data.Object as Account;
                if (acct != null)
                {
                    var vendor = (await _uow.Vendors.FindAsync(v => v.StripeAccountId == acct.Id)).FirstOrDefault();
                    if (vendor != null)
                    {
                        vendor.StripeOnboardingDone = acct.ChargesEnabled && acct.PayoutsEnabled;
                        _uow.Vendors.Update(vendor);
                        await _uow.SaveChangesAsync();
                    }
                }
                break;
        }

        return Ok();
    }
}
