using MarleneCollectionXmlTool.Domain.Queries.GetAllProductConfigurations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using MarleneCollectionXmlTool.Domain.Queries.SyncProductStocksWithWholesales;
using System.Linq;
using MarleneCollectionXmlTool.Domain.Queries.UpdateMissingMetaLookups;
using MarleneCollectionXmlTool.Domain.Queries.DownloadProductImages;

namespace MarleneCollectionXmlTool.Controllers;

public class MapProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MapProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("SyncProductStocksWithWholesaler")]
    public async Task<IActionResult> SyncProductStocksWithWholesaler(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new SyncProductStocksWithWholesalerRequest(), cancellationToken); ;

        if (response.IsFailed)
            return BadRequest(response.Errors.First().Message);

        return Ok(response.Value);
    }

    [HttpPost("UpdateMetaLookups")]
    public async Task<IActionResult> UpdateMetaLookups(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new UpdateMetaLookupsRequest(), cancellationToken); ;

        if (response.IsFailed)
            return BadRequest(response.Errors.First().Message);

        return Ok(response.Value);
    }

    [HttpPost("DownloadProductImages")]
    public async Task<IActionResult> DownloadProductImages(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DownloadProductImagesRequest(), cancellationToken); ;

        if (response.IsFailed)
            return BadRequest(response.Errors.First().Message);

        return Ok(response.Value);
    }
}
