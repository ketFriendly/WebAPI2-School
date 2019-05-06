using It_Girls_Projekat.Infrastructure;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Providers;
using It_Girls_Projekat.Repositories;
using It_Girls_Projekat.Services;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using Unity;
using Unity.Lifetime;

using Unity.WebApi;

[assembly: OwinStartup(typeof(It_Girls_Projekat.Startup))]
namespace It_Girls_Projekat
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = SetupUnity();
            ConfigureOAuth(app, container);

            HttpConfiguration config = new HttpConfiguration();
            config.DependencyResolver = new UnityDependencyResolver(container);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
        public void ConfigureOAuth(IAppBuilder app, UnityContainer container)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider(container)
            };
            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
        private UnityContainer SetupUnity()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            // e.g. container.RegisterType<ITestService, TestService>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            
            container.RegisterType<DbContext, AuthContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>();

            container.RegisterType<IGenericRepository<ApplicationUser>, GenericRepository<ApplicationUser>>();
            container.RegisterType<IGenericRepository<Admin>, GenericRepository<Admin>>();
            container.RegisterType<IGenericRepository<TeachesSubjects>, GenericRepository<TeachesSubjects>>();
            
            container.RegisterType<IAuthRepository, AuthRepository>();
            container.RegisterType<IParentsRepository, ParentsRepository>();
            container.RegisterType<IStudentsRepository, StudentsRepository>();
            container.RegisterType<ITeachersRepository, TeachersRepository>();
            container.RegisterType<IMarksRepository, MarksRepository>();
            container.RegisterType<ISubjectsRepository, SubjectsRepository>();
            container.RegisterType<IClassesRepository, ClassesRepository>();

            container.RegisterType<IUsersService, UsersService>();
            container.RegisterType<IParentsService, ParentsService>();
            container.RegisterType<ITeachersService, TeachersService>();
            container.RegisterType<IStudentsService, StudentsService>();
            container.RegisterType<IMarksService, MarksService>();
            container.RegisterType<IClassesService, ClassesService>();
            container.RegisterType<ISubjectsService, SubjectsService>();
            return container;
        }

    }
}