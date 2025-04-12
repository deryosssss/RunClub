import { Outlet } from 'react-router-dom'
import AdminHeader from '../components/AdminHeader'

const AdminLayout = () => {
  return (
    <>
      <AdminHeader />
      <main className="container py-4" style={{ maxWidth: '1100px', margin: '0 auto' }}>
        <Outlet />
      </main>
    </>
  )
}

export default AdminLayout
