FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS builder
COPY ./app /app
WORKDIR /app
RUN dotnet publish -o /app/build -c Release

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
COPY --from=builder /app/build /app
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:5225
EXPOSE 5225
ENTRYPOINT [ "./app"]