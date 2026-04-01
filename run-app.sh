#!/bin/bash

cd "$HOME/ETSU-Marketplace/ETSU-Marketplace/publish" || exit
dotnet ETSU-Marketplace.dll --urls "http://0.0.0.0:80"

