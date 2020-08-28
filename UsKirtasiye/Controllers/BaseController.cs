using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UsKirtasiye.Controllers
{
    public class BaseController : Controller
    {
        public IMapper _mapper;

        public BaseController()
        {
            ConfigureAutomapper();
        }

        private void ConfigureAutomapper()
        {
            if (_mapper == null)
            {
                var configuration = new MapperConfiguration(cfg =>
                {
                    /* cfg.CreateMap<Products, MembersDto>().ReverseMap();*/  // mepper member
                });
                // only during development, validate your mappings; remove it before release
                configuration.AssertConfigurationIsValid();
                // use DI (http://docs.automapper.org/en/latest/Dependency-injection.html) or create the mapper yourself
                _mapper = configuration.CreateMapper();
            }
        }
    }
}