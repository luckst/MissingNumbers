
namespace PuntosColombia.MissingNumbers.Application.Services
{
    using PuntosColombia.MissingNumbers.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ISecurityService
    {
        User AuthenticateUser(string username, string password);

        int RegisterUser(User user);
    }
}
