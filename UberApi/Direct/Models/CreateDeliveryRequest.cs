using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Uber.Direct;

namespace UberApi.Direct.Models
{
    /// <summary>
    /// Request payload for Create Delivery endpoint.
    /// Based on Direct API (1.0.1)
    /// </summary>
    public class CreateDeliveryRequest
    {
        /// <summary>
        /// Designation of the location where the courier will make the pickup. This information will be visible within the courier app.
        /// Note: The app will prioritize the utilization of the pickup_business_name if this parameter is provided.
        /// </summary>
        [JsonPropertyName("pickup_name")]
        [Required]
        public string PickupName { get; set; }

        /// <summary>
        /// Designation of the location where the courier will make the pickup. This information will be visible within the courier app.
        /// Note: The app will prioritize the utilization of the pickup_business_name if this parameter is provided.
        /// https://developer.uber.com/docs/deliveries/faq#what-are-the-address-best-practices
        /// </summary>
        [JsonPropertyName("pickup_address")]
        [Required]
        public string PickupAddress { get; set; }

        /// <summary>
        /// Phone number for the pickup location, usually the store's contact. This number allows the courier to call before heading to the dropoff location.
        /// </summary>
        [JsonPropertyName("pickup_phone_number")]
        [Required]
        public string PickupPhoneNumber { get; set; }

        /// <summary>
        /// Name of the place where the courier will make the dropoff. This information will be visible in the courier app.
        /// </summary>
        [JsonPropertyName("dropoff_name")]
        [Required]
        public string DropoffName { get; set; }

        /// <summary>
        /// JSON string containing dropoff address details.
        /// Note: Please note the escaping of the quotes( \ " ). This is not the simplest format of expression addresses, however, the benefit comes with the corresponding parsing and accuracy when it is received by our booking engine. Please refer to our FAQ section for more information on how to format addresses.
        /// https://developer.uber.com/docs/deliveries/faq#what-are-the-address-best-practices
        /// </summary>
        [JsonPropertyName("dropoff_address")]
        [Required]
        public string DropoffAddress { get; set; }

        /// <summary>
        /// Phone number for the dropoff location, usually belonging to the end-user (recipient). This number enables the courier to make calls after en route to the dropoff and before completing the trip.
        /// </summary>
        [JsonPropertyName("dropoff_phone_number")]
        [Required]
        public string DropoffPhoneNumber { get; set; }

        /// <summary>
        /// List of items being delivered. This information will be visible in the courier app.
        /// </summary>
        [JsonPropertyName("manifest_items")]
        [Required]
        public List<ManifestItem> ManifestItems { get; set; }

        /// <summary>
        /// Business name of the pickup location. This information will be visible in the courier app and will override the pickup_name if provided.
        /// </summary>
        [JsonPropertyName("pickup_business_name")]
        public List<ManifestItem> PickupBusinessName { get; set; }

        /// <summary>
        /// Latitude of pickup location (optional; may be required in some regions).
        /// </summary>
        [JsonPropertyName("pickup_latitude")]
        public double? PickupLatitude { get; set; }

        /// <summary>
        /// Longitude of pickup location (optional; may be required in some regions).
        /// </summary>
        [JsonPropertyName("pickup_longitude")]
        public double? PickupLongitude { get; set; }

        /// <summary>
        /// Additional instructions for the courier at the pickup location. Max 280 characters. (e.g. access instructions).
        /// </summary>
        [JsonPropertyName("pickup_notes")]
        [MaxLength(280)]
        public string PickupNotes { get; set; }

        /// <summary>
        /// Verification steps (e.g. Picture, Barcode scanning) that must be taken before the pickup can be completed.
        /// </summary>
        [JsonPropertyName("pickup_verification")]
        public VerificationRequirements PickupVerification { get; set; }

        /// <summary>
        /// Business name of the dropoff location.
        /// </summary>
        [JsonPropertyName("dropoff_business_name")]
        public string DropoffBusinessName { get; set; }

        /// <summary>
        /// Dropoff latitude coordinate. This field adds precision to dropoff_address field. For example, if the dropoff address is "JFK Airport Queens, NY 11430", it would be highly recommended to use coordinates to locate the precise location of the dropoff (optional).
        /// </summary>
        [JsonPropertyName("dropoff_latitude")]
        public double? DropoffLatitude { get; set; }

        /// <summary>
        /// Dropoff longitude coordinate. This field adds precision to dropoff_address field. For example, if the dropoff address is "JFK Airport Queens, NY 11430", it would be highly recommended to use coordinates to locate the precise location of the dropoff. (optional).
        /// </summary>
        [JsonPropertyName("dropoff_longitude")]
        public double? DropoffLongitude { get; set; }

        /// <summary>
        /// Notes for drop-off location accessible after the courier accepts the trip and before heading to the dropoff location.
        /// </summary>
        [JsonPropertyName("dropoff_notes")]
        [MaxLength(280)]
        public string DropoffNotes { get; set; }

        /// <summary>
        /// Merchant's extra dropoff instructions, accessible after the courier accepts the trip and before heading to the dropoff location. Limited to 280 characters.
        /// </summary>
        [JsonPropertyName("dropoff_seller_notes")]
        [MaxLength(280)]
        public string DropoffSellerNotes { get; set; }

        /// <summary>
        /// Verification steps (e.g. Picture, Barcode scanning) that must be taken before the dropoff can be completed.
        /// </summary>
        [JsonPropertyName("dropoff_verification")]
        public DropoffVerification DropoffVerification { get; set; }

