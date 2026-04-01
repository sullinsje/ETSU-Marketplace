#!/bin/bash

set -e

echo "--- Starting ETSU Marketplace Environment Setup ---"

# Update package list
sudo apt-get update

# Install prerequisites for .NET repository
sudo add-apt-repository ppa:dotnet/backports
sudo apt-get update -y && \
  sudo apt-get install -y dotnet-sdk-9.0 sqlite3 libsqlite3-dev git

# Verify .NET installation
echo "Checking .NET version..."
dotnet --version

# Install EF Core Global Tool
# Useful for running migrations directly on the server if needed
echo "Installing EF Core tools..."
dotnet tool install --global dotnet-ef --version 9.* || echo "EF tools already installed."

# Add to .bashrc for FUTURE logins
if ! grep -q 'dotnet/tools' ~/.bashrc; then
    echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.bashrc
fi

export PATH="$PATH:$HOME/.dotnet/tools"

# Verify it works immediately
dotnet ef --version

# make directory and clone repo into it
mkdir -p ~/ETSU-Marketplace/
git clone https://github.com/sullinsje/ETSU-Marketplace ~/ETSU-Marketplace/

# make listings folder RWX for everyone
chmod 777 ~/ETSU-Marketplace/ETSU-Marketplace/wwwroot/images/listings

cd "$HOME/ETSU-Marketplace/ETSU-Marketplace" || exit

# update database and publish
dotnet ef database update
mkdir -p ./publish
dotnet publish -c Release -o ./publish

echo "--- Setup Complete ---"
