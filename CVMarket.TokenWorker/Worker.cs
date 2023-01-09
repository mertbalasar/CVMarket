using CVMarket.DAL.Repository;
using CVMarket.Entities.Models;
using LinqKit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CVMarket.TokenWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMongoRepository<User> _userRepository;

        public Worker(ILogger<Worker> logger,
            IMongoRepository<User> userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;

            _logger.LogInformation("Worker initialized");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckDatabase();
                await Task.Delay(100, stoppingToken);
            }
        }

        private async Task CheckDatabase()
        {
            try
            {
                var filter = PredicateBuilder.New<User>(true);
                filter = filter.And(x => x.TokenExpireAt <= DateTime.UtcNow);

                var users = await _userRepository.FindManyAsync(filter);
                if (users.Code == 200)
                {
                    if (users.Result != null && users.Result.Count > 0)
                    {
                        users.Result.ForEach(async x =>
                        {
                            x.Token = null;
                            x.TokenExpireAt = null;
                            var updateResult = await _userRepository.UpdateOneAsync(x);
                            if (updateResult.Code != 200)
                            {
                                _logger.LogError("Can not update to User token");
                            }
                            else if (updateResult.Result != null)
                            {
                                _logger.LogInformation("Updated " + updateResult.Result.ModifiedCount + " user(s)");
                            }
                        });
                    }
                }
                else
                {
                    _logger.LogError("Can not access to User repository");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
