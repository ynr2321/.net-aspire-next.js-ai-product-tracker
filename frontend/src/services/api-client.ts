import { config } from './config'

const API_URL = config.apiUrl

function getAuthHeaders(): Record<string, string> {
  if (typeof window === 'undefined') return {}
  try {
    const raw = localStorage.getItem('auth')
    if (raw) {
      const auth = JSON.parse(raw)
      if (auth?.token) {
        return { Authorization: `Bearer ${auth.token}` }
      }
    }
  } catch {
    // ignore parse errors
  }
  return {}
}

function handleUnauthorized(response: Response): void {
  if (response.status === 401 && typeof window !== 'undefined') {
    localStorage.removeItem('auth')
    document.cookie = 'has_auth=; path=/; max-age=0'
    window.location.href = '/login'
  }
}

export class ApiError extends Error {
  status: number
  data: any

  constructor(status: number, data: any) {
    super(data?.message || `API error: ${status}`)
    this.status = status
    this.data = data
  }
}

export const api = {
  async get<T>(endpoint: string): Promise<T> {
    const url = `${API_URL}${endpoint}`
    console.log(`[API] GET ${url}`)
    const response = await fetch(url, {
      headers: {
        ...getAuthHeaders(),
      },
    })
    if (!response.ok) {
      handleUnauthorized(response)
      const data = await response.json().catch(() => ({}))
      throw new ApiError(response.status, data)
    }
    return response.json()
  },

  async post<T>(endpoint: string, data: any): Promise<T> {
    const url = `${API_URL}${endpoint}`
    console.log(`[API] POST ${url}`, data)
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        ...getAuthHeaders(),
      },
      body: JSON.stringify(data),
    })
    if (!response.ok) {
      handleUnauthorized(response)
      const errorData = await response.json().catch(() => ({}))
      throw new ApiError(response.status, errorData)
    }
    return response.json()
  },

  async put<T>(endpoint: string, data: any): Promise<T> {
    const url = `${API_URL}${endpoint}`
    console.log(`[API] PUT ${url}`, data)
    const response = await fetch(url, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        ...getAuthHeaders(),
      },
      body: JSON.stringify(data),
    })
    if (!response.ok) {
      handleUnauthorized(response)
      const errorData = await response.json().catch(() => ({}))
      throw new ApiError(response.status, errorData)
    }
    return response.json()
  },

  async delete<T>(endpoint: string): Promise<T> {
    const url = `${API_URL}${endpoint}`
    console.log(`[API] DELETE ${url}`)
    const response = await fetch(url, {
      method: 'DELETE',
      headers: {
        ...getAuthHeaders(),
      },
    })
    if (!response.ok) {
      handleUnauthorized(response)
      const errorData = await response.json().catch(() => ({}))
      throw new ApiError(response.status, errorData)
    }
    return response.json()
  },
} 