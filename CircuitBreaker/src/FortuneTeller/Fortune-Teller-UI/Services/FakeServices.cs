using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fortune_Teller_UI.Services
{
    public interface IFakeService1
    {
        Task<List<Fortune>> RandomFortunes();
    }
    public interface IFakeService2
    {
        Task<List<Fortune>> RandomFortunes();
    }
    public interface IFakeServices3
    {
        Task<List<Fortune>> RandomFortunes();
    }
    public class FakeService1 : BaseFakeService, IFakeService1
    {
        IFakeService2 _service2;
        public FakeService1(IFortuneServiceCollapser fortuneService, IFakeService2 service2, ILogger<FakeService1> logger) : base(fortuneService, logger)
        {
            _service2 = service2;
        }

        public Task<List<Fortune>> RandomFortunes()
        {
            // Get a random fortune using FortuneServiceCollapser
            var myFortune = GetMyFortune();

            // Ask another service for some Fortunes
            var s2Fortunes = _service2.RandomFortunes();

            // Return a task that will combine the results into a List of Fortunes
            return CombineWith(myFortune, s2Fortunes);
        }
    }
    public class FakeService2 : BaseFakeService, IFakeService2
    {
        IFakeServices3 _service3;
        public FakeService2(IFortuneServiceCollapser fortuneService, IFakeServices3 service3, ILogger<FakeService2> logger) : base(fortuneService, logger)
        {
            _service3 = service3;
        }

        public Task<List<Fortune>> RandomFortunes()
        {

            // Get a random fortune using FortuneServiceCollapser
            var myFortune = GetMyFortune();

            // Ask another service for some Fortunes
            var s3Fortunes = _service3.RandomFortunes();

            // Return a task that will combine the results into a List of Fortunes
            return CombineWith(myFortune, s3Fortunes);

        }
    }
    public class FakeService3 : BaseFakeService, IFakeServices3
    {
     
        public FakeService3(IFortuneServiceCollapser fortuneService, ILogger<FakeService3> logger) : base(fortuneService, logger)
        {
        }

        public Task<List<Fortune>> RandomFortunes()
        {
            // Get a random fortune using FortuneServiceCollapser
            var myFortune = GetMyFortune();

            // Start with empty list of fortunes
            var result = Task.FromResult(new List<Fortune>());

            // Return a task that will combine the results into a List of Fortunes
            return CombineWith(myFortune, result);

        }
    }
    public abstract class BaseFakeService
    {
        protected IFortuneServiceCollapser _fortuneService;
        protected ILogger _logger;
        protected Random random = new Random();

        public BaseFakeService(IFortuneServiceCollapser fortuneService, ILogger logger)
        {
            this._fortuneService = fortuneService;
            this._logger = logger;
        }

        protected Task<Fortune> GetMyFortune()
        {
            // Fortune IDs are 1000-1049
            int id = random.Next(50) + 1000;
            return GetFortuneById(id);
        }

        protected Task<Fortune> GetFortuneById(int id)
        {
            // Use the FortuneServiceCollapser to obtain a Fortune

            // The collapser to configured (appsettings.json) to batch up 
            // multiple requests and then do a request every 250 milliseconds
            _logger.LogInformation("GetFortuneById({0})", id);
            _fortuneService.FortuneId = id;
            return _fortuneService.ExecuteAsync();
        }

        protected Task<List<Fortune>> CombineWith(Task<Fortune> t1, Task<List<Fortune>> t2)
        {
            Task<List<Fortune>> bothDone = Task.WhenAll(t1, t2)
              .ContinueWith<List<Fortune>>((t) =>
              {
                  // TODO: Should check for success
                  t2.Result.Add(t1.Result);
                  return t2.Result;
              });
            return bothDone;
        }

    }
}
