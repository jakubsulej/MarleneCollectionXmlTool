using FluentResults;
using MediatR;

namespace MarleneCollectionXmlTool.Domain.Commands.UpdateMissingMetaLookups;

public class UpdateMetaLookupsRequest : IRequest<Result<UpdateMetaLookupsResponse>> { }
