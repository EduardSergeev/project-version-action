FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

WORKDIR /work
COPY . .
WORKDIR /work/test/dotnet/attribute
RUN dotnet build

ENTRYPOINT ["dotnet", "test"]
