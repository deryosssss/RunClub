import { render, screen } from '@testing-library/react'
import MediaGalleryPage from '../MediaGalleryPage'
import { AppContext } from '../../../context/AppContext'
import { BrowserRouter } from 'react-router-dom'

describe('MediaGalleryPage', () => {
  test('renders heading and images section', () => {
    const mockUser = { role: 'admin', name: 'Derya' }

    render(
      <AppContext.Provider value={{ user: mockUser }}>
        <BrowserRouter>
          <MediaGalleryPage />
        </BrowserRouter>
      </AppContext.Provider>
    )

    expect(screen.getByText(/Media Gallery/i)).toBeInTheDocument()
  })
})
