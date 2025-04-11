import { Outlet } from 'react-router-dom'
import CoachHeader from '../components/CoachHeader'

const CoachLayout = () => {
  return (
    <>
      <CoachHeader />
      <main className="container mt-4">
        <Outlet />
      </main>
    </>
  )
}

export default CoachLayout
