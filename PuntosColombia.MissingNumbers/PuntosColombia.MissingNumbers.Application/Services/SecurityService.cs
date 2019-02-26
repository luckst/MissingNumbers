
namespace PuntosColombia.MissingNumbers.Application.Services
{
    using PuntosColombia.MissingNumbers.Domain.Entities;
    using PuntosColombia.MissingNumbers.Infrastructure.Framework.Instrumentation.Exceptions;
    using PuntosColombia.MissingNumbers.Infrastructure.Framework.Instrumentation.Logging;
    using PuntosColombia.MissingNumbers.Infrastructure.Framework.RepositoryPattern;
    using System;

    public class SecurityService : ISecurityService
    {
        IRepository<User> userRepository;
        ILoggerService loggerService;

        public SecurityService(IRepository<User> userRepository, ILoggerService loggerService)
        {
            this.userRepository = userRepository;
            this.loggerService = loggerService;
        }

        public User AuthenticateUser(string username, string password)
        {
            try
            {
                var user = userRepository.Get(u => u.UserName == username && u.Password == u.Password);

                if (user is null)
                    throw new UserNotFoundException();

                return user;
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                loggerService.LogException(ex);
                throw;
            }
        }

        public int RegisterUser(User user)
        {
            try
            {
                var userOld = userRepository.Get(u => u.UserName == user.UserName);

                if (userOld != null)
                    throw new ExistUserException();

                userOld = userRepository.Get(u => u.Email == user.Email);

                if (userOld != null)
                    throw new ExistUserException();

                userRepository.Add(user);
                userRepository.Commit();

                return user.UserId;
            }
            catch (ExistUserException)
            {
                throw;
            }
            catch (Exception ex)
            {
                loggerService.LogException(ex);
                throw;
            }
        }
    }
}
