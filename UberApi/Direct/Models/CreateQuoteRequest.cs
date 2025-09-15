using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text.Json.Serialization;
using Uber.Direct;

namespace UberApi.Direct.Models
{
    /// <summary>
    /// Create a quote to check deliverability, validity and cost for delivery between two addresses.
    /// Based on Direct API (1.0.1)
    /// </summary>
    public class CreateQuoteRequest
    {
        /// <summary>
        /// JSON string containing pickup address details.
        /// Note: Please note the escaping of the quotes( \ " ). This is not the simplest format of expression addresses, however, the benefit comes with the corresponding parsing and accuracy when it is received by our booking engine. Please refer to our FAQ section for more information on how to format addresses.
        /// https://developer.uber.com/docs/deliveries/faq#what-are-the-address-best-practices
        /// </summary>
        [JsonPropertyName("pickup_address")]
        [Required]
        public string PickupAddress { get; set; }

        /// <summary>
        /// JSON string containing dropoff address details.
        /// Note: Please note the escaping of the quotes( \ " ). This is not the simplest format of expression addresses, however, the benefit comes with the corresponding parsing and accuracy when it is received by our booking engine. Please refer to our FAQ section for more information on how to format addresses.
        /// https://developer.uber.com/docs/deliveries/faq#what-are-the-address-best-practices
        /// </summary>
        [JsonPropertyName("dropoff_address")]
        [Required]
        public string DropoffAddress { get; set; }

        /// <summary>
        /// Latitude coordinate of the pickup location. Required in some regions.
        /// </summary>
        [JsonPropertyName("pickup_latitude")]
        public double? PickupLatitude { get; set; }

        /// <summary>
        /// Longitude coordinate of the pickup location. Required in some regions.
        /// </summary>
        [JsonPropertyName("pickup_longitude")]
        public double? PickupLongitude { get; set; }

        /// <summary>
        /// Latitude coordinate of the drop-off location.
        /// </summary>
        [JsonPropertyName("dropoff_latitude")]
        public double? DropoffLatitude { get; set; }

        /// <summary>
        /// Longitude coordinate of the drop-off location.
        /// </summary>
        [JsonPropertyName("dropoff_longitude")]
        public double? DropoffLongitude { get; set; }

        /// <summary>
        /// ISO 8601 timestamp. (RFC 3339) Beginning of the window when an order must be picked up. Must be less than 30 days in the future.
        /// </summary>
        [JsonPropertyName("pickup_ready_dt")]
        public string PickupReadyDt { get; set; }

        /// <summary>
        /// ISO 8601 timestamp. (RFC 3339) End of the window when an order may be picked up. Must be at least 10 mins later than pickup_ready_dt and at least 20 minutes in the future from now.
        /// </summary>
        [JsonPropertyName("pickup_deadline_dt")]
        public string PickupDeadlineDt { get; set; }

        /// <summary>
        /// ISO 8601 timestamp. (RFC 3339) Beginning of the window when an order must be dropped off. Must be less than or equal to pickup_deadline_dt.
        /// </summary>
        [JsonPropertyName("dropoff_ready_dt")]
        public string DropoffReadyDt { get; set; }

        /// <summary>
        /// ISO 8601 timestamp. (RFC 3339) End of the window when an order must be dropped off. Must be at least 20 mins later than dropoff_ready_dt and must be greater than or equal to pickup_deadline_dt.
        /// </summary>
        [JsonPropertyName("dropoff_deadline_dt")]
        public string DropoffDeadlineDt { get; set; }

        /// <summary>
        /// Phone number for the pickup location, usually the store's contact. This number allows the courier to call before heading to the dropoff location.
        /// ^\+[0-9]+$
        /// </summary>
        [JsonPropertyName("pickup_phone_number")]
        public string PickupPhoneNumber { get; set; }

        /// <summary>
        /// Phone number for the dropoff location, usually belonging to the end-user (recipient). This number enables the courier to make calls after en route to the dropoff and before completing the trip.
        /// ^\+[0-9]+$
        /// </summary>
        [JsonPropertyName("dropoff_phone_number")]
        [Required]
        public string DropoffPhoneNumber { get; set; }

        /// <summary>
        /// Value in cents ( 1/100 of currency unit ) of the items in the delivery. i.e.: $10.99 => 1099.
        /// </summary>
        [JsonPropertyName("manifest_total_value")]
        public int? ManifestTotalValue { get; set; }

        /// <summary>
        /// Unique identifier used by our Partners to reference a store or location.
        /// Note: Please be aware that if you utilize external_store_id in the Create Delivery process, you MUST also include this field in your Create Quote API calls.
        /// </summary>
        [JsonPropertyName("external_store_id")]
        public string ExternalStoreId { get; set; }
    }
}
