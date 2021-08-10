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

