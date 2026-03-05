import { NextRequest, NextResponse } from 'next/server'

function getUpstreamUrl(): string {
  return (
    process.env.services__apiservice__https__0 ||
    process.env.services__apiservice__http__0 ||
    'http://localhost:5451'
  )
}

async function proxyRequest(
  req: NextRequest,
  { params }: { params: Promise<{ path: string[] }> },
): Promise<NextResponse> {
  const { path } = await params
  const upstream = getUpstreamUrl()
  const target = `${upstream}/api/${path.join('/')}`

  const url = new URL(target)
  req.nextUrl.searchParams.forEach((value, key) => {
    url.searchParams.append(key, value)
  })

  const headers = new Headers(req.headers)
  headers.delete('host')

  // Inject Authorization header from httpOnly cookie
  const authToken = req.cookies.get('auth_token')?.value
  if (authToken) {
    headers.set('Authorization', `Bearer ${authToken}`)
  }

  const init: RequestInit = {
    method: req.method,
    headers,
  }

  if (req.method !== 'GET' && req.method !== 'HEAD') {
    init.body = await req.arrayBuffer()
  }

  const upstream_response = await fetch(url.toString(), init)

  const responseHeaders = new Headers(upstream_response.headers)
  responseHeaders.delete('transfer-encoding')

  // Intercept login response to set httpOnly cookie and strip token
  const isLoginRoute = path.join('/').toLowerCase() === 'auth/login'
  if (isLoginRoute && upstream_response.status === 200) {
    const body = await upstream_response.json()
    const { token, email, roles } = body as { token: string; email: string; roles: string[] }

    const secure = process.env.NODE_ENV === 'production' ? '; Secure' : ''
    responseHeaders.append(
      'Set-Cookie',
      `auth_token=${token}; HttpOnly${secure}; SameSite=Lax; Path=/; Max-Age=86400`,
    )

    return NextResponse.json(
      { email, roles },
      { status: 200, headers: responseHeaders },
    )
  }

  return new NextResponse(upstream_response.body, {
    status: upstream_response.status,
    statusText: upstream_response.statusText,
    headers: responseHeaders,
  })
}

export const GET = proxyRequest
export const POST = proxyRequest
export const PUT = proxyRequest
export const DELETE = proxyRequest
export const PATCH = proxyRequest
