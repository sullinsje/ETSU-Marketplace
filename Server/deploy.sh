#!/bin/bash

COMPOSE_FILE="/home/ubuntu/compose.yaml"

echo "--- Starting Deployment ---"

docker compose -f $COMPOSE_FILE pull
docker compose -f $COMPOSE_FILE up -d --remove-orphans

echo "Cleaning up old image layers..."
docker image prune -f

echo "--- Deployment Complete! ---"
docker compose -f $COMPOSE_FILE ps
