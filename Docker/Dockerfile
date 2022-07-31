FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /opt/Build

COPY Source/. .
RUN dotnet publish -c Release -o /opt/Built Isla/Isla.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS run
WORKDIR /opt/Run

# Install cultures, these are missing in the alpine image.
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /opt/Built .

ENTRYPOINT ["dotnet", "Isla.dll"]