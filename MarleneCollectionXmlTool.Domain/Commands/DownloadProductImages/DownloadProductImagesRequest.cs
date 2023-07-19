using FluentResults;
using MediatR;

namespace MarleneCollectionXmlTool.Domain.Commands.DownloadProductImages;

public class DownloadProductImagesRequest : IRequest<Result<DownloadProductImagesResponse>>
{
}
