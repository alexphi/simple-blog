FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://+:80
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /
COPY src/ src/
WORKDIR /src/web
RUN dotnet build -c Release -o /dist

FROM build as publish
RUN dotnet publish -c Release -o /dist

FROM base
WORKDIR /app
COPY --from=publish /dist .
ENTRYPOINT ["dotnet", "AlejoF.SimpleBlog.dll"]