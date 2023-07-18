using FluentResults;
using MediatR;

namespace MarleneCollectionXmlTool.Domain.Queries.DownloadProductImages;

public class DownloadProductImagesRequest : IRequest<Result<DownloadProductImagesResponse>>
{
}
