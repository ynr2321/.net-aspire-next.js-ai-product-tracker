export const config = {
  apiUrl: '/api',  // proxied through Next.js Route Handler
  environment: process.env.NODE_ENV || 'development',
  isDevelopment: process.env.NODE_ENV === 'development',
  isProduction: process.env.NODE_ENV === 'production',
} as const