
namespace PuntosColombia.MissingNumbers.Application.Services
{
    using PuntosColombia.MissingNumbers.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ISearchService
    {
        Search DoSearch(Search search);

        List<Search> GetSearches(int userId, DateTime? startDate, DateTime? endDate);

        void Delete(int id);
    }
}
