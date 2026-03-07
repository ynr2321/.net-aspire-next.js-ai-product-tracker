import { NextResponse } from 'next/server'

export async function POST(): Promise<NextResponse> {
  const response = NextResponse.json({ success: true }, { status: 200 })

  response.headers.append(
    'Set-Cookie',
    'auth_token=; HttpOnly; SameSite=Lax; Path=/; Max-Age=0',
  )

  return response
}
