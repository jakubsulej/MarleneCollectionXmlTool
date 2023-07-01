using FluentResults;
using MediatR;

namespace MarleneCollectionXmlTool.Domain.Queries.UploadProductImages;

public class UploadProductImagesRequest : IRequest<Result<UploadProductImagesResponse>>
{
}
