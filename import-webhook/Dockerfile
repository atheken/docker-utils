FROM --platform=amd64 mcr.microsoft.com/dotnet/sdk:7.0-alpine AS builder
RUN mkdir /app
COPY ./src /app/src
COPY ./tests /app/tests
COPY ./*.sln ./app
WORKDIR /app
RUN dotnet test
RUN dotnet publish -o /app/build -c Release

FROM  --platform=amd64 mcr.microsoft.com/dotnet/aspnet:7.0-alpine
COPY --from=builder /app/build /app
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:5225
EXPOSE 5225
ENTRYPOINT [ "./app"]