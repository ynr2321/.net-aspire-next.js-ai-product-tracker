import { Suspense } from 'react'
import { ErrorBoundary } from 'react-error-boundary'
import { LoadingFallback } from '@/components/ui/loading-fallback'
import { ErrorFallback } from '@/components/ui/error-fallback'
import { ProductDetails } from '@/components/client/product-details'
import { api } from '@/services/api-client'
import { endpoints } from '@/services/api-endpoints'
import { Product } from '@/types/order'

async function getProductData(id: string) {
  const product = await api.get<Product>(endpoints.products.get(id))
  return { product }
}

export default async function ProductDetailsPage({ params }: { params: { id: string } }) {
  const { product } = await getProductData(params.id)
  
  return (
    <ErrorBoundary FallbackComponent={ErrorFallback}>
      <Suspense fallback={<LoadingFallback />}>
        <ProductDetails product={product} />
      </Suspense>
    </ErrorBoundary>
  )
} 