# 🏃‍♀️ RunClub Frontend

Welcome to the **RunClub Frontend** — a modern, single-page React application built to power a running club's digital experience for **runners**, **coaches**, and **admins**. This app is connected to a .NET Core backend API and provides users with event discovery, enrollment, coach support, and administrative tools.

---

## 🚀 Features

### 👥 User Roles
- **Runners**: Browse events, enroll, track progress, and manage accounts.
- **Coaches**: Support runners, track their progress, and manage accounts.
- **Admins**: Create/edit events and coaches, manage FAQs, media gallery, and the club's story.

### 🧩 Functionality
- Event search and quick enrollment
- Coach directory with dynamic rating & bio display
- Editable media gallery (admin-only)
- Custom FAQ and contact section
- Profile management for all roles
- Route protection with role-based access

---

## 🛠️ Tech Stack

- **React 18+**
- **React Router DOM**
- **Bootstrap 5** + custom styles
- **Framer Motion** (for animations)
- **Axios** for API calls
- **Context API** for global user/auth state
- **Vite** for blazing-fast development

---

## 📁 Project Structure

```plaintext
src/
├── components/       # Shared UI components (Header, ProtectedRoute, etc.)
├── context/          # Global AppContext (auth, user info)
├── layouts/          # Layouts for Main, Guest, Admin, Coach
├── pages/
│   ├── Admin/        # Admin-specific tools and dashboards
│   ├── Auth/         # Login page
│   ├── Coach/        # Coach-specific views
│   ├── Public/       # Publicly accessible pages (Story, Gallery, Help)
│   ├── Runner/       # Runner dashboard, enrollments, etc.
├── routes/           # Custom route protection components
├── services/         # Axios API setup
└── App.jsx           # Main app routing


## 📦 Getting Started

### 1️⃣ Clone the Repo

```bash
git clone https://github.com/your-username/runclub-frontend.git
cd runclub-frontend

 2️⃣ Install Dependencies

npm install

3️⃣ Configure API Base URL

Update the API base URL in:

src/services/api.js

const api = axios.create({
  baseURL: 'http://localhost:5000/api', // change to your backend URL
});

4️⃣ Run the Dev Server

npm run dev

##✅ Authentication
- Auth is managed through a login form and persisted via global context.
- Role-based access (admin, coach, runner) determines which routes/pages a user can access.
- Protected routes are implemented using <PrivateRoute />.

##✨ Admin Tools
Admins can:
- ✅ Add, edit, delete events
- ✅ Manage coaches
- ✅ Edit the FAQ and club story
- ✅ Manage a dynamic media gallery
- All features are accessible via the /admin section.
