# VideoAPI

Simple ASP.NET Core minimal API for video streaming with MariaDB and a local NAS.

## Overview

This project demonstrates:

- Entity Framework Core with Pomelo MySQL/MariaDB provider
- Storing video metadata in MariaDB
- Serving video files from a locally mounted NAS share
- Uploading files via minimal API endpoints

## Getting Started

1. **Configure MariaDB**
   - Install MariaDB and create a database `videoapp`.
   - Update the connection string in `appsettings.json` (`DefaultConnection`).
   - Example:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Port=3306;Database=videoapp;User=root;Password=YourPassword;"
     }
     ```

2. **Mount NAS Share**
   - Ensure your NAS export is mounted on the host filesystem.  For example:
     ```bash
     sudo mount -t cifs //nas.local/videos /mnt/nas/videos -o username=nobody,password=guest
     ```
   - Update `appsettings.json` under `VideoSettings:NasBasePath` if different.

3. **Migrations**
   - Install the EF CLI tools if you haven't already:
     ```bash
     dotnet tool install --global dotnet-ef
     ```
   - Create an initial migration and apply it:
     ```bash
     dotnet ef migrations add InitialCreate
     dotnet ef database update
     ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Sample endpoints**
   - `GET /videos/{id}/stream` - stream a video by ID
   - `POST /videos` - add metadata (JSON body)
   - `POST /videos/upload` - multipart form upload (`file`, `title`)

## Notes

- Video files are saved to the configured NAS base path.  Only metadata is stored in MariaDB.
- For production, configure authentication/authorization and secure file handling.
- Use a reverse proxy or CDN for efficient streaming as needed.

