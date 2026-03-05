'use client'

import { atom, useAtom } from 'jotai'
import { useCallback, useEffect } from 'react'
import { useRouter } from 'next/navigation'
import { api, ApiError } from './api-client'

export interface AuthUser {
  email: string
  roles: string[]
}

// --- Jotai atom ---
const authAtom = atom<AuthUser | null>(null)

// --- Hook ---
export function useAuth() {
  const [user, setUser] = useAtom(authAtom)
  const router = useRouter()

  // Hydrate from localStorage on mount
  useEffect(() => {
    if (user) return
    try {
      const raw = localStorage.getItem('auth')
      if (raw) {
        const parsed: AuthUser = JSON.parse(raw)
        if (parsed?.email) {
          setUser(parsed)
        }
      }
    } catch {
      // corrupt data – clear it
      localStorage.removeItem('auth')
    }
  }, []) // eslint-disable-line react-hooks/exhaustive-deps

  const login = useCallback(
    async (email: string, password: string) => {
      const data = await api.post<AuthUser>('/Auth/login', { email, password })
      localStorage.setItem('auth', JSON.stringify(data))
      setUser(data)
      router.push('/')
    },
    [setUser, router],
  )

  const logout = useCallback(async () => {
    await fetch('/api/auth/logout', { method: 'POST' })
    localStorage.removeItem('auth')
    setUser(null)
    router.push('/login')
  }, [setUser, router])

  return {
    user,
    login,
    logout,
    isAuthenticated: !!user,
  }
}
