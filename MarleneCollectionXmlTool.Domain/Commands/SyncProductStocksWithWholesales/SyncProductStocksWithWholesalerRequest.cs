using FluentResults;
using MediatR;
namespace MarleneCollectionXmlTool.Domain.Commands.SyncProductStocksWithWholesales;

public class SyncProductStocksWithWholesalerRequest : IRequest<Result<SyncProductStocksWithWholesalerResponse>>
{

}
