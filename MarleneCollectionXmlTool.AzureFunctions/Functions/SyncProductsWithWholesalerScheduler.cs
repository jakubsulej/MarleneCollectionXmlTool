using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MarleneCollectionXmlTool.Domain.Queries.SyncProductStocksWithWholesales;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MarleneCollectionXmlTool.AzureFunctions.Functions;

public class SyncProductsWithWholesalerScheduler
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public SyncProductsWithWholesalerScheduler(IMediator mediator, ILoggerFactory loggerFactory)
    {
        _mediator = mediator;
        _logger = loggerFactory.CreateLogger<SyncProductsWithWholesalerScheduler>();
    }

    /// <summary>
    /// "0 6,12,18 * * * *" = At 06:00, 12:00 and 18:00
    /// </summary>
    [Function("SyncProductsWithHurtIvon")]
    public async Task SyncProductsWithHurtIvon([TimerTrigger("0 6,12,18 * * * *")] SyncProductStocksWithWholesalerRequest request)
    {
        //_logger.LogInformation($"{nameof(SyncProductsWithWholesalerScheduler)} has started");
        var sw = new Stopwatch();

        try
        {
            var result = await _mediator.Send(request);
            //_logger.LogInformation($"{nameof(SyncProductsWithWholesalerScheduler)} has finished at {DateTime.Now} in {sw.Elapsed}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(SyncProductsWithWholesalerScheduler)} returned an error message: {ex.Message}. Time taken: {sw.Elapsed}");
            throw new Exception(ex.Message);
        }
    }
}
