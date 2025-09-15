using System.Net;
using System.Text;
using System.Text.Json;

using Uber.Direct;
using UberApi.Direct.Models;

namespace UberApi.Tests.Direct
{
    [TestClass]
    public class UberDirectClientTests
    {
        private const string BaseUrl = "https://api.uber.com/";   // trailing slash to verify TrimEnd('/')
        private const string CustomerId = "cust_123";
        private const string Token = "test_token";

        // ------------ Helpers ------------

        private static UberDirectClient MakeClient(RecordingHandler handler)
        {
            var http = new HttpClient(handler) { BaseAddress = null }; // client builds absolute URLs
            var client = new UberDirectClient(http, BaseUrl, CustomerId) { AccessToken = Token };
            return client;
        }

        private static HttpResponseMessage Json200(object body)
            => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(body, new JsonSerializerOptions { PropertyNamingPolicy = null }), Encoding.UTF8, "application/json")
            };

        // ========== Tests ==========

        [TestMethod]
        public async Task CreateQuote_SendsCorrectRequest_AndParsesResponse()
        {
            // Arrange
            var handler = new RecordingHandler(req =>
            {
                Assert.AreEqual(HttpMethod.Post, req.Method);
                Assert.AreEqual($"https://api.uber.com/v1/customers/{CustomerId}/delivery_quotes", req.RequestUri!.ToString());
                Assert.AreEqual("Bearer", req.Headers.Authorization!.Scheme);
                Assert.AreEqual(Token, req.Headers.Authorization!.Parameter);

                // Body must be snake_case per JsonOpts (PropertyNamingPolicy = null)
                var payload = req.Content!.ReadAsStringAsync().Result;
                Assert.IsTrue(payload.Contains("\"dropoff_address\""));
                Assert.IsTrue(payload.Contains("\"pickup_address\""));
                Assert.IsFalse(payload.Contains("\"PickupAddress\"")); // ensure not camel-cased

                var resp = new CreateQuoteResponse { Kind = "delivery_quote", Id = "q_1", Fee = 599, Currency = "usd" };
                return Json200(resp);
            });

            var client = MakeClient(handler);

            // Act
            var result = await client.CreateQuoteAsync(new CreateQuoteRequest
            {
                PickupAddress = "20 W 34th St, New York, NY 10001",
                DropoffAddress = "285 Fulton St, New York, NY 10006"
            });

            // Assert
            Assert.AreEqual("delivery_quote", result.Kind);
            Assert.AreEqual("q_1", result.Id);
            Assert.AreEqual(599, result.Fee);
            Assert.AreEqual("usd", result.Currency);
        }

        [TestMethod]
        public async Task CreateDelivery_SendsCorrectRequest_AndParsesResponse()
        {
            var handler = new RecordingHandler(req =>
            {
                Assert.AreEqual(HttpMethod.Post, req.Method);
                Assert.AreEqual($"https://api.uber.com/v1/customers/{CustomerId}/deliveries", req.RequestUri!.ToString());

                var body = req.Content!.ReadAsStringAsync().Result;

                Assert.IsTrue(body.Contains("\"quote_id\":\"q_123\""));
                Assert.IsTrue(body.Contains("\"pickup_address\""));
                Assert.IsTrue(body.Contains("\"pickup_name\":\"Store NYC\""));
                Assert.IsTrue(body.Contains("\"dropoff_name\":\"Ada Lovelace\""));

                var resp = new Delivery { Id = "del_1", QuoteId = "q_123", Status = "processing", TrackingUrl = "https://t" };
                return Json200(resp);
            });

            var client = MakeClient(handler);

            var result = await client.CreateDeliveryAsync(new CreateDeliveryRequest
            {
                QuoteId = "q_123",
                PickupAddress = "A",
                PickupName = "Store NYC",
                PickupPhoneNumber = "1111111111",
                DropoffAddress = "B",
                DropoffName = "Ada Lovelace",
                DropoffPhoneNumber = "2222222222",
                ManifestItems = new List<ManifestItem> { new ManifestItem { Name = "Box", Quantity = 1 } }
            });

            Assert.AreEqual("del_1", result.Id);
            Assert.AreEqual("q_123", result.QuoteId);
            Assert.AreEqual("processing", result.Status);
            Assert.AreEqual("https://t", result.TrackingUrl);
        }

        [TestMethod]
        public async Task ListDeliveries_BuildsQueryParams_AndParsesResponse()
        {
            var handler = new RecordingHandler(req =>
            {
                Assert.AreEqual(HttpMethod.Get, req.Method);
                // filter must be URL-encoded
                Assert.AreEqual($"https://api.uber.com/v1/customers/{CustomerId}/deliveries?filter=status%3Aactive&limit=25&offset=50", req.RequestUri!.ToString());

                var resp = new ListDeliveriesResponse
                {
                    Count = 2,
                    Data = new List<Delivery>
                {
                    new Delivery { Id = "d1", Status = "active" },
                    new Delivery { Id = "d2", Status = "active" }
                }
                };

                return Json200(resp);
            });

            var client = MakeClient(handler);

            var result = await client.ListDeliveriesAsync("status:active", 25, 50);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(new[] { "d1", "d2" }.First(), result.Data.Select(d => d.Id).First());
        }

        [TestMethod]
        public async Task UpdateDelivery_UsesCorrectPath_AndParsesResponse()
        {
            var handler = new RecordingHandler(req =>
            {
                Assert.AreEqual(HttpMethod.Post, req.Method);
                Assert.AreEqual($"https://api.uber.com/v1/customers/{CustomerId}/deliveries/del_abc", req.RequestUri!.ToString());

                var body = req.Content!.ReadAsStringAsync().Result;
                Assert.IsTrue(body.Contains("\"pickup_notes\":\"ring bell\""));

                var resp = new Delivery { Id = "del_abc", Status = "updated" };
                return Json200(resp);
            });

            var client = MakeClient(handler);

            var result = await client.UpdateDeliveryAsync("del_abc", new UpdateDeliveryRequest { PickupNotes = "ring bell" });
            Assert.AreEqual("del_abc", result.Id);
            Assert.AreEqual("updated", result.Status);
        }

        [TestMethod]
        public async Task GetDelivery_UsesCorrectPath_AndParsesResponse()
        {
            var handler = new RecordingHandler(req =>
            {
                Assert.AreEqual(HttpMethod.Get, req.Method);
                Assert.AreEqual($"https://api.uber.com/v1/customers/{CustomerId}/deliveries/del_42", req.RequestUri!.ToString());

                var resp = new Delivery { Id = "del_42", Status = "active" };
                return Json200(resp);
            });

            var client = MakeClient(handler);

            var result = await client.GetDeliveryAsync("del_42");
            Assert.AreEqual("del_42", result.Id);
            Assert.AreEqual("active", result.Status);
        }

        [TestMethod]
        public async Task CancelDelivery_UsesCorrectPath_AndParsesResponse()
        {
            var handler = new RecordingHandler(req =>
            {
                Assert.AreEqual(HttpMethod.Post, req.Method);
                Assert.AreEqual($"https://api.uber.com/v1/customers/{CustomerId}/deliveries/del_42/cancel", req.RequestUri!.ToString());

                var resp = new Delivery { Id = "del_42", Status = "canceled" };
                return Json200(resp);
            });

            var client = MakeClient(handler);

            var result = await client.CancelDeliveryAsync("del_42");
            Assert.AreEqual("del_42", result.Id);
            Assert.AreEqual("canceled", result.Status);
        }

        [TestMethod]
        public async Task ProofOfDelivery_ReturnsRawBase64String()
        {
            var base64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAAB"; // fake png
            var handler = new RecordingHandler(req =>
            {
                Assert.AreEqual(HttpMethod.Post, req.Method);
                Assert.AreEqual($"https://api.uber.com/v1/customers/{CustomerId}/deliveries/del_42/proof-of-delivery", req.RequestUri!.ToString());

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(base64, Encoding.UTF8, "application/json")
                };
            });

            var client = MakeClient(handler);

            var result = await client.GetProofOfDeliveryBase64Async("del_42");
            Assert.AreEqual(base64, result);
        }

        [TestMethod]
        public async Task AuthorizationHeader_IsPresent_OnEveryRequest()
        {
            var hit = 0;
            var handler = new RecordingHandler(req =>
            {
                hit++;
                Assert.AreEqual("Bearer", req.Headers.Authorization!.Scheme);
                Assert.AreEqual(Token, req.Headers.Authorization!.Parameter);
                return Json200(new { ok = true });
            });

            var client = MakeClient(handler);

            await client.CreateQuoteAsync(new CreateQuoteRequest { PickupAddress = "A", DropoffAddress = "B" });
            await client.ListDeliveriesAsync();
            await client.GetDeliveryAsync("x");

            Assert.IsTrue(hit >= 3);
        }
    }

    /// <summary>
    /// Minimal in-memory handler that lets us assert on requests and return canned responses.
    /// </summary>
    internal sealed class RecordingHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;

        public RecordingHandler(Func<HttpRequestMessage, HttpResponseMessage> responder)
            => _responder = responder ?? throw new ArgumentNullException(nameof(responder));

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(_responder(request));
    }

}