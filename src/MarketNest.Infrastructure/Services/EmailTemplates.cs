using MarketNest.Domain.Entities;
using System.Text;

namespace MarketNest.Infrastructure.Services
{
    public static class EmailTemplates
    {
        public static string ReviewReceived(Review review)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<h1>New review received</h1>");
            sb.AppendLine($"<p>Rating: {review.Rating}</p>");
            sb.AppendLine($"<p>Comment: {review.Comment}</p>");
            sb.AppendLine("<p><small>This is an automated notification from MarketNest.</small></p>");
            return sb.ToString();
        }

        public static string DisputeOpened(Domain.Entities.Dispute dispute)
        {
            return $"<h1>Dispute opened</h1><p>Reason: {dispute.Reason}</p>";
        }

        public static string DisputeResolved(Domain.Entities.Dispute dispute)
        {
            return $"<h1>Dispute resolved</h1><p>Resolution: {dispute.Resolution}</p>";
        }
    }
}
