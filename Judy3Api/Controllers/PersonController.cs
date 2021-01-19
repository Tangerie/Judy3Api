using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Judy.Modules;
using Judy.Models;

namespace Judy.Controllers
{
    [Route("People")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public List<Person> GetPeople()
        {
            return Storage.Instance.GetPeople();
        }

        [HttpGet("{id:int}")]
        public Person GetPerson(int id)
        {
            return Storage.Instance.GetPerson(id);
        }
    }
}
