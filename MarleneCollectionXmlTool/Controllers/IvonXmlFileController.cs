using MarleneCollectionXmlTool.Domain.Queries;
using MarleneCollectionXmlTool.Domain.Queries.CountTotalNumberOfUniqueItems;
using MarleneCollectionXmlTool.Domain.Queries.GetAllProductConfigurations;
using MarleneCollectionXmlTool.Domain.Queries.GetProductAttributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarleneCollectionXmlTool.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IvonXmlFileController : ControllerBase
{
    private readonly IMediator _mediator;

    public IvonXmlFileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetAllUniqueAttributesAndValuesFromXml")]
    public async Task<IActionResult> GetAllUniqueAttributesAndValuesFromXml(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetProductAttributesRequest(), cancellationToken);

        if (response.IsFailed)
            return BadRequest(response.Errors.First().Message);

        return Ok(response.Value);
    }

    [HttpGet("GetTotalNumberOfUniqueItems")]
    public async Task<IActionResult> GetTotalNumberOfUniqueItems(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CountTotalNumberOfUniqueItemsRequest(), cancellationToken);

        if (response.IsFailed)
            return BadRequest(response.Errors.First().Message);

        return Ok(response.Value);
    }

    [HttpGet("GetAllProductConfigurations")]
    public async Task<IActionResult> GetAllProductConfigurations(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAllProductConfigurationsRequest(), cancellationToken);;

        if (response.IsFailed)
            return BadRequest(response.Errors.First().Message);

        return Ok(response.Value);
    }
}
