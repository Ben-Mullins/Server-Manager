﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using System.Runtime.InteropServices;

namespace ServerProjectTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DockerApiController : ControllerBase
    {
        List<Models.Container> ContainersList = new();
        DockerClient client;

        // GET: api/<DockerAPIController>
        [HttpGet]
        public async Task<List<Models.Container>> GetAsync()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                client = new DockerClientConfiguration(
                    new Uri("npipe://./pipe/docker_engine"))
                     .CreateClient();
            }
            else
            {
                client = new DockerClientConfiguration(
                    new Uri("unix:///var/run/docker.sock"))
                     .CreateClient();
            }

            IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
                new ContainersListParameters()
                {
                    Limit = 10,
                });

            foreach (ContainerListResponse container in containers)
            {
                Models.Container c = new()
                {
                    Id = container.ID,
                    Name = container.Names[0],
                    Created = container.Created,
                    Status = container.Status,
                    State = container.State
                };
                ContainersList.Add(c);
            }

            return ContainersList;
        }

        //// GET api/<DockerApiController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<DockerApiController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<DockerApiController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<DockerApiController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
