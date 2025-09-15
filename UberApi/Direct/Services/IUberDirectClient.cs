using UberApi.Direct.Models;

namespace Uber.Direct
{
    public interface IUberDirectClient
    {
        Task<Delivery> CancelDeliveryAsync(string deliveryId, CancellationToken ct = default);
        
        Task<Delivery> CreateDeliveryAsync(CreateDeliveryRequest body, CancellationToken ct = default);
        
        Task<CreateQuoteResponse> CreateQuoteAsync(CreateQuoteRequest body, CancellationToken ct = default);
        
        Task<Delivery> GetDeliveryAsync(string deliveryId, CancellationToken ct = default);
        
        Task<string> GetProofOfDeliveryBase64Async(string deliveryId, CancellationToken ct = default);
        
        Task<ListDeliveriesResponse> ListDeliveriesAsync(string filter = "", int? limit = null, int? offset = null, CancellationToken ct = default);
        
        Task<Delivery> UpdateDeliveryAsync(string deliveryId, UpdateDeliveryRequest body, CancellationToken ct = default);
    }
}