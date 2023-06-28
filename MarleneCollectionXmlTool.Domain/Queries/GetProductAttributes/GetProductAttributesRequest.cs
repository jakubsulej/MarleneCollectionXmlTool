using FluentResults;
using MediatR;

namespace MarleneCollectionXmlTool.Domain.Queries.GetProductAttributes;

public class GetProductAttributesRequest : IRequest<Result<GetProductAttributesResponse>>
{

}
