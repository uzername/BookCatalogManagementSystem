FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ENV DOTNET_NUGET_SIGNATURE_VERIFICATION=false
COPY ["BCMS_FrontendBlazor/BCMS_FrontendBlazor.csproj", "BCMS_FrontendBlazor/"]
RUN dotnet restore "BCMS_FrontendBlazor/BCMS_FrontendBlazor.csproj"
COPY . .
WORKDIR "/src/BCMS_FrontendBlazor"
RUN dotnet build "BCMS_FrontendBlazor.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "BCMS_FrontendBlazor.csproj" -c Release -o /app/publish /p:UseAppHost=false
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
RUN apk add jq
RUN apk add nano
COPY ["BCMS_FrontendBlazor/script.sh", "/docker-entrypoint.d/35-script.sh"]
RUN chmod +x /docker-entrypoint.d/35-script.sh
COPY --from=publish /app/publish/wwwroot . 
EXPOSE 7164
CMD /docker-entrypoint.d/35-script.sh && nginx -g "daemon off;"