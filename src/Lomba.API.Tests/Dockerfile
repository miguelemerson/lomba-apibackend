FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app
COPY . .
RUN dotnet restore ./Lomba.API.Tests.csproj
RUN dotnet test -e MERGETEST=merge

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
#ENTRYPOINT [ "dotnet", "/app/Lomba.API.Tests.dll" ]