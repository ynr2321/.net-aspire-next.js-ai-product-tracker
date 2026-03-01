import { NextResponse } from 'next/server'
import type { NextRequest } from 'next/server'

const PUBLIC_PATHS = ['/login', '/register']

const IGNORED_PREFIXES = ['/_next', '/favicon.ico', '/img', '/api']

export function middleware(request: NextRequest) {
  const { pathname } = request.nextUrl

  // Allow static & internal routes
  if (IGNORED_PREFIXES.some((prefix) => pathname.startsWith(prefix))) {
    return NextResponse.next()
  }

  // Allow public auth pages
  if (PUBLIC_PATHS.includes(pathname)) {
    return NextResponse.next()
  }

  // Check for the auth cookie flag
  const hasAuth = request.cookies.get('has_auth')?.value

  if (!hasAuth) {
    const loginUrl = new URL('/login', request.url)
    return NextResponse.redirect(loginUrl)
  }

  return NextResponse.next()
}

export const config = {
  matcher: ['/((?!_next/static|_next/image|favicon.ico).*)'],
}
