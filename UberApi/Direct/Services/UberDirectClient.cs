using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using UberApi.Direct.Models;

namespace Uber.Direct
{
    /// <summary>
    /// Minimal client for the Uber Direct REST API.
    /// Base URL: https://api.uber.com
    /// Auth: OAuth2 client_credentials with scope "eats.deliveries" (Bearer token).
    /// See: Uber Developers → Deliveries → Direct API and public Postman collection.
    /// </summary>
    public class UberDirectClient : IUberDirectClient
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;
        private readonly string _customerId;

        /// <summary>
        /// OAuth 2.0 access token (client_credentials; scope: eats.deliveries). Reuse until expiry.
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Create a new client.
        /// </summary>
        /// <param name="http">Configured HttpClient.</param>
        /// <param name="baseUrl">API origin, typically "https://api.uber.com".</param>
        /// <param name="customerId">Your Uber Direct customer/organization ID.</param>
        public UberDirectClient(HttpClient http, string baseUrl, string customerId)
        {
            _http = http;
            _baseUrl = baseUrl.TrimEnd('/');
            _customerId = customerId;
        }

        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNamingPolicy = null, // keep snake_case properties
            WriteIndented = false
        };

        private HttpRequestMessage Build(HttpMethod method, string path, object body = null)
        {
            var req = new HttpRequestMessage(method, $"{_baseUrl}{path}");
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);

            if (body != null)
            {
                var json = JsonSerializer.Serialize(body, JsonOpts);
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return req;
        }

        private async Task<T> SendAsync<T>(HttpRequestMessage req, CancellationToken ct)
        {
            using var res = await _http.SendAsync(req, ct).ConfigureAwait(false);
            res.EnsureSuccessStatusCode();
            if (typeof(T) == typeof(string))
            {
                var txt = await res.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                return (T)(object)txt;
            }
            var payload = await res.Content.ReadFromJsonAsync<T>(JsonOpts, ct).ConfigureAwait(false);
            return payload!;
        }

        /// <summary>
        /// Create Quote — POST /v1/customers/{customer_id}/delivery_quotes
        /// Creates a quote to check deliverability, validity, and estimated fee/ETA between two addresses.
        /// Request body: <see cref="CreateQuoteRequest"/>
        /// Response body: <see cref="CreateQuoteResponse"/>
        ///
        /// HTTP responses (per docs & Postman):
        /// • 200 OK — Quote created; response contains fee, currency, ETA, and expiry. :contentReference[oaicite:3]{index=3}
        /// • 400 Bad Request — Invalid parameters (including address/geocode issues); note:
        ///   `address_undeliverable_limited_couriers` returns 400 (changed from 422). :contentReference[oaicite:4]{index=4}
        /// • 401 Unauthorized — Missing/invalid Bearer token. :contentReference[oaicite:5]{index=5}
        /// • 403 Forbidden — Token lacks required scope/permissions.
        /// • 404 Not Found — Customer/org not found or path variables invalid.
        /// • 409 Conflict — Idempotency conflict (if using an idempotency key in future).
        /// • 422 Unprocessable Entity — Validation failed (format/semantic issues not covered by 400).
        /// • 429 Too Many Requests — Rate-limited.
        /// • 5xx Server errors — Uber service unavailable or transient failure.
        /// </summary>
        public Task<CreateQuoteResponse> CreateQuoteAsync(CreateQuoteRequest body, CancellationToken ct = default)
        {
            var req = Build(HttpMethod.Post, $"/v1/customers/{_customerId}/delivery_quotes", body);
            return SendAsync<CreateQuoteResponse>(req, ct);
        }

        /// <summary>
        /// Create Delivery — POST /v1/customers/{customer_id}/deliveries
        /// Creates a delivery using a prior quote (quote_id) and contact details for pickup/dropoff.
        /// Request body: <see cref="CreateDeliveryRequest"/>
        /// Response body: <see cref="Delivery"/>
        ///
        /// HTTP responses (per docs & Postman):
        /// • 200 OK — Delivery created; response includes IDs, tracking_url, stop details. :contentReference[oaicite:6]{index=6}
        /// • 400 Bad Request — Invalid parameters (incl. undeliverable addresses). Changelog:
        ///   `address_undeliverable_limited_couriers` returns 400. :contentReference[oaicite:7]{index=7}
        /// • 401 Unauthorized — Missing/invalid Bearer token. :contentReference[oaicite:8]{index=8}
        /// • 403 Forbidden — Token lacks permissions for this customer.
        /// • 404 Not Found — Customer or referenced objects not found (e.g., quote_id unknown/expired).
        /// • 409 Conflict — Idempotency conflict (if idempotency_key reused with different payload).
        /// • 422 Unprocessable Entity — Validation failed (e.g., malformed phone numbers/fields).
        /// • 429 Too Many Requests — Rate-limited.
        /// • 5xx Server errors — Uber service unavailable or transient failure.
        /// </summary>
        public Task<Delivery> CreateDeliveryAsync(CreateDeliveryRequest body, CancellationToken ct = default)
        {
            var req = Build(HttpMethod.Post, $"/v1/customers/{_customerId}/deliveries", body);
            return SendAsync<Delivery>(req, ct);
        }

        /// <summary>
        /// List Deliveries — GET /v1/customers/{customer_id}/deliveries
        /// Lists deliveries for the customer; supports filtering/paging (filter, limit, offset).
        /// Response body: <see cref="ListDeliveriesResponse"/>
        ///
        /// HTTP responses:
        /// • 200 OK — Page of deliveries returned.
        /// • 400 Bad Request — Invalid query params (filter/limit/offset).
        /// • 401 Unauthorized — Missing/invalid Bearer token. :contentReference[oaicite:9]{index=9}
        /// • 403 Forbidden — Token lacks permissions for this customer.
        /// • 429 Too Many Requests — Rate-limited.
        /// • 5xx Server errors — Uber service unavailable or transient failure.
        /// </summary>
        public Task<ListDeliveriesResponse> ListDeliveriesAsync(string filter = "", int? limit = null, int? offset = null, CancellationToken ct = default)
        {
            var qs = new List<string>();
            if (!string.IsNullOrWhiteSpace(filter)) qs.Add($"filter={Uri.EscapeDataString(filter)}");
            if (limit.HasValue) qs.Add($"limit={limit.Value}");
            if (offset.HasValue) qs.Add($"offset={offset.Value}");
            var q = qs.Count > 0 ? "?" + string.Join("&", qs) : string.Empty;

            var req = Build(HttpMethod.Get, $"/v1/customers/{_customerId}/deliveries{q}");
            return SendAsync<ListDeliveriesResponse>(req, ct);
        }

        /// <summary>
        /// Update Delivery — POST /v1/customers/{customer_id}/deliveries/{delivery_id}
        /// Updates mutable fields of an existing delivery (TIMING-SENSITIVE; see FAQ). Typical edits:
        /// notes, some time windows, coordinates, manifest reference/tip, etc.
        /// Request body: <see cref="UpdateDeliveryRequest"/>
        /// Response body: <see cref="Delivery"/>
        ///
        /// HTTP responses:
        /// • 200 OK — Delivery updated; response reflects latest state. :contentReference[oaicite:10]{index=10}
        /// • 400 Bad Request — Invalid/mutually-exclusive updates or lifecycle stage disallows update.
        /// • 401 Unauthorized — Missing/invalid Bearer token. :contentReference[oaicite:11]{index=11}
        /// • 403 Forbidden — Token lacks permissions for this customer.
        /// • 404 Not Found — Unknown delivery_id.
        /// • 409 Conflict — Update conflicts with current delivery state.
        /// • 422 Unprocessable Entity — Validation failed (field formats).
        /// • 429 Too Many Requests — Rate-limited.
        /// • 5xx Server errors — Uber service unavailable or transient failure.
        /// </summary>
        public Task<Delivery> UpdateDeliveryAsync(string deliveryId, UpdateDeliveryRequest body, CancellationToken ct = default)
        {
            var req = Build(HttpMethod.Post, $"/v1/customers/{_customerId}/deliveries/{deliveryId}", body);
            return SendAsync<Delivery>(req, ct);
        }

        /// <summary>
        /// Get Delivery — GET /v1/customers/{customer_id}/deliveries/{delivery_id}
        /// Fetches the full delivery object (status, ETAs, proof links, stops, manifest).
        /// Response body: <see cref="Delivery"/>
        ///
        /// HTTP responses:
        /// • 200 OK — Delivery found and returned. :contentReference[oaicite:12]{index=12}
        /// • 401 Unauthorized — Missing/invalid Bearer token. :contentReference[oaicite:13]{index=13}
        /// • 403 Forbidden — Token lacks permissions for this customer.
        /// • 404 Not Found — Unknown delivery_id.
        /// • 429 Too Many Requests — Rate-limited.
        /// • 5xx Server errors — Uber service unavailable or transient failure.
        /// </summary>
        public Task<Delivery> GetDeliveryAsync(string deliveryId, CancellationToken ct = default)
        {
            var req = Build(HttpMethod.Get, $"/v1/customers/{_customerId}/deliveries/{deliveryId}");
            return SendAsync<Delivery>(req, ct);
        }

        /// <summary>
        /// Cancel Delivery — POST /v1/customers/{customer_id}/deliveries/{delivery_id}/cancel
        /// Requests cancellation of the delivery (allowed only at certain lifecycle stages).
        /// Response body: <see cref="Delivery"/>
        ///
        /// HTTP responses:
        /// • 200 OK — Cancellation accepted; delivery transitions accordingly. :contentReference[oaicite:14]{index=14}
        /// • 400 Bad Request — Cancellation not allowed in current state or invalid params.
        /// • 401 Unauthorized — Missing/invalid Bearer token. :contentReference[oaicite:15]{index=15}
        /// • 403 Forbidden — Token lacks permissions for this customer.
        /// • 404 Not Found — Unknown delivery_id.
        /// • 409 Conflict — State conflict (e.g., already completed).
        /// • 422 Unprocessable Entity — Validation failed.
        /// • 429 Too Many Requests — Rate-limited.
        /// • 5xx Server errors — Uber service unavailable or transient failure.
        /// </summary>
        public Task<Delivery> CancelDeliveryAsync(string deliveryId, CancellationToken ct = default)
        {
            var req = Build(HttpMethod.Post, $"/v1/customers/{_customerId}/deliveries/{deliveryId}/cancel");
            return SendAsync<Delivery>(req, ct);
        }

        /// <summary>
        /// Proof of Delivery — POST /v1/customers/{customer_id}/deliveries/{delivery_id}/proof-of-delivery
        /// Retrieves a Base64-encoded PNG image for proof (e.g., photo/signature), when available.
        /// Request body: none
        /// Response body: string (Base64 PNG)
        ///
        /// HTTP responses:
        /// • 200 OK — Base64 PNG returned in response body. :contentReference[oaicite:16]{index=16}
        /// • 400 Bad Request — Proof unavailable for current state.
        /// • 401 Unauthorized — Missing/invalid Bearer token. :contentReference[oaicite:17]{index=17}
        /// • 403 Forbidden — Token lacks permissions for this customer.
        /// • 404 Not Found — Unknown delivery_id or proof not found.
        /// • 422 Unprocessable Entity — Validation failed.
        /// • 429 Too Many Requests — Rate-limited.
        /// • 5xx Server errors — Uber service unavailable or transient failure.
        /// </summary>
        public Task<string> GetProofOfDeliveryBase64Async(string deliveryId, CancellationToken ct = default)
        {
            var req = Build(HttpMethod.Post, $"/v1/customers/{_customerId}/deliveries/{deliveryId}/proof-of-delivery");
            return SendAsync<string>(req, ct);
        }
    }
}
