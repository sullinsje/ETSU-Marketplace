# syntax=docker/dockerfile:1

# 1. Build Stage
# Using the .NET 10.0 SDK on Alpine for a lightweight, fast build environment
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG TARGETARCH
WORKDIR /source

# Copy project file and restore dependencies
# This layer is cached unless the .csproj changes
COPY ["ETSU-Marketplace.csproj", "./"]
RUN dotnet restore -a ${TARGETARCH/amd64/x64}

# Copy the rest of the source code
COPY . .

# Build and publish the application using the .NET 10.0 runtime
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish "ETSU-Marketplace.csproj" \
    -a ${TARGETARCH/amd64/x64} \
    --use-current-runtime \
    --self-contained false \
    -o /app

# 2. Final Runtime Stage
# Minimal runtime image for .NET 10.0
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app

# Install ICU libraries (Required for SQLite & Globalization on Alpine)
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Copy the published app from the build stage
COPY --from=build /app .

# Create Data directory and set permissions for the SQLite database
RUN mkdir -p /app/Data /app/wwwroot/images/listings

# Apply ownership to everything at once
RUN chown -R $APP_UID:$APP_UID /app/Data /app/wwwroot/images

# Standard port for ASP.NET Core and Prometheus metrics
EXPOSE 8080

# Switch to the non-privileged user for security
USER $APP_UID

ENTRYPOINT ["dotnet", "ETSU-Marketplace.dll"]