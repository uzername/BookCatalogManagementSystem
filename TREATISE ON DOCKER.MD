To run your .NET Core project in Docker and make it accessible on port 5195, you need to create a Dockerfile for your application, build the Docker image, and run it while mapping the desired port. Here's how to do it step by step:

1. Create a Dockerfile
Create a Dockerfile in the root of your project:

```
# Use the official .NET runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Copy the build output from your local machine
COPY ./bin/Release/net8.0/publish ./ 

# Expose port 5195
EXPOSE 5195

# Define the entry point
ENTRYPOINT ["dotnet", "BCMS_backend.dll"]
```
2. Build the Project for Release
Make sure to build your project in release mode and publish it:

```
dotnet publish -c Release -o ./bin/Release/net8.0/publish
```
3. Build the Docker Image
Build a Docker image using the Dockerfile:

```
docker build -t bcms-backend .
```
4. Run the Docker Container
Run the container while mapping port 5195 on the host to 5195 in the container:

```
docker run -d -p 5195:5195 bcms-backend --urls=http://+:5195
```
Here’s what happens:

-d: Runs the container in detached mode.
-p 5195:5195: Maps port 5195 on your local machine to port 5195 in the container.
--urls=http://+:5195: Configures the application to listen on all network interfaces within the container on port 5195.
5. Verify the Running Container
Check if the container is running:

```
docker ps
```
You should see your container running and exposing port 5195.

6. Access Your Application
Open a browser or use a tool like curl to access the application:

```
http://localhost:5195
```
If everything is set up correctly, your .NET Core project will now be running inside Docker and accessible via the specified port.