FROM mcr.microsoft.com/dotnet/sdk:2.1-alpine AS build

# for dotnet build package initialization purposes
RUN dotnet new console -o consolebasicapp && \
    dotnet publish consolebasicapp -c Release


# copying current project #
WORKDIR /API
COPY ./Control.Facilites ./Control.Facilites
COPY ./Control.Facilites.AppServices ./Control.Facilites.AppServices
COPY ./Control.Facilites.Data ./Control.Facilites.Data
COPY ./Control.Facilites.Domain ./Control.Facilites.Domain
COPY ./Control.Facilites.DomainServices ./Control.Facilites.DomainServices
COPY ./Control.Facilites.IoC ./Control.Facilites.IoC

RUN dotnet publish Control.Facilites -c Release -o ../start


# --------- Runtime Project ---------
FROM mcr.microsoft.com/dotnet/aspnet:2.1-alpine AS runtime
WORKDIR /app
COPY --from=build /API/start .
ENV ASPNETCORE_URLS=http://+:5027
EXPOSE 5000/tcp
#ENTRYPOINT ["dotnet", "Control.Facilites.dll"]
#ENTRYPOINT ["/bin/sh"]

