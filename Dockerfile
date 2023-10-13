# syntax=docker/dockerfile:1

# Create a stage for building the application.
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
ARG TARGETARCH

COPY . /source
WORKDIR /source/ibge

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app


# Create a new stage for running the application that contains the minimal
# runtime dependencies for the application.
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS final
WORKDIR /app

COPY --from=build /app .

ARG UID=10001
RUN adduser \
    --disabled-password \
    --gecos "" \
    --home "/nonexistent" \
    --shell "/sbin/nologin" \
    --no-create-home \
    --uid "${UID}" \
    appuser
USER appuser

ENTRYPOINT ["dotnet", "ibge.dll"]
