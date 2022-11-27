FROM --platform=amd64 mcr.microsoft.com/dotnet/sdk:6.0 AS builder
COPY ./app /app
WORKDIR /app
RUN donet build --configuration=Release
FROM --platform=amd64 public.ecr.aws/lambda/dotnet:6
COPY ./app/bin/Release/* ${LAMBDA_TASK_ROOT}
CMD [ "app::app.Function::Handler" ]