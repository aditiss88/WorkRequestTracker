# WorkRequestTracker

A full-stack project to manage work requests.  
Backend built with ASP.NET Core, frontend with React + Vite.

## Features
- Create, update, and track work requests
- Filter and search requests
- SQL database integration
- Simple UI with React + TypeScript

## Project Structure
- `WorkRequestTracker.Api/` → Backend API
- `work-request-tracker-ui/` → Frontend UI
- `.gitignore` → Git configuration

## Setup

### Backend
```terminal
cd WorkRequestTracker.Api
dotnet restore
dotnet run

### Frontend
```terminal
cd work-request-tracker-ui
npm install
npm run dev
