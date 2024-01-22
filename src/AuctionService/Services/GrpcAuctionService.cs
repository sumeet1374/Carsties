using AuctionService.Data;
using Grpc.Core;

namespace AuctionService.Services;

public class GrpcAuctionService:GrpcAuction.GrpcAuctionBase
{
    private readonly AuctionDbContext _auctionDbContext;

    public GrpcAuctionService(AuctionDbContext auctionDbContext)
    {
        _auctionDbContext = auctionDbContext;
    }

    public override async Task<GrpcAuctionResponse> GetAuction(GetAuctionRequest request,ServerCallContext context)
    {
        Console.WriteLine("==> Received GRPC request for auction");
        var auction = await _auctionDbContext.Auctions.FindAsync(Guid.Parse(request.Id));
        if (auction == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound,"Not found"));
        }

        var response = new GrpcAuctionResponse()
        {
            Auction = new GrpcAuctionModel()
            {
                AuctionEnd = auction.AuctionEnd.ToString(),
                Id=auction.Id.ToString(),
                ReservedPrice = auction.ReservePrice,
                Seller = auction.Seller
                
            }            
        };

        return response;
    }
}