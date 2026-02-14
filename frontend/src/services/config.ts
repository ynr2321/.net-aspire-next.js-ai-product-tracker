export const config = {
  apiUrl: process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5451/api',
  environment: process.env.NODE_ENV || 'development',
  isDevelopment: process.env.NODE_ENV === 'development',
  isProduction: process.env.NODE_ENV === 'production',
} as const

// Debug log to see what URL is being used
//console.log('Config loaded with API URL:', config.apiUrl) 