FROM microsoft/dotnet:latest AS build-env
COPY . /src/
WORKDIR /src
RUN dotnet publish Api -c Release -o out

FROM microsoft/dotnet:2.2-runtime
WORKDIR /app
COPY --from=build-env /src/Api/out .

ENV AwsAccessKeyId ""
ENV AwsSecretAccessKey ""
ENV CorsOrigins ""
ENV ConnectionStrings__DefaultConnection ""
ENV JwtSecret ""
ENV JwtIssuer ""
ENV JwtAudience ""

ENTRYPOINT [ "dotnet","Api.dll" ]