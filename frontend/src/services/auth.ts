'use client'

import { atom, useAtom } from 'jotai'
import { useCallback, useEffect } from 'react'
import { useRouter } from 'next/navigation'
import { api, ApiError } from './api-client'

export interface AuthUser {
  token: string
  email: string
  roles: string[]
}

// --- Jotai atom ---
const authAtom = atom<AuthUser | null>(null)

// --- Cookie helpers ---
function setAuthCookie() {
  document.cookie = 'has_auth=true; path=/; max-age=86400; SameSite=Lax'
}

function clearAuthCookie() {
  document.cookie = 'has_auth=; path=/; max-age=0'
}

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
        if (parsed?.token) {
          setUser(parsed)
          setAuthCookie()
        }
      }
    } catch {
      // corrupt data – clear it
      localStorage.removeItem('auth')
      clearAuthCookie()
    }
  }, []) // eslint-disable-line react-hooks/exhaustive-deps

  const login = useCallback(
    async (email: string, password: string) => {
      const data = await api.post<AuthUser>('/Auth/login', { email, password })
      localStorage.setItem('auth', JSON.stringify(data))
      setAuthCookie()
      setUser(data)
      router.push('/')
    },
    [setUser, router],
  )

  const logout = useCallback(() => {
    localStorage.removeItem('auth')
    clearAuthCookie()
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
