using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ServerProjectTracker.AppLogic
{
    public class DockerApi
    {
        List<Models.Container> ContainersList = new();
        DockerClient client;

        public async Task<List<Models.Container>> GetListAsync()
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
    }
}
