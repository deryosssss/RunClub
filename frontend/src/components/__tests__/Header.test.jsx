import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { BrowserRouter } from 'react-router-dom'
import Header from '../Header'
import { AppContext } from '../../context/AppContext'
import { vi } from 'vitest'

describe('Header component', () => {
  test('displays user role and logout buttons', () => {
    const mockUser = { role: 'runner', name: 'Test User' }

    render(
      <AppContext.Provider value={{ user: mockUser, logout: vi.fn() }}>
        <BrowserRouter>
          <Header />
        </BrowserRouter>
      </AppContext.Provider>
    )

    // Logo & role
    expect(screen.getByText(/Momentum/i)).toBeInTheDocument()
    expect(screen.getByText((t) => t.includes('| RUNNER'))).toBeInTheDocument()

    // Logout buttons (both desktop & mobile)
    const logoutButtons = screen.getAllByText(/logout/i)
    expect(logoutButtons.length).toBeGreaterThanOrEqual(1)
  })

  test('calls logout function on logout click', async () => {
    const user = userEvent.setup()
    const mockLogout = vi.fn()
    const mockUser = { role: 'runner', name: 'Test User' }

    render(
      <AppContext.Provider value={{ user: mockUser, logout: mockLogout }}>
        <BrowserRouter>
          <Header />
        </BrowserRouter>
      </AppContext.Provider>
    )

    const logoutButtons = screen.getAllByText(/logout/i)

    // Click the first visible logout button
    await user.click(logoutButtons[0])

    expect(mockLogout).toHaveBeenCalledTimes(1)
  })
})

/*  Why it matters:
This confirms your component renders what you expect when a runner is logged in. If it ever breaks in future updates, this test will fail — alerting you before your users notice.

 Why this matters:
You isolated the test. Instead of calling a real backend or full app, you mocked just what was needed.*/

//  Header component test checks:

// Whether the user’s role appears

// Whether the logout button shows

// Whether clicking logout triggers the function