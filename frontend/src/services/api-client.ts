import { config } from './config'

const API_URL = config.apiUrl

export const api = {
  async get<T>(endpoint: string): Promise<T> {
    const url = `${API_URL}${endpoint}`
    console.log(`[API] GET ${url}`)
    const response = await fetch(url)
    if (!response.ok) {
      throw new Error(`API error: ${response.statusText}`)
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
      },
      body: JSON.stringify(data),
    })
    if (!response.ok) {
      throw new Error(`API error: ${response.statusText}`)
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
      },
      body: JSON.stringify(data),
    })
    if (!response.ok) {
      throw new Error(`API error: ${response.statusText}`)
    }
    return response.json()
  },

  async delete<T>(endpoint: string): Promise<T> {
    const url = `${API_URL}${endpoint}`
    console.log(`[API] DELETE ${url}`)
    const response = await fetch(url, {
      method: 'DELETE',
    })
    if (!response.ok) {
      throw new Error(`API error: ${response.statusText}`)
    }
    return response.json()
  },
} 