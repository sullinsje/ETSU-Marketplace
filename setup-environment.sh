#!/bin/bash

# Update system packages first to ensure security patches
sudo apt-get update && sudo apt-get upgrade -y

# Official Docker Install
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Add user to docker group
sudo usermod -aG docker $USER

echo "DONE: Docker installed. IMPORTANT: Run 'exit' and log back in to refresh permissions."
