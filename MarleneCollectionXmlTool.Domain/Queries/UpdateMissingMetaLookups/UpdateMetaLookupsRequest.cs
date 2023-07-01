using FluentResults;
using MediatR;

namespace MarleneCollectionXmlTool.Domain.Queries.UpdateMissingMetaLookups;

public class UpdateMetaLookupsRequest : IRequest<Result<UpdateMetaLookupsResponse>> { }
