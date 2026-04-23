# Deployment Environment

## GitHub Container Registry (GHCR)
When development branches are pulled into the main and pass all tests, a GitHub Action containerizes the application with Docker and pushes it to GitHub Container Registry, a platform for hosting Docker containers

## Production Server
An Amazon Web Services EC2 Instance running Ubuntu acts as our production server:
- It has an Elastic IP (a static IP that AWS reserves) for consistent access through HTTP and SSH
- A `compose.yaml` file sits on the server that pulls the latest application image from GHCR as well as the images for our monitoring tools and NGINX. The images are all then run as containers over the necessary ports for functionality
- On boot (using `crontab`), it utilizes a deployment script `deploy.sh` that cleans up the previous containers and uses `compose.yaml` to pull fresh ones and run them
- An NGINX container runs on the server as a reverse proxy, forwarding HTTP requests from the Elastic IP to the application's domain (http://etsumarketplace.xyz)

## Running your own instance of ETSU Marketplace
- The server configuration can be placed on any Ubuntu server utilizing the scripts in the repository directory `ETSU-Marketplace/server/`
- Docker is required for the application to run and can be installed using the `setup.sh` script in `ETSU-Marketplace/server/`.
- Following the Docker installation, ensure that these files inside of `ETSU-Marketplace/server/` are placed in the `/home/ubuntu/` directory on an Ubuntu server:
  - `deploy.sh`
  - `compose.yaml`
  - `nginx.conf`
  - `prometheus.yml`
  - A `.env` file containing a GitHub Access Token for the ETSU Marketplace repository
- **You will to modify `nginx.conf` to point to your domain (if you have one). If not, a custom `nginx.conf` will likely be needed.**
