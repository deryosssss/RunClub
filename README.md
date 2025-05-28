# 🏃‍♂️ RunClub Web Application

A full-stack web platform designed to empower runners, coaches, and administrators to connect, manage events, and track progress. Built with a React frontend and a secure ASP.NET Core backend.

---

## Table of Contents

1. [Project Title](#1-project-title)  
2. [Project Description](#2-project-description)  
3. [Features](#3-features)  
4. [Frontend Setup](#4-frontend-setup)  
5. [Backend Setup](#5-backend-setup)  
6. [Authentication & Roles](#6-authentication--roles)  
7. [Running Tests](#7-running-tests)  
8. [API Documentation](#8-api-documentation)  
9. [Production Deployment](#9-production-deployment)  
10. [Contributing](#10-contributing)  
11. [Contact](#11-contact)

---

## 1. Project Title

**Momentum** – A modern full-stack platform for managing running club memberships, event enrollments, coaching, and progress tracking.

---

## 2. Project Description

Momentum combines a fast React frontend with a robust .NET Core RESTful API to deliver a seamless experience for runners, coaches, and administrators. It includes event discovery, enrollments, progress logs, role-based dashboards, and admin tools — all with secure JWT authentication and responsive design.

---

## 3. Features

### User Roles
- **Runners**: Enroll in events, track progress, manage profiles.
- **Coaches**: Support runners, monitor stats, manage users.
- **Admins**: Full control over events, coaches, media, and FAQs.

### Platform Capabilities
- Event search & enrollment
- Coach directory with ratings
- Media gallery & FAQ (admin-controlled)
- Role-based protected routes
- Real-time validation and error handling
- Responsive design for mobile and desktop
- Health check & Swagger API docs

---

## 4. Frontend Setup

### Tech Stack
- React 18+
- React Router DOM
- Axios
- Bootstrap 5
- Framer Motion
- Vite
- Context API

###  Getting Started

```bash
git clone https://github.com/deryosssss/RunClub-frontend.git
cd RunClub-frontend
npm install
```

### Configure API

Update `src/services/Api.js`:

```js
const api = axios.create({
  baseURL: 'http://localhost:5187/api', // or your deployed API
});
```

###  Run Frontend
```bash
npm run dev
```

---

## 5. Backend Setup

### Tech Stack
- ASP.NET Core 7+
- Entity Framework Core
- SQLite / SQL Server
- Swagger
- JWT Auth
- IP Rate Limiting

### Setup

```bash
git clone https://github.com/deryosssss/RunClub.git
cd runclub-api
dotnet restore
dotnet ef database update
dotnet run
```

### .env Setup (backend)

Create a `.env` or use appsettings:

```env
ConnectionStrings__RunClubDb="YourDatabaseConnectionString"
Jwt__Key="YourSuperSecretKey"
Jwt__Issuer="https://yourapi.com"
Jwt__Audience="https://yourapi.com"
AllowedOrigins="http://localhost:5173"
```

---

## 6. Authentication & Roles

- Login handled via JWT token stored in localStorage
- Token automatically added to requests via Axios interceptor
- Roles (`admin`, `coach`, `runner`) determine access to routes
- React context (`AppContext`) maintains session across the app

---

## 7. Running Tests

### Frontend (using Vitest + Testing Library)

```bash
npm run test
```

- Component tests (e.g., Header)
- Mocked API calls with axios
- Role-based logic tests

### Backend (using xUnit)

```bash
dotnet test
```

- Unit & integration tests for controller logic and services

---

## 8. API Documentation

### Swagger UI:
- Local: [http://localhost:5187/swagger](http://localhost:5187/swagger)
- Production: [https://runclub-api.azurewebsites.net/swagger](https://runclub-api.azurewebsites.net/swagger)

---

## 9. Production Deployment (Azure)

- **Frontend**: Vite React app hosted on Azure Static Web Apps or Netlify
- **Backend**: Deployed to Azure App Service
- **Health check**:  
  [https://myrunclub.azurewebsites.net/health](https://myrunclub.azurewebsites.net/health)

---

## 10. Contributing

We welcome contributions!  
Steps to contribute:

1. Fork the repo  
2. Create a branch (`git checkout -b feature-name`)  
3. Commit your changes (`git commit -m 'Add new feature'`)  
4. Push and create a Pull Request  

For major features or questions, please open an issue first.

---

## 11. Contact

- Issues: [RunClub GitHub Issues](https://github.com/deryosss/runclub/issues)
- Author: [@deryosssss](https://github.com/deryosssss)

---

**Thank you for using RunClub! 🏃‍♀️ Let’s go the extra mile.**
