FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Part.VideoUploader.Api/Part.VideoUploader.Api.csproj", "Part.VideoUploader.Api/"]
RUN dotnet restore "Part.VideoUploader.Api/Part.VideoUploader.Api.csproj"
COPY . .
WORKDIR "/src/Part.VideoUploader.Api"
RUN dotnet build "Part.VideoUploader.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Part.VideoUploader.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Part.VideoUploader.Api.dll"]
