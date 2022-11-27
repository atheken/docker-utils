FROM --platform=amd64 mcr.microsoft.com/dotnet/sdk:6.0 AS builder
COPY ./app /app
WORKDIR /app
RUN dotnet publish -c Release -o ./bin/published
FROM --platform=amd64 public.ecr.aws/lambda/dotnet:6
COPY --from=builder /app/bin/published/* ${LAMBDA_TASK_ROOT}/
EXPOSE 8080
CMD [ "app::app.Function::Handler" ]