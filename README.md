# Server Manager
### Weber State CS 4450

#### <u>*The Purpose*</u>
The purpose of the Server Manager project is to provide Weber State students a simple way to host their web applications on a publicly accessible server. Weber state has set up the subdomain [csprojects.weber.edu](http://csprojects.weber.edu) to accomplish this purpose, and allocated us a Linux VM to do so.

## START HERE

The interface of the project can be found at http://csprojects.weber.edu, and is primarily made for two purposes

1. Allow professors to easily manage which docker containers (student projects) are running and how many resources each one takes
2. Allow students to upload a container, have a professor approve it, and have their project hosted with just a few button clicks

The Server Manager Tracking Web Application is not running in a docker container. The project is located in the following locations:

- All source files are located in `/opt/Tracker/Server-Manager/`, this folder contains the clone of this repository.
- Published files are located in `/opt/Tracker/Server-Tracker/`
- The application is being run with a linux service at `/etc/systemd/system/tracker-app.service`

The application makes use of a Microsoft SQL database located on the server, which is used to keep track of projects and the technology they use. This data is manually entered when uploading a new project. You can associate each project with a docker container using it's id, and by using the docker API, you can get information about each container, like it's state and uptime, and although not currently implemented, you can also use the api to allow the application to track resource usage, activate and deactivate containers, etc.

**You will need to ask Brad to create a user with root privileges for each of you on the server this project is hosted on.**

## Setting Up and Configuring A New Project

- ssh to Icarus or hop on the Weber VPN
- Use icarus (or connect via VPN) as a jumpbox to the server everything is being hosted on
- Run your docker container, and make sure to forward from an open port on the host machine to the container's port that the website is on (`sudo docker run -d -p 9001:80 yourusername/projectname`)
- `cd /etc/nginx/sites-available`
- Use your favorite text editor (vim/pico) to edit the file named default (`/etc/nginx/sites-available/default`)
- At the bottom of the file, before the last `}`, paste the following, swapping out [appname] with your application name/url, and [port] with the port that is forwarding to your docker container

```
location /[appname]/ {
	proxy_pass http://127.0.0.1:[port]/;
	proxy_set_header Accept-Encoding "";
        sub_filter "src=\"/" "src=\"/[appname]/";
	sub_filter "href=\"/" "href=\"/[appname]/";
	sub_filter_types *;
	sub_filter_once off;
}
```

- Run the command `sudo nginx -s reload` to refresh the NGINX configs. Your site should be hosted at http://csprojects.weber.edu/[appname] if everything works correctly.

## CURRENT KNOWN ISSUES
- Currently Subfilter is not a sufficient solution for all issues on hosting a multi-page web application, it does not handle redirects. A solution for handling redirects properly needs to be found.
- The web application probably shouldn't use the user access level in cache, in case it ever changes while they are still logged in
- SSL/HTTPS may not work without giving warnings

## <u>*Server Tracking Web Application General Information*</u>
The Server Project Tracker Application is a .NET Core 5.0 ASP.net Razor application and uses Entity Framework Core (the standard for .NET). As re reminder, the source files are located on the server at `/opt/Tracker/Server-Manager/`. All published files are located in `/opt/Tracker/Server-Tracker/`.

Using the database is detailed further down. For connecting to the database, the connection string can be found in:
- From the server at `/opt/Tracker/Server-Manager/ServerProjectTracker/ServerProjectTracker/appsettings.json`
- From this github repositroy `Server-Manager/ServerProjectTracker/ServerProjectTracker/appsettings.json`. 
- And the connection string should be `"Data Source=127.0.0.1,1433;Initial Catalog=Server_Tracker;User ID=SA;Password=CSWeber!"`. 
 
**However it should be noted that this connection string will only work when running from on the server itself, or if you are behind the Weber firewall and have an ssh tunnel setup to bind your local port 1433 to the server's port 1433.**

If you need to update the web app on the server with the latest git push, the following commands can update it:

1. `cd /opt/Tracker/Server-Manager`
2. `git pull origin master`
3. `cd /ServerProjectTracker`
4. `sudo dotnet publish -c Release -o /opt/Tracker/Server-Tracker`

You should always publish to the Server-Tracker folder, as those files are looked at by the associated service for the web application. This service is located at `/etc/systemd/system` and is called `tracker-app.service`. Once you update the files make sure to restart the service by using the command: `sudo systemctl restart tracker-app.service`

**Our application for now uses a simple unsecure login system, so please do not use a common password. We intended to eventually have this app use Weber State's CAS login system, but were unable to implement it.**

#### Web Application Detailed Information
This web application uses Entity framework, which means we have a few model classes that are reflected in the database. Currently the application has the following tables:
- Users
- Project
- ProjectUsers
- Container

**Users**: Is the simplest class, and is what is used for the simple login system. For now it stores a hashed password (Never store a raw password in a database), and their username. There is a field called "CasId" which was left an ambigious string, as the original intent was to use Weber State's CAS login system, so that field can hold whatever is returned by that system that is needed to connect a user to weber state's system, at which point, their username should probably be equal to their eWeber login username, and the password unneccessary. It also stores their first and last name, but the information is not really used.

The more imporant part to understand in the User Table is the User Access Level, as this determines the overall rights a user has in the web application
- Level 0: Is Global Ownership Access, this means they have full control of **everything**, this should be strictly limited to site admins.
- Level 1: Is Global Developer Access, this means that they can partially modify any project.
- Level 2: Is Global Viewer Access, meaning that they can at least see every project, allowing them to see basic information but no modification rights.
- Level 3: Is Default Access, in other words, their access level is set per individual project.
- Level 4: Is Pending Access, this can be used to approve any new users that login to the system a level 0 admin must approve them access before they can do **anything** on the web app, even if they are added to a specific project.
- Level 5: Is Rejected/Revoked Access, this prevents a user from accessing **anything** on the web app. They will only be able to access something until a level 0 admin returns access.

By default, even if a user has above level 3 access, if their access is better on an individual project, they will get that access level with that project (A level 2 viewer with developer access to a specific project will have developer rights for that project). People with below level 3 access will have no access to any project. Most importantly **NEVER allow a user to modify their own access level**.

**Project**: this table tracks information that is the whole point of the web application. A project contains the title, and description as required fields, all remaining fields are optional. The most important optional field includes the DockerId, which connects a project to the Docker Continer, allowing the web application to manipulate (currently only view) docker containers and their information, like thier current running state and uptime. 

Other optional fields includes the projects programming language (Java, C#, Javascript, etc.), the project's database (MySQL, Microsoft SQL, MongoDB, etc.), Project Backend (ASP.net, React, Django, etc.), and a catch all for any other major technology that they might be using is ProjectTechnologyMisc. The ProjectLink is a url, which can be pointed to the nginx location (http://csprojects.weber.edu/[appname]/), and finally, the ProjectImageLink, which is currently NOT implemented. It was added in case someone might want to add something visual to distinguish each project.

**ProjectUsers**: This table is the link between an individual user and their access rights for a project. Importantly, these access rules track when and who gave the current access level, but it might not be a bad idea to keep track of all access rule changes (including user access level changes) in the future, if neccessary. This is to create accountibility. 

The Access levels for a project are very similar to that of global access levels. but some changes.
- Level 0: Is Owner Access, or full access
- Level 1: Is Developer Access, or partial access, they cannot modify project security (they cannot modify entires on this table)
- Level 2: Is Viewer Access, they can only view basic information but cannot modify anything in anyway.

A notable exception is a negative access level (maybe this should be changed in the future) indicates that access was previously given but has since been revoked. Rather than delete entires this allows you to track if a user might have ever had access in the past, but no longer does, and when.

**Container**: Lastly, the container is not currently on the database, it can be though if a migration is made using the Package Manger Console. It is simply an easy way to transport data from the Docker API to a webpage, but it could be used to track information about a Docker Container, such as possibly it's resource usage history or anything else that might become neccessary.

## <u>*Nginx Information*</u>

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

<b>There are a few caveats with this solution, although unlikely, this solution probably doesn't work with any absolute links. Though we have not gotten to test it. We have not tested it, so we don't know if this will modify javascript files so this may not work with any javascript based front end web applications.</b>

The last 2 things in the location block to note is the sub_filter_types, which is simply to make it apply to all requested files, and the sub_filter_once needs to be off, to ensure that it applies this filter on every request.

## <u>Creating A Docker Container From .NET Project</u>
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
Once you have the docker container on the server, you'll need to be able to run it, and forward from an arbitrary port on the host machine, to port 80 or 443 on the container. To do so, run a command similar to `sudo docker run -d -p 9001:80 yourusername/projectname`, where 9001 is the port on the host machine, and 80 is the port on the container your web app is being served. The -d means you're running it in detached mode, so you can do other things in your terminal, -p sets which port is being forwarded. If you have environment variables that need to be set, you can do so with the -e flag

### <u>Microsoft SQL Server</u>
There is currently a MS SQL Server 2019 Developer edition running on the server.  The reason we chose the  developer edition is that it lets developers build any kind of application on top of SQL Server. It includes all the functionality of Enterprise edition, but is licensed for use as a development and test system, not as a production server.  It was also free this way and it lines up well with what we are using it for.

When a team needs a database on the server what you want to do is create a server for them on the admin account (login information in connecting section).  Then you will need to create a login for them followed by a user.  The user should be tied to the database such that they can only access that database.  Then assign the user to the role 'db_owner' to make them the owner of the database.  If you don't give them a role then they won't be able to do anything on the database.  You can do all of this in a simple C# class that we created, there will also be a link to how to create in the link section.

<b>Connecting to the server</b>

The first thing that you need to do in order to connect to the MS SQL Server is to download weber states vpn and connect to it.  The instructions can be found <a href="https://www.weber.edu/help/kb/VPN_Install.html">here</a> 

Next you will need to setup putty.  To setup putty the first thing you need to do is enter your username followed by “@137.190.18.16” in the Host Name and enter 22 under port.  Then enter a descriptive name under Saved Sessions and click save so you don’t have to enter everything again (Shown Below).

![alt text](https://github.com/Ben-Mullins/Server-Manager/blob/master/Images/ReadmeSQL1.png?raw=true)
1. Now on the left side of putty there is a Category section. Under Connections you want to hit the “+” next to SSH.
2. Next click on Tunnels
3. Under source port enter 4444
4. Under destinations enter 127.0.0.1:1433
5. click add

![alt text](https://github.com/Ben-Mullins/Server-Manager/blob/master/Images/ReadmeSQL2.png?raw=true)

Now your putty should look like the image below. Scroll back up in the Category section and click session then click save.

![alt text](https://github.com/Ben-Mullins/Server-Manager/blob/master/Images/ReadmeSQL3.png?raw=true)

Now click the open putty and it will prompt you to enter your password to the server. Enter your password and minimize the window. <b>DO NOT CLOSE IT OR YOU WON’T BE ABLE TO CONNECT TO THE MS SQL SERVER.</b>

The last thing to do is open up Microsoft SQL Server Management Studio, or any other way you would like to connect, and enter the server name as 127.0.0.1,4444 and your login information.  <b>The admin account's username is 'SA' and the password is 'CSWeber!'.</b> (See image below)

![alt text](https://github.com/Ben-Mullins/Server-Manager/blob/master/Images/ReadmeSQL4.png?raw=true)

<b>Commands and Queries</b>
1. To get the current status of the server run:
	`systemctl status mssql-server`

2. To stop the server
`sudo systemctl stop mssql-server`

3. To start the server
`sudo systemctl start mssql-server`

4. You can run queries in the command line by using 
`/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'CSWeber!'` 
Then you should the see a '1>' and you can start entering your queries.  When you are done entering them all just hit enter, type go, and then it will execute them.  Below are some useful queries you can use.

5. To create a new login that will have the user change their password
`CREATE LOGIN <login_name> WITH PASSWORD = '<enterStrongPasswordHere>' MUST_CHANGE, CHECK_EXPIRATION = ON;`

6. Create a new database
`CREATE DATABASE <database_name>`

7. Create a user for a database
`USE <database_name>
CREATE USER <user_name> FOR LOGIN <login_name>`

8. Make the user the owner of the database
`USE <database_name> EXEC sp_addrolemember 'db_owner>', '<user_name>'`

## <u>Docker API</u>

A .NET specific docker API is available for use at [github.com/dotnet/Docker.DotNet](https://github.com/dotnet/Docker.DotNet) which includes instructions for installation and usage. So far in the Server Tracker Project only getting a list of containers has been used. In the future more commands can be implemented to enable the Project to interact completely with the Docker daemon through the application rather than having to use the command line for getting, starting, stopping, and deleting containers/images.

## List of Helpful Docker Commands
**Commands need to be run as root**

`sudo docker ps` :: List all currently running containers

`sudo docker image ls` :: List all images

`sudo docker run -it -d -o 9001:80 myusername/myproject:latest` :: runs your project in detatched mode, forwarding the host device's 9001 to the container's port 80

`sudo docker kill imageid` :: Kills the image with the ID supplied. ID can be found with sudo docker ps

## <u>To Do List</u>
#### Tier 1
<ul>
<li>Ability to pass professor a docker image and have the professor upload it to the Linux server ✓</li>
<ul><li>Professor will be responsible for setting up Nginx reverse proxy and forwarding correct port from host to container</li></ul>
<li>Be able to host multiple docker container projects on the same server ✓</li>
<li>All shared databases installed on Linux server (Currently MSSQL only) ✓</li>
<li>Role based access for different project databases ✓</li>
<li>FERPA compliancy ✓</li>
</ul>

#### Tier 2
<ul>
<li>Automate docker containerization of projects (.NET, React, Java based?/other common web frameworks used by Weber State students/courses)</li>
<li>Docker API ✓</li>
<li>Automate project creation/deployment through API</li>
<li>Automate project deletion through API</li>
<li>Find way to support HTTPS without warnings</li>
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

#### Tier 3
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

## Useful Links
<ul>
<li><a href="https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-ubuntu?view=sql-server-ver15">How to install</a></li>
<li><a href="https://docs.microsoft.com/en-us/sql/relational-databases/security/authentication-access/database-level-roles?view=sql-server-ver15">Database-Level Roles</a></li>
<li><a href="https://docs.microsoft.com/en-us/sql/relational-databases/system-stored-procedures/sp-addrolemember-transact-sql?view=sql-server-ver15">Adding Roles</a></li>
<li><a href="https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-security-get-started?view=sql-server-ver15">Security</a></li>
<li><a href="https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlcommand?redirectedfrom=MSDN&view=dotnet-plat-ext-5.0">C# SQL Command Class</a></li>
</ul>

## <u>Other Databases</u>
We did look into putting other databases on the server but only ended up putting one on there for now.  One thing to pay attention to if you are going to put more on there is the amount of ram each one takes up.  There is currently only 8GB of ram on the server and each database takes somewhere between 2-4GB to run.  I will provide some useful links below to other database that we researched but didn't put on.

<ul>
<li><a href="https://likegeeks.com/mysql-on-linux-beginners-tutorial">My SQL</a></li>
<li><a href="https://docs.mongodb.com/manual/tutorial/install-mongodb-on-ubuntu/">MongoDB</a></li>
</ul>
