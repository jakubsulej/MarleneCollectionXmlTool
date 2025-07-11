using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MarleneCollectionXmlTool.Domain.Commands.SyncProductStocksWithWholesales;
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
    /// "0 0 */6 * * *" every 6 hours run
    /// </summary>
    [Function("SyncProductsWithHurtIvon")]
    public async Task SyncProductsWithHurtIvon([TimerTrigger("0 0 */6 * * *")] SyncProductStocksWithWholesalerRequest request)
    {
        var sw = new Stopwatch();
        try
        {
            var result = await _mediator.Send(request);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(SyncProductsWithWholesalerScheduler)} returned an error message: {ex.Message}. Time taken: {sw.Elapsed}");
            throw new Exception(ex.Message);
        }
    }
}
