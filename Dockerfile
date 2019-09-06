# This is a multi-stage docker file
# https://docs.docker.com/engine/userguide/eng-image/multistage-build/#use-multi-stage-builds

##### Stage 1 (Build)
FROM microsoft/dotnet:2.2-sdk as builder

# This should almost never change, so keeping it high in order to always cache
COPY nuget.config .

# Keep the .csproj on different COPY commands to make sure if any change, it invalidates the below caches
COPY PaymentGateway/PaymentGateway.csproj ./PaymentGateway/

# Keep on a single RUN to only create a single cache layer
RUN dotnet restore ./PaymentGateway/PaymentGateway.csproj

# Keep this near the end since it will use the above cache layers if nothing above changes
COPY . .

RUN dotnet restore PaymentGateway/PaymentGateway.csproj
RUN dotnet publish PaymentGateway/PaymentGateway.csproj --output /app/ --configuration Release 

##### Stage 2 (Package)
FROM microsoft/dotnet:2.2-aspnetcore-runtime

# Create the home directory for the new app user.
RUN mkdir -p /home/app

# Create an app user so our program doesn't run as root.
RUN groupadd -r app && \
	useradd -r -g app -d /home/app -s /sbin/nologin -c "Docker image user" app

# Set the home directory to our app user's home.
ENV HOME=/home/app
ENV APP_HOME=/home/app/project

## SETTING UP THE APP ##
RUN mkdir $APP_HOME
WORKDIR $APP_HOME
USER app
ENTRYPOINT ["dotnet", "PaymentGateway.dll"]
EXPOSE 6125
ENV ASPNETCORE_URLS http://*:6125
COPY --from=builder /app/ $APP_HOME