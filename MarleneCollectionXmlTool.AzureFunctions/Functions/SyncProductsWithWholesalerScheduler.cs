using System;
using System.Threading.Tasks;
using MarleneCollectionXmlTool.Domain.Queries.SyncProductStocksWithWholesales;
using MediatR;
using Microsoft.Azure.Functions.Worker;

namespace MarleneCollectionXmlTool.AzureFunctions.Functions;

public class SyncProductsWithWholesalerScheduler
{
    private readonly IMediator _mediator;

    public SyncProductsWithWholesalerScheduler(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// "0 6,12,18 * * *" = At 06:00, 12:00 and 18:00
    /// </summary>
    [Function("SyncProductsWithHurtIvon")]
    public async Task SyncProductsWithHurtIvon([TimerTrigger("0 6,12,18 * * *")] SyncProductStocksWithWholesalerRequest request)
    {
        try
        {
            var result = await _mediator.Send(request);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
