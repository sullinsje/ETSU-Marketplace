#!/bin/bash

# variables
IMAGE_NAME="ghcr.io/sullinsje/etsu-marketplace:latest"
CONTAINER_NAME="marketplace-app"

echo "--- Starting Deployment for $CONTAINER_NAME ---"

# Pull the latest image from GHCR
echo "Pulling latest image..."
docker pull $IMAGE_NAME

# Stop and Remove the old container
# We use || true so the script doesn't crash if the container doesn't exist yet
echo "Stopping old container..."
docker stop $CONTAINER_NAME || true
docker rm $CONTAINER_NAME || true

# Run the new container
# Note: We keep the -v (volume) so your SQLite data persists!
echo "Starting new container..."
docker run -d \
  --name $CONTAINER_NAME \
  -p 8080:8080 \
  -v marketplace-data:/app/Data \
  -v marketplace-images:/app/wwwroot/images/listings \
  --restart unless-stopped \
  $IMAGE_NAME

# Cleanup: Remove "dangling" images
# This deletes the OLD version of the image that is now untagged (<none>)
# This saves disk space on your EC2 without touching your volumes!
echo "Cleaning up old image layers..."
docker image prune -f

echo "--- Deployment Complete! ---"
docker ps | grep $CONTAINER_NAME
