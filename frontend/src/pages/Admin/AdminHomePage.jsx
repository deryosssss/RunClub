import { useNavigate } from 'react-router-dom'
import './AdminHomePage.css'

const AdminHomePage = () => {
  const navigate = useNavigate()

  const cards = [
    {
      title: 'Coaches',
      description: 'View, create, and manage coach accounts.',
      path: '/admin/coaches',
      emoji: '🧑‍🏫',
    },
    {
      title: 'Events',
      description: 'Manage running events across the platform.',
      path: '/admin/events',
      emoji: '🏃‍♂️',
    },
    {
      title: 'FAQ',
      description: 'Edit the frequently asked questions section.',
      path: '/admin/faq',
      emoji: '❓',
    },
    {
      title: 'Gallery',
      description: 'Add and manage media in the gallery.',
      path: '/admin/gallery',
      emoji: '🖼️',
    },
    {
      title: 'Our Story',
      description: 'Update the platform’s background story.',
      path: '/admin/our-story',
      emoji: '📖',
    },
    {
      title: 'Account',
      description: 'View and edit your profile.',
      path: '/admin/account/me',
      emoji: '👤',
    },
  ]

  return (
    <div className="admin-home">
      <h2 className="admin-title"> Welcome, Admin</h2>
      <p className="admin-subtitle">Manage and monitor key areas of the platform below.</p>

      <div className="admin-grid">
        {cards.map((card) => (
          <div
            key={card.title}
            className="admin-card"
            onClick={() => navigate(card.path)}
          >
            <div className="admin-icon">{card.emoji}</div>
            <h4>{card.title}</h4>
            <p>{card.description}</p>
          </div>
        ))}
      </div>
    </div>
  )
}

export default AdminHomePage
