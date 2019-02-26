
namespace PuntosColombia.MissingNumbers.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PuntosColombia.MissingNumbers.Domain.Entities;
    using PuntosColombia.MissingNumbers.Infrastructure.Framework.Instrumentation.Exceptions;
    using PuntosColombia.MissingNumbers.Infrastructure.Framework.Instrumentation.Logging;
    using PuntosColombia.MissingNumbers.Infrastructure.Framework.RepositoryPattern;

    public class SearchService : ISearchService
    {
        IRepository<Search> searchRepository;
        ILoggerService loggerService;

        public SearchService(IRepository<Search> searchRepository, ILoggerService loggerService)
        {
            this.searchRepository = searchRepository;
            this.loggerService = loggerService;
        }

        public void Delete(int id)
        {
            try
            {
                searchRepository.Delete(id);
                searchRepository.Commit();
            }
            catch (Exception ex)
            {
                loggerService.LogException(ex);
                throw;
            }
        }

        public Search DoSearch(Search search)
        {
            try
            {
                var searchOld = searchRepository.Get(s => s.QuantityListOne == search.QuantityListOne && s.QuantityListTwo == search.QuantityListTwo && s.ListOne == search.ListOne && s.ListTwo == search.ListTwo && s.UserId == search.UserId);

                if (searchOld != null)
                    throw new ExistSearchException();

                var n = search.QuantityListOne;
                var m = search.QuantityListTwo;
                string[] arr_temp = search.ListOne.Split(' ');
                var arr = Array.ConvertAll(arr_temp, Int32.Parse).ToList();
                string[] brr_temp = search.ListTwo.Split(' ');
                var brr = Array.ConvertAll(brr_temp, Int32.Parse).ToList();

                bool valido = true;

                if (n <= 0)
                    valido = false;

                if (m > (2 * Math.Pow(10, 5)))
                    valido = false;

                if (n > m)
                    valido = false;

                if ((brr.Any(x => x <= 0 || x > (Math.Pow(10, 4)))))
                    valido = false;

                if ((brr.Max() - brr.Min()) > 100)
                    valido = false;

                if (!valido)
                    throw new MissingNumbersEception();

                arr.Sort();
                brr.Sort();
                List<int> result = valido ? MissingNumbers(arr, brr) : new List<int>();

                search.Result = string.Join(' ', result);
                search.Date = DateTime.Now;

                searchRepository.Add(search);
                searchRepository.Commit();

                return search;
            }
            catch (ExistSearchException)
            {
                throw;
            }
            catch (MissingNumbersEception)
            {
                throw;
            }
            catch (Exception ex)
            {
                loggerService.LogException(ex);
                throw;
            }
        }

        public List<Search> GetSearches(int userId, DateTime? startDate, DateTime? endDate)
        {
            return searchRepository.Get(s => s.UserId == userId &&
            (!startDate.HasValue || s.Date >= startDate) &&
            (!endDate.HasValue || s.Date <= endDate), s => s.OrderByDescending(o => o.Date)).ToList();
        }

        private List<int> MissingNumbers(List<int> arr, List<int> brr)
        {
            List<int> res = new List<int>();

            foreach (var a in arr.Distinct())
            {
                if (res.Count == 0 || res.Count(x => x == a) == 0)
                {
                    var ca = arr.Count(x => x == a);
                    var cb = brr.Count(x => x == a);

                    if (ca != cb)
                    {
                        res.Add(a);
                    }
                }
            }

            return res;
        }
    }
}
