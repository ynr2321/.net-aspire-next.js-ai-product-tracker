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
