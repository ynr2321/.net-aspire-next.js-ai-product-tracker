'use client'

export function ErrorFallback({ error }: { error: Error }) {
  return (
    <div className="p-4 bg-red-50 text-red-700 rounded">
      <h2 className="text-lg font-semibold mb-2">Something went wrong:</h2>
      <pre className="mt-2 text-sm">{error.message}</pre>
    </div>
  )
} 