using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarleneCollectionXmlTool.Domain.Queries.CountTotalNumberOfUniqueItems;

public class CountTotalNumberOfUniqueItemsRequest : IRequest<Result<CountTotalNumberOfUniqueItemsResponse>>
{

}
