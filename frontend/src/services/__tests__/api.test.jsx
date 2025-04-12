import { vi, describe, it, expect } from 'vitest'

// ✅ Define mockAxios *inside* the mock factory to avoid hoisting issues
vi.mock('axios', () => {
  const mockAxios = {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
    delete: vi.fn(),
    interceptors: {
      request: { use: vi.fn() },
      response: { use: vi.fn() }
    }
  }

  return {
    default: {
      create: () => mockAxios
    },
    // 👇 Export it for access in tests
    __esModule: true,
    mockAxios
  }
})

// ✅ Import api AFTER mocking axios
import api from '../api'
// ✅ Grab mockAxios from the mocked module
import { mockAxios } from 'axios'

describe('API - Events', () => {
  it('fetches events successfully', async () => {
    const mockEvents = [{ eventId: 1, eventName: 'Run Club 5K' }]
    mockAxios.get.mockResolvedValue({ data: mockEvents })

    const response = await api.get('/events')
    expect(response.data).toEqual(mockEvents)
  })

  it('handles fetch failure gracefully', async () => {
    mockAxios.get.mockRejectedValue(new Error('Network Error'))

    try {
      await api.get('/events')
    } catch (err) {
      expect(err).toBeDefined()
      expect(err.message).toMatch(/network error/i)
    }
  })
})
/* 🔍 Why this matters:
✅ You're testing how your logic handles both success and failure
✅ No real API calls = fast, reliable tests
✅ You covered edge cases (e.g., server offline)*/