        /// <summary>
        /// Specify the action for the courier to take on a delivery.
        /// "deliverable_action_meet_at_door" = Meet at door delivery. This is the default if DeliverableAction is not set.
        /// "deliverable_action_leave_at_door" = The “happy path” action for the courier to take on a delivery.When used, delivery action can be set to “leave at door” for a contactless delivery.Cannot leave at door when signature or ID verification requirements are applied when creating a delivery.Photo confirmation of delivery will be automatically applied as a requirement to complete drop-off.
        /// </summary>
        [JsonPropertyName("deliverable_action")]
        public string DeliverableAction { get; set; }

        /// <summary>
        /// A reference identifying the manifest. Utilize this to link a delivery with relevant data in your system. This detail will be visible within the courier app.
        /// Note:
        /// Please be aware that the combination of this field with external_id must be unique; otherwise, the delivery creation will not succeed.
        /// If you can't ensure uniqueness for the manifest_reference, please include the "idempotency_key" in the request body and make sure it is unique.
        /// </summary>
        [JsonPropertyName("manifest_reference")]
        public string ManifestReference { get; set; }

        /// <summary>
        /// Value in cents ( 1/100 of currency unit ) of the items in the delivery. i.e.: $10.99 => 1099.
        /// </summary>
        [JsonPropertyName("manifest_total_value")]
        public int ManifestTotalValue { get; set; }

        /// <summary>
        /// The quote_id generated from CreateQuote. Ensures cost & deliverability.
        /// </summary>
        [JsonPropertyName("quote_id")]
        public string QuoteId { get; set; }

        /// <summary>
        /// Enum: "leave_at_door" "return" "discard"
        /// If not set then the default value is return
        /// leave_at_door: Once a normal delivery attempt is made and a customer is not available.This action requests the courier to leave the package at dropoff location.
        /// Note 1: It cannot be set to leave at door when signature or PIN or ID verification requirements are applied when creating a delivery.
        /// Note 2: A photo confirmation of delivery will be automatically applied as a requirement to complete the dropoff.
        /// return: Once a normal delivery attempt is made and a customer is not available. This action requests the courier to return the package back to the pickup location.
        /// Note: Even if deliverable_action was set as leave at door and courier feels it is not okay then the package can be returned back to the pickup location.
        /// discard: Discard option will allow the courier to keep/throw away the package if they are unable to deliver the package
        /// </summary>
        [JsonPropertyName("undeliverable_action")]
        public string UndeliverableAction { get; set; }

        /// <summary>
        /// (RFC 3339) Beginning of the window when an order must be picked up. Must be less than 30 days in the future.
        /// </summary>
        [JsonPropertyName("pickup_ready_dt")]
        public string PickupReadyDt { get; set; }

        /// <summary>
        /// (RFC 3339) End of the window when an order may be picked up. Must be at least 10 mins later than pickup_ready_dt and at least 20 minutes in the future from now.
        /// </summary>
        [JsonPropertyName("pickup_deadline_dt")]
        public string PickupDeadlineDt { get; set; }

        /// <summary>
        /// (RFC 3339) Beginning of the window when an order must be dropped off. Must be less than or equal to pickup_deadline_dt.
        /// </summary>
        [JsonPropertyName("dropoff_ready_dt")]
        public string DropoffReadyDt { get; set; }

        /// <summary>
        /// (RFC 3339) End of the window when an order must be dropped off. Must be at least 20 mins later than dropoff_ready_dt and must be greater than or equal to pickup_deadline_dt.
        /// </summary>
        [JsonPropertyName("dropoff_deadline_dt")]
        public string DropoffDeadlineDt { get; set; }

        /// <summary>
        /// Amount in cents ( 1/100 of currency unit ) that will be paid to the courier as a tip. e.g.: $5.00 => 500.
        /// Note: The fee value in the Create Delivery response includes the tip value.
        /// </summary>
        [JsonPropertyName("tip")]
        public int Tip { get; set; }

        /// <summary>
        /// A key which is used to avoid duplicate order creation with identical idempotency keys for the same account. The key persists for a set time frame, defaulting to 60 minutes.
        /// </summary>
        [JsonPropertyName("idempotency_key")]
        public int IdempotencyKey { get; set; }

        /// <summary>
        /// Unique identifier used by our Partners to reference a store or location.
        /// Note: Please be aware that if you utilize external_store_id in the Create Delivery process, you MUST also include this field in your Create Quote API calls.
        /// </summary>
        [JsonPropertyName("external_store_id")]
        public string ExternalStoreId { get; set; }

        /// <summary>
        /// Additional instructions for the courier for return trips. Max 280 characters.
        /// </summary>
        [JsonPropertyName("return_notes")]
        public string ReturnNotes { get; set; }

        /// <summary>
        /// Verification steps (barcode scanning, picture, or signature) that must be taken before the return can be completed.
        /// </summary>
        [JsonPropertyName("return_verification")]
        public VerificationRequirements ReturnVerification { get; set; }

        /// <summary>
        /// End-user's information to help identify users.
        /// </summary>
        [JsonPropertyName("external_user_info")]
        public object ExternalUserInfo { get; set; }

        /// <summary>
        /// Additional reference to identify the manifest. To be used by aggregators, POS systems, and other organization structures which have an internal reference in addition to the manifest_reference used by your sub-account. Merchants can search for this value in the dashboard, and it is also visible on the billing details report generated by the dashboard.
        /// </summary>
        [JsonPropertyName("external_id")]
        public string ExternalId { get; set; }

        /// <summary>
        /// A breakdown of how the order value is calculated
        /// </summary>
        [JsonPropertyName("user_fees_summary")]
        public IEnumerable<UserFeesSummary> UserFeesSummary { get; set; }
    }
}
