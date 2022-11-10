#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
# EXPOSE 80
# EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TodoListWebAPI/TodoListWebAPI.csproj", "TodoListWebAPI/"]
RUN dotnet restore "TodoListWebAPI/TodoListWebAPI.csproj"
COPY . .
WORKDIR "/src/TodoListWebAPI"
RUN dotnet build "TodoListWebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoListWebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "TodoListWebAPI.dll"]

# https://blog.miniasp.com/post/2022/05/28/Sum-up-ASPNETCORE-Environment-Variables
# docker run -p 5000:5000 -p 5001:5001 -e ASPNETCORE_HTTP_PORT=https://+:5001 -e ASPNETCORE_URLS=http://+:5000 <image_name>

# docker run -p 5000:5000 --link mongo:mongo <image_name>
