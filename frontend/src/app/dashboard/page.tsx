import { Suspense } from 'react'
import { ErrorBoundary } from 'react-error-boundary'
import Dashboard from '@/components/client/dashboard'
import { LoadingFallback } from '@/components/ui/loading-fallback'
import { ErrorFallback } from '@/components/ui/error-fallback'
import { api } from '@/services/api-client'
import { endpoints } from '@/services/api-endpoints'
import { Order } from '@/types/order'
import { Product } from '@/types/product'

async function getDashboardData() {
  const [orders, products] = await Promise.all([
    api.get<Order[]>(endpoints.orders.list),
    api.get<Product[]>(endpoints.products.list),
  ])

  return { orders, products }
}

export default async function DashboardPage() {
  const { orders, products } = await getDashboardData()
  
  return (
    <ErrorBoundary FallbackComponent={ErrorFallback}>
      <Suspense fallback={<LoadingFallback />}>
        <Dashboard orders={orders} products={products} />
      </Suspense>
    </ErrorBoundary>
  )
} 