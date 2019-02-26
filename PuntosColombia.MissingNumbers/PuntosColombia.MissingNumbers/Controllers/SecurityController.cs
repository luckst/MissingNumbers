
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
    using PuntosColombia.MissingNumbers.Models.Security;

    [Route("api/[controller]")]
    public class SecurityController : Controller
    {
        ISecurityService securityService;
        IMapper mapper;

        public SecurityController(ISecurityService securityService, IMapper mapper)
        {
            this.securityService = securityService;
            this.mapper = mapper;
        }

        [HttpPost("[action]")]
        public ActionResult Authenticate([FromBody]User request)
        {
            var model = new UserViewModel();
            try
            {
                model = mapper.Map<UserViewModel>(securityService.AuthenticateUser(request.UserName, request.Password));
                model.MessageType = MessageTypeEnum.success;
            }
            catch (UserNotFoundException)
            {
                model.MessageType = MessageTypeEnum.warning;
                model.Message = "Usuario o contraseña inconrrectos.";
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

        [HttpPost("[action]")]
        public ActionResult Register([FromBody]User request)
        {
            var model = new UserViewModel();
            var id = 0;
            try
            {
                id = securityService.RegisterUser(request);
                model.MessageType = MessageTypeEnum.success;
            }
            catch (ExistUserException)
            {
                model.MessageType = MessageTypeEnum.warning;
                model.Message = "Nombre de usuario o correo ya existe.";
                model.ShowMessage = true;
            }
            catch (Exception)
            {
                model.MessageType = MessageTypeEnum.danger;
                model.Message = "Error en la aplicación";
                model.ShowMessage = true;

            }

            return Json(new { model, id });
        }
    }
}