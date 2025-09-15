using System.Text.Json.Serialization;

namespace UberApi.Direct.Models
{
    /// <summary>
    /// Response returned by Create Quote.
    /// </summary>
    public class CreateQuoteResponse
    {
        /// <summary>
        /// The kind of object; should be "delivery_quote".
        /// </summary>
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        /// <summary>
        /// Unique identifier for this quote. (always starts with dqt_)
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Date/Time timestamp (RFC 3339) at which the quote was created.
        /// </summary>
        [JsonPropertyName("created")]
        public string Created { get; set; }

        /// <summary>
        /// Date/Time timestamp (RFC 3339) after which the quote will no longer be accepted.
        /// </summary>
        [JsonPropertyName("expires")]
        public string Expires { get; set; }

        /// <summary>
        /// Fee for the delivery (in cents / minor currency units).
        /// </summary>
        [JsonPropertyName("fee")]
        public int Fee { get; set; }

        /// <summary>
        /// Currency code (lower-case, ISO 4217).
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Upper-case currency type (often same as currency, may differ in certain regions).
        /// </summary>
        [JsonPropertyName("currency_type")]
        public string CurrencyType { get; set; }

        /// <summary>
        /// Estimated time of arrival at drop-off (ISO 8601).
        /// </summary>
        [JsonPropertyName("dropoff_eta")]
        public string DropoffEta { get; set; }

        /// <summary>
        /// Estimated duration of the delivery, in minutes.
        /// </summary>
        [JsonPropertyName("duration")]
        public int? Duration { get; set; }

        /// <summary>
        /// Estimated time until pickup is completed, in minutes.
        /// </summary>
        [JsonPropertyName("pickup_duration")]
        public int? PickupDuration { get; set; }

        /// <summary>
        /// Drop-off deadline (ISO 8601), time by which the delivery must be completed.
        /// </summary>
        [JsonPropertyName("dropoff_deadline")]
        public string DropoffDeadline { get; set; }
    }
}
