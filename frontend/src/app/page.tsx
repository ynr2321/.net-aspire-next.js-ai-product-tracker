import Link from 'next/link'

export default function Home() {
  return (
    <main className="min-h-screen flex flex-col items-center justify-center p-4">
      <h1 className="text-4xl font-bold mb-6">Welcome to Admin Portal</h1>
      <p className="text-lg text-gray-600 mb-8">Manage your products, orders, and users in one place</p>
      <Link 
        href="/dashboard" 
        className="px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
      >
        Go to Dashboard
      </Link>
    </main>
  )
} 