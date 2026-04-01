#!/bin/bash

cd "$HOME/ETSU-Marketplace/ETSU-Marketplace/publish" || exit
sudo dotnet ETSU-Marketplace.dll --urls "http://0.0.0.0:80"

