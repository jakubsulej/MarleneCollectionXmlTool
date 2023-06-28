using FluentResults;
using MediatR;

namespace MarleneCollectionXmlTool.Domain.Queries.GetAllProductConfigurations;

public class GetAllProductConfigurationsRequest : IRequest<Result<GetAllProductConfigurationsResponse>>
{
}
