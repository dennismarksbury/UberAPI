using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Uber.Direct
{

    public class UserFeesSummary
    {
        /// <summary>
        /// Specifies the type of fee to be added or subtracted. Possible values include delivery fee, promo, loyalty points, etc
        /// </summary>
        [JsonPropertyName("fee_type")]
        public string FeeType { get; set; }

        /// <summary>
        /// Integer price in cents
        /// </summary>
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("user_fee_tax_info")]
        public UserFeesSummaryTaxInfo UserFeeTaxInfo { get; set; }
    }

    public class UserFeesSummaryTaxInfo
    {
        /// <summary>
        /// Integer tax added to the price to get a total
        /// </summary>
        [JsonPropertyName("tax_rate")]
        public int TaxRate { get; set; }
    }

    /// <summary>
    /// Representation of a delivery after it has been created, or when fetched.
    /// </summary>
    public class Delivery
    {
        /// <summary>
        /// Unique identifier for the delivery.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// The quote_id used to generate this delivery.
        /// </summary>
        [JsonPropertyName("quote_id")]
        public string QuoteId { get; set; }

        /// <summary>
        /// Current status of the delivery.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// Flag indicating whether the delivery is complete (true if delivered or returned / finalized).
        /// </summary>
        [JsonPropertyName("complete")]
        public bool? Complete { get; set; }

        /// <summary>
        /// Kind of object; typically "delivery".
        /// </summary>
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        /// <summary>
        /// Details about the pickup waypoint.
        /// </summary>
        [JsonPropertyName("pickup")]
        public StopInfo Pickup { get; set; }

        /// <summary>
        /// Details about the drop-off waypoint.
        /// </summary>
        [JsonPropertyName("dropoff")]
        public StopInfo Dropoff { get; set; }

        /// <summary>
        /// Manifest summary (description, total value) for this delivery.
        /// </summary>
        [JsonPropertyName("manifest")]
        public ManifestSummary Manifest { get; set; }

        /// <summary>
        /// Array of manifest items with detailed info (name, quantity, dimensions, etc.).
        /// </summary>
        [JsonPropertyName("manifest_items")]
        public List<ManifestItem> ManifestItems { get; set; }

        /// <summary>
        /// ISO 8601 timestamp when delivery was created.
        /// </summary>
        [JsonPropertyName("created")]
        public string Created { get; set; }

        /// <summary>
        /// ISO 8601 timestamp when delivery was last updated.
        /// </summary>
        [JsonPropertyName("updated")]
        public string Updated { get; set; }

        /// <summary>
        /// ISO 8601 timestamp when pickup window opens (ready).
        /// </summary>
        [JsonPropertyName("pickup_ready")]
        public string PickupReady { get; set; }

        /// <summary>
        /// ISO 8601 timestamp when pickup must happen by (deadline).
        /// </summary>
        [JsonPropertyName("pickup_deadline")]
        public string PickupDeadline { get; set; }

        /// <summary>
        /// ISO 8601 timestamp when drop-off window opens.
        /// </summary>
        [JsonPropertyName("dropoff_ready")]
        public string DropoffReady { get; set; }

        /// <summary>
        /// ISO 8601 timestamp when drop-off must happen by (deadline).
        /// </summary>
        [JsonPropertyName("dropoff_deadline")]
        public string DropoffDeadline { get; set; }

        /// <summary>
        /// Estimated time of arrival at pickup location (ISO 8601).
        /// </summary>
        [JsonPropertyName("pickup_eta")]
        public string PickupEta { get; set; }

        /// <summary>
        /// Estimated time of arrival at drop-off location (ISO 8601).
        /// </summary>
        [JsonPropertyName("dropoff_eta")]
        public string DropoffEta { get; set; }

        /// <summary>
        /// Fee charged for this delivery (in cents / minor currency units).
        /// </summary>
        [JsonPropertyName("fee")]
        public int? Fee { get; set; }

        /// <summary>
        /// Currency code for the fee (lower-case ISO 4217).
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// URL for tracking the delivery status.
        /// </summary>
        [JsonPropertyName("tracking_url")]
        public string TrackingUrl { get; set; }

        /// <summary>
        /// Indicates if courier is imminently arriving (within ~1 minute) either for pickup or dropoff.
        /// </summary>
        [JsonPropertyName("courier_imminent")]
        public bool? CourierImminent { get; set; }

        /// <summary>
        /// Whether the delivery is in live mode (true) or test/sandbox mode.
        /// </summary>
        [JsonPropertyName("live_mode")]
        public bool? LiveMode { get; set; }

        /// <summary>
        /// If delivery was undeliverable, the action taken.
        /// </summary>
        [JsonPropertyName("undeliverable_action")]
        public string UndeliverableAction { get; set; }

        /// <summary>
        /// If delivery was undeliverable, the reason why.
        /// </summary>
        [JsonPropertyName("undeliverable_reason")]
        public string UndeliverableReason { get; set; }

        /// <summary>
        /// Alternate unique identifier (“uuid”) without contextual prefix; case-insensitive, UUID v4 without dashes.
        /// </summary>
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }
    }

    /// <summary>
    /// Items included in the delivery manifest.
    /// </summary>
    public class ManifestItem
    {
        /// <summary>
        /// Name or description of the item.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Quantity of this item.
        /// </summary>
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Approximate size category of the item: “small”, “medium”, “large”, or “xlarge”. Defaults to “small” if unspecified.
        /// </summary>
        [JsonPropertyName("size")]
        public string Size { get; set; }

        /// <summary>
        /// Weight of the item in grams. Required when dimensions are provided.
        /// </summary>
        [JsonPropertyName("weight")]
        public double? Weight { get; set; }

        /// <summary>
        /// Price of the item in cents / minor currency units.
        /// </summary>
        [JsonPropertyName("price")]
        public int? Price { get; set; }

        /// <summary>
        /// Physical dimensions of the item (length, height, depth) (centimeters).
        /// </summary>
        [JsonPropertyName("dimensions")]
        public Dimensions Dimensions { get; set; }

        /// <summary>
        /// If the item must be kept upright during delivery (boolean).
        /// </summary>
        [JsonPropertyName("must_be_upright")]
        public bool? MustBeUpright { get; set; }
    }

    /// <summary>
    /// Summary of manifest: description and total monetary value.
    /// </summary>
    public class ManifestSummary
    {
        /// <summary>
        /// A (possibly multi-line) description of what the courier is delivering. [Deprecated in some contexts; prefer manifest_items for item-level description.] 
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Total value of the items in the delivery, in cents / minor currency units.
        /// </summary>
        [JsonPropertyName("total_value")]
        public int? TotalValue { get; set; }
    }

    /// <summary>
    /// Waypoint info (pickup or drop-off) for a delivery.
    /// </summary>
    public class StopInfo
    {
        /// <summary>
        /// Name of the person or place at the waypoint.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Phone number at the waypoint.
        /// </summary>
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Single line or user-friendly address string.
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; }

        /// <summary>
        /// Structured address fields (street lines, city, state, zip, country).
        /// </summary>
        [JsonPropertyName("detailed_address")]
        public DetailedAddress DetailedAddress { get; set; }

        /// <summary>
        /// Latitude / Longitude location of the waypoint.
        /// </summary>
        [JsonPropertyName("location")]
        public LatLng Location { get; set; }

        /// <summary>
        /// Additional instructions or notes for the waypoint.
        /// </summary>
        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        /// <summary>
        /// Requirements for verification at the waypoint (e.g. whether signature, picture, barcode, etc. are required).
        /// </summary>
        [JsonPropertyName("verification_requirements")]
        public VerificationRequirements VerificationRequirements { get; set; }

        /// <summary>
        /// Proof of verification once completed (e.g. signature image, barcode scan result), if available.
        /// </summary>
        [JsonPropertyName("verification")]
        public VerificationProof Verification { get; set; }
    }

    /// <summary>
    /// Structured address breakdown.
    /// </summary>
    public class DetailedAddress
    {
        /// <summary>
        /// First line of street address for the waypoint.
        /// </summary>
        [JsonPropertyName("street_address_1")]
        public string StreetAddress1 { get; set; }

        /// <summary>
        /// Optional second street address line.
        /// </summary>
        [JsonPropertyName("street_address_2")]
        public string StreetAddress2 { get; set; }

        /// <summary>
        /// City name of the waypoint.
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        /// State or region code of the waypoint.
        /// </summary>
        [JsonPropertyName("state")]
        public string State { get; set; }

        /// <summary>
        /// Postal (zip) code of the waypoint.
        /// </summary>
        [JsonPropertyName("zip_code")]
        public string ZipCode { get; set; }

        /// <summary>
        /// Country code (ISO 2-letter).
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }
    }

    /// <summary>
    /// Latitude / Longitude pair.
    /// </summary>
    public class LatLng
    {
        /// <summary>
        /// Latitude coordinate.
        /// </summary>
        [JsonPropertyName("lat")]
        public double? Lat { get; set; }

        /// <summary>
        /// Longitude coordinate.
        /// </summary>
        [JsonPropertyName("lng")]
        public double? Lng { get; set; }
    }

    /// <summary>
    /// Verification settings required at drop-off (or return) in Create Delivery.
    /// </summary>
    public class DropoffVerification
    {
        /// <summary>
        /// When set to true, mandates the Courier to capture an image as proof of delivery.
        /// </summary>
        [JsonPropertyName("picture")]
        public bool? Picture { get; set; }

        /// <summary>
        /// Signature requirement settings.
        /// </summary>
        [JsonPropertyName("signature_requirement")]
        public SignatureRequirement SignatureRequirement { get; set; }

        /// <summary>
        /// Whether barcodes are required or used in verification.
        /// </summary>
        [JsonPropertyName("barcodes")]
        public List<BarcodeSpec> Barcodes { get; set; }

        /// <summary>
        /// Identification verification spec (e.g. minimum age, etc.).
        /// </summary>
        [JsonPropertyName("identification")]
        public IdentificationSpec Identification { get; set; }
    }

    /// <summary>
    /// Settings related to signature verification.
    /// </summary>
    public class SignatureRequirement
    {
        /// <summary>
        /// Whether a signature is required.
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Whether the signer’s name must be collected.
        /// </summary>
        [JsonPropertyName("collect_signer_name")]
        public bool? CollectSignerName { get; set; }

        /// <summary>
        /// Whether signer’s relationship to recipient must be collected.
        /// </summary>
        [JsonPropertyName("collect_signer_relationship")]
        public bool? CollectSignerRelationship { get; set; }
    }

    /// <summary>
    /// Specification for barcodes for verification.
    /// </summary>
    public class BarcodeSpec
    {
        /// <summary>
        /// The string value encoded in the barcode.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }

        /// <summary>
        /// Type format of the barcode. Valid examples: CODE39, CODE39_FULL_ASCII, CODE128, QR.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    /// <summary>
    /// Identification spec (e.g. minimum age verification).
    /// </summary>
    public class IdentificationSpec
    {
        /// <summary>
        /// Minimum age verified for identification.
        /// </summary>
        [JsonPropertyName("min_age")]
        public int? MinAge { get; set; }
    }

    public sealed class Dimensions
    {
        [JsonPropertyName("length")]
        public double? Length { get; set; }

        [JsonPropertyName("height")]
        public double? Height { get; set; }

        [JsonPropertyName("depth")]
        public double? Depth { get; set; }
    }

    /// <summary>
    /// Verification steps (barcode scanning, picture, or signature) that must be taken.
    /// </summary>
    public sealed class VerificationRequirements
    {
        /// <summary>
        /// Barcode values/types that must be scanned at the waypoint. Number of elements in the array is equal to the number of barcodes that must be scanned.
        /// </summary>
        [JsonPropertyName("barcodes")] 
        public BarcodeSpec Barcodes { get; set; }

        /// <summary>
        /// Flag to indicate whether a photo is mandatory at this waypoint.
        /// </summary>
        [JsonPropertyName("picture")] 
        public bool? Picture { get; set; }

        /// <summary>
        /// Signature requirement spec to indicate that a signature must be collected at this waypoint.
        /// </summary>
        [JsonPropertyName("signature_requirement")] 
        public SignatureRequirement SignatureRequirement { get; set; }

        /// <summary>
        /// Identification scanning/verification requirements for this waypoint.
        /// </summary>
        [JsonPropertyName("identification")]
        public IdentificationSpec Identification { get; set; }
    }

    public sealed class VerificationProof
    {
        [JsonPropertyName("picture")] public ProofImage Picture { get; set; }
        [JsonPropertyName("barcodes")] public BarcodeProof Barcodes { get; set; }
        [JsonPropertyName("signature_proof")] public SignatureProof SignatureProof { get; set; }
    }

    public sealed class ProofImage { 
        [JsonPropertyName("image_url")] 
        public string ImageUrl { get; set; } 
    }

    public sealed class BarcodeProof
    {
        [JsonPropertyName("value")] public string Value { get; set; }
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("scan_result")] public BarcodeScanResult ScanResult { get; set; }
    }

    public sealed class BarcodeScanResult
    {
        [JsonPropertyName("outcome")] public string Outcome { get; set; }
        [JsonPropertyName("timestamp")] public string Timestamp { get; set; }
    }

    public sealed class SignatureProof
    {
        [JsonPropertyName("image_url")] public string ImageUrl { get; set; }
        [JsonPropertyName("signer_name")] public string SignerName { get; set; }
    }

    // ---- List/Get/Cancel/Delivery model ----
    public sealed class ListDeliveriesResponse
    {
        [JsonPropertyName("count")] 
        public int Count { get; set; }
        
        [JsonPropertyName("data")] 
        public List<Delivery> Data { get; set; }
    }

    public class UpdateDeliveryRequest
    {
        [JsonPropertyName("pickup_notes")]
        [MaxLength(280)]
        public string PickupNotes { get; set; }
    }
}
