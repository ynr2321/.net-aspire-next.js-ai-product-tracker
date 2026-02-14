import { Suspense } from 'react'
import { ErrorBoundary } from 'react-error-boundary'
import { LoadingFallback } from '@/components/ui/loading-fallback'
import { ErrorFallback } from '@/components/ui/error-fallback'
import { OrderDetails } from '@/components/client/order-details'
import { api } from '@/services/api-client'
import { endpoints } from '@/services/api-endpoints'
import { Order, Product } from '@/types/order'

async function getOrderData(id: string) {
  const [order, products] = await Promise.all([
    api.get<Order>(endpoints.orders.get(id)),
    api.get<Product[]>(endpoints.products.list),
  ])

  return { order, products }
}

export default async function OrderDetailsPage({ params }: { params: { id: string } }) {
  const { order, products } = await getOrderData(params.id)
  
  return (
    <ErrorBoundary FallbackComponent={ErrorFallback}>
      <Suspense fallback={<LoadingFallback />}>
        <OrderDetails order={order} products={products} />
      </Suspense>
    </ErrorBoundary>
  )
} 