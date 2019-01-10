﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using CastleAOPTest.SomeService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CastleAOPTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IServiceOne _serviceOne;
        public ValuesController(IServiceOne serviceOne, ILogger<ValuesController> log)
        {
            _serviceOne = serviceOne;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { _serviceOne.WriteLog(), _serviceOne.WriteLog2() };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetAsync(int id)
        {
            return await _serviceOne.WriteLog3();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
