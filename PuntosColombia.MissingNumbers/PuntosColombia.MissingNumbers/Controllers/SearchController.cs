
namespace PuntosColombia.MissingNumbers.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using PuntosColombia.MissingNumbers.Application.Services;
    using PuntosColombia.MissingNumbers.Domain.Entities;
    using PuntosColombia.MissingNumbers.Infrastructure.Framework.Instrumentation.Exceptions;
    using PuntosColombia.MissingNumbers.Models;
    using PuntosColombia.MissingNumbers.Models.Searchs;

    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        ISearchService searchService;
        IMapper mapper;

        public SearchController(ISearchService searchService, IMapper mapper)
        {
            this.searchService = searchService;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        public List<Search> GetSearches(int userId, string startDate, string endDate)
        {
            return searchService.GetSearches(userId, !string.IsNullOrEmpty(startDate)? (DateTime?)Convert.ToDateTime(startDate): null, !string.IsNullOrEmpty(endDate) ? (DateTime?)Convert.ToDateTime(endDate) : null);
        }

        [HttpPost("[action]")]
        public ActionResult DoSearch([FromBody]Search request)
        {
            var model = new SearchViewModel();
            try
            {
                model = mapper.Map<SearchViewModel>(searchService.DoSearch(request));
                model.MessageType = MessageTypeEnum.success;
            }
            catch (ExistSearchException)
            {
                model.MessageType = MessageTypeEnum.warning;
                model.Message = "La busqueda ingresada ya existe.";
                model.ShowMessage = true;
            }
            catch (MissingNumbersEception)
            {
                model.MessageType = MessageTypeEnum.warning;
                model.Message = "Error en la validaciones.";
                model.ShowMessage = true;
            }
            catch (Exception)
            {
                model.MessageType = MessageTypeEnum.danger;
                model.Message = "Error en la aplicación";
                model.ShowMessage = true;

            }

            return Json(model);
        }

        [HttpGet("[action]")]
        public ActionResult Delete(int id)
        {
            var model = new BaseViewModel();
            try
            {
               searchService.Delete(id);
                model.MessageType = MessageTypeEnum.success;
            }
            catch (Exception)
            {
                model.MessageType = MessageTypeEnum.danger;
                model.Message = "Error en la aplicación";
                model.ShowMessage = true;

            }

            return Json(model);
        }
    }
}