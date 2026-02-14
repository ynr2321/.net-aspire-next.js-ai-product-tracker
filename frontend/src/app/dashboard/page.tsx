import { Suspense } from 'react'
import { ErrorBoundary } from 'react-error-boundary'
import Dashboard from '@/components/client/dashboard'
import { LoadingFallback } from '@/components/ui/loading-fallback'
import { ErrorFallback } from '@/components/ui/error-fallback'
import { api } from '@/services/api-client'
import { endpoints } from '@/services/api-endpoints'
import { Order } from '@/types/order'
import { Product } from '@/types/product'




export default async function DashboardPage() {
  
  return (
    <ErrorBoundary FallbackComponent={ErrorFallback}>
      <Suspense fallback={<LoadingFallback />}>
        <Dashboard/>
      </Suspense>
    </ErrorBoundary>
  )
} 