#!/bin/bash
APP_DIR="$HOME/ETSU-Marketplace/ETSU-Marketplace"

# DevSecOps Practice: Simple Linting
# Check if Dockerfile is trying to run as 'root' (A common security finding)
if grep -q "USER root" "$APP_DIR/Dockerfile"; then
    echo "WARNING: Dockerfile is explicitly set to root. Consider using a non-root user."
fi

# DevSecOps Practice: Check for sensitive files
if [ ! -f "$APP_DIR/.dockerignore" ]; then
    echo "SECURITY ALERT: No .dockerignore found! You might accidentally upload secrets to your image."
fi

# Use 'sg' to run docker compose with the 'docker' group privileges 
# without needing to log out and back in.
cd "$APP_DIR"
sg docker -c "docker compose up --build -d"

echo "ETSU Marketplace is starting via 'sg' group elevation..."
