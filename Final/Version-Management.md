# Version Management

## Docker & GitHub Container Registry
- GitHub Actions, on a successful workflow run, will use Docker to automatically containerize the application and upload it to GitHub Container Registry
- Due to the frequency of commits and versions and by the nature of DevOps, versions are not inherently tracked.
- This application sits on a server anyways, so versioning is not too important.
- For documentation purposes, GitHub Container Registry does track the versions using a hash. You can install specific versions by their hash
