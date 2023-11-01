using FluentResults;
using MediatR;

namespace MarleneCollectionXmlTool.Domain.Queries.GetAllProductsWithoutSkus;

public class GetAllProductsWithoutSkusRequest : IRequest<Result<GetAllProductsWithoutSkusResponse>>
{
}
