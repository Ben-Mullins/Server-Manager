
# Server Manager
### Weber State CS 4450

#### <u>*The Purpose*</u>
The purpose of the Server Manager project is to provide Weber State students a simple way to host their web applications on a publicly accessible server. Weber state has set up the subdomain [csprojects.weber.edu](http://csprojects.weber.edu) to accomplish this purpose, and allocated us a Linux VM to do so.

#### <u>*Server Tracking Web Application General Information*</u>
The Server Project Tracker Application is a .NET Core 5.0 ASP.net Razor application and uses Entity Framework Core (the standard for .NET). For using the database, the connection string is located in the appsettings.json file located in ServerProjectTracker/SeverProjectTracker. However to manipulate the db it is easiest to ssh into the server and use the command: `/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'EXAMPLE PASSWORD'`  the password for the db is located in PLACEHOLDER on the server. The actual connection string should look like: `"Data Source=127.0.0.1,1433;Initial Catalog=Server_Tracker;User ID=SA;Password=EXAMPLE!"`,  this connection string can only be used while on the server itself, or you must be behind the Weber State Firewall, and must port tunnel using ssh to bind your local port 1433 to the server's port 1433.

If you need to update the web app on the server with the latest git push, the source files are located on the server at `/opt/Tracker/Server-Manager/` and all files lower than that directory. All published files are located in `/opt/Tracker/Server-Tracker/`. For security reasons, please do not push the actual db password to the github repository, you may need to stash changes or rewrite the appsettings.json connection string. The basic commands for updating the files will go as follows:

1. `cd /opt/Tracker/Server-manager`
2. `git pull origin master`
3. `cd /ServerProjectTracker`
4. `sudo dotnet publish -c Release -o /opt/Tracker/Server-Tracker`

You should always publish to the Server-Tracker folder, as those files are looked at by the associated service for the web application. This service is located at `/etc/systemd/system` and is called `tracker-app.service`. Once you update the files make sure to restart the service by using the command: `sudo systemctl restart tracker-app.service`

<b>Our application for now uses a simple unsecure login system, so please do not use a common password. We intended to eventually have this app use Weber State's CAS login system, but were unable to implement it.</b>

#### <u>*Nginx Information*</u>

This server makes use of nginx to act as the front facing server for all requests outside of the server, nginx. This is what allows us to host multiple projects. Nginx uses a config file to control most things, for the most part, the only file you really need to manipulate is located at `/etc/nginx/sites-enabled/default`, inside this file (which you must open as root if you want to edit) is the main server block, and all projects are listed as their own location blocks. An example location block looks like the following:

	
```
location /[appname]/ {
	proxy_pass http://127.0.0.1:[port]/;
	proxy_set_header Accept-Encoding "";
	sub_filter "href=\"/" "href=\"/[appname]/" 
	sub_filter_types *;
	sub_filter_once off;
}
```

Let's break down the location block, the [appname] would be replaced with what you want to use in the url to reach the project. Replace [port] with whatever port the project is running on the server with. As an overview, the location block tells nginx how to handle a request, and as a url would look like: `http://csprojects.weber.edu/[appname]/`, it is important that you format the top of the location block like `/[appname]/`, the first '/' is required, and the one appended to the end is to help with potential formatting issues. 

The <b>proxy_pass</b> is the root of how projects are able to be reached through nginx. The proxy_pass tells nginx to redirect all requests from this location to the given url. In the case of our server, all projects would be at the localhost ip address of 127.0.0.1, it is important that you use the IP address and not localhost as nginx doesn't work well with localhost. <b>It is crucial that you include the '/' after the port number in the url, without it nginx will not automatically strip out the location [appname] when it sends a request to the server, which will break the return of the web application.</b>

The `proxy_set_header Accept-Encoding "";` is important to add in case the project server returns something other than plain text, such as anything compressed, which is important for the item we will address next, the sub_filter.

The <b>sub_fitler</b> is relatively simple, what nginx does with a sub_fitler is search for anywhere in anything returned from the server (the HTML) and in this case looks for all href's, and appends `/appname/` before the rest of the link. <b>This is crucial to not break the webpage css and any links.</b> Why? All nginx does is act as a middleman between the client and the actual project server, the server doesn't know that it's being requested at `http://csprojects.weber.edu/[appname]`, and has no way to know that it needs to append the /[appname]/ in requests, you could manually configure your application to do so, but sub_filter makes that unnecessary. If necessary, you can add more sub_filter entries to take into account any other types of links like src or content. 

<b>There are a few carveouts with this solution, although unlikely, this solution probably doesn't work with any absolute links. Though we have not gotten to test it. We have not tested it, so we don't know if this will modify javascript files so this may not work with any javascript based front end web applications.</b>

The last 2 things in the location block to note is the sub_filter_types, which is simply to make it apply to all requested files, and the sub_filter_once needs to be off, to ensure that it applies this filter on every request.

### <u>To Do List</u>
Tier 1
<ul>
<li>Ability to pass professor a docker image and have the professor upload it to the Linux server ✓</li>
<ul><li>Professor will be responsible for setting up Nginx reverse proxy and forwarding correct port from host to container</li></ul>
<li>Be able to host multiple docker container projects on the same server ✓</li>
<li>All shared databases installed on Linux server (Currently MSSQL only) ✓</li>
<li>Role based access for different project databases ✓</li>
<li>FERPA compliancy ✓</li>
</ul>

Tier 2
<ul>
<li>Automate docker containerization of projects (.NET, React, Java based?/other common web frameworks used by Weber State students/courses)</li>
<li>Docker API ✓</li>
<li>Automate project creation/deployment through API</li>
<li>Automate project deletion through API</li>
<li>Manage resources used by docker containers (limit usage)</li>
<li>Manage resources used by databases</li>
<li>Single DB account per project</li>
<li>Automate docker port forwarding</li>
<li>Automate Nginx config for projects</li>
<li>Automate roles/account assignment for DBs</li>
<li>Automate project contactless project updating</li>
<ul><li>Potentially use Github and pull new docker images (github repo monitor)</li></ul>
<li>Web app display information</li>
<ul><li>CAS login or SSL</li></ul>
<li></li>
</ul>

Tier 3
<ul>
<li>Basic login</li>
<li>Graphs (resource usage)</li>
<li>Manipulate docker containers from web app</li>
<li>Manipulate database roles from web app</li>
<li>Enable/disable from web app</li>
<li>Student access to web app</li>
<li>Create, Edit, Delete Projects/containers from web app</li>
<li>Control resource allocation for each docker container from web app</li>
<li>Individual DB accounts per user</li>
<ul><li>Potentially tie to CAS</li></ul>
</ul>

### <u>Creating A Docker Container From .NET Project</u>
You will need a DockerHub account to proceed
<ul>
<li>Open your solution in Visual Studio</li>
<li>Right click your project (NOT the solution) in the Solution Explorer</li>
<li>Select Add > Docker Support</li>
<ul><li>Visual Studio will download and install Docker Desktop for you, requiring a reboot/li></ul>
<li>Right click the project after Docker Desktop has installed and is running, and select Publish</li>
<li>Follow instructions on screen to publish to Docker Hub</li>
</ul>
Once your image is published to docker hub, on the server, you can run the command `sudo docker pull yourusername/projectname:latest` to pull the image onto the server.

### <u>Running your Docker Container and Forwarding Ports</u>
Once you have the docker container on the server, you'll need to be able to run it, and forward from an arbitrary port on the host machine, to port 80 or 443 on the container. To do so, run a command similar to `sudo docker run -d -p 9001:80 yourusername/projectname`, where 9001 is the port on the host machine, and 80 is the port on the container your web app is being served. The -d means you're running it in detached mode, so you can do other things in your terminal, -p sets which port is being forwarded.
