import { Suspense } from 'react'
import { ErrorBoundary } from 'react-error-boundary'
import Dashboard from '@/components/client/dashboard'
import { LoadingFallback } from '@/components/ui/loading-fallback'
import { ErrorFallback } from '@/components/ui/error-fallback'
import { endpoints } from '@/services/api-endpoints'
import { api } from '@/services/api-client'

// Prevent static prerendering — this page requires a live API call
export const dynamic = 'force-dynamic'

export default async function DashboardPage() {

  const statusResponse: any = await api.get(endpoints.healthCheck.get())
  
  return (
    <ErrorBoundary FallbackComponent={ErrorFallback}>
      <Suspense fallback={<LoadingFallback />}>
        <Dashboard apiStatus={statusResponse.status}/>
      </Suspense>
    </ErrorBoundary>
  )
}