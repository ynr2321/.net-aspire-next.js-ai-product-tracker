'use client'

import { Suspense, useState, useEffect } from 'react'
import { useForm } from 'react-hook-form'
import { useSearchParams } from 'next/navigation'
import Link from 'next/link'
import { useAuth } from '@/services/auth'
import { ApiError } from '@/services/api-client'
import { EnvelopeIcon, LockClosedIcon } from '@heroicons/react/24/outline'

interface LoginForm {
  email: string
  password: string
}

function LoginForm() {
  const { login } = useAuth()
  const searchParams = useSearchParams()
  const [apiError, setApiError] = useState('')
  const [showSuccess, setShowSuccess] = useState(false)

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<LoginForm>()

  useEffect(() => {
    if (searchParams.get('registered') === 'true') {
      setShowSuccess(true)
    }
  }, [searchParams])

  const onSubmit = async (data: LoginForm) => {
    setApiError('')
    try {
      await login(data.email, data.password)
    } catch (err) {
      if (err instanceof ApiError) {
        setApiError(err.data?.message || 'Login failed. Please try again.')
      } else {
        setApiError('An unexpected error occurred.')
      }
    }
  }

  return (
    <div className="w-full max-w-md px-4">
      <div className="rounded-2xl bg-white px-8 py-10 shadow-xl ring-1 ring-gray-900/5">
        {/* Header */}
        <div className="mb-8 text-center">
          <h1 className="text-3xl font-bold tracking-tight text-gray-900">
            Welcome back
          </h1>
          <p className="mt-2 text-sm text-gray-500">
            Sign in to your account to continue
          </p>
        </div>

        {/* Success banner */}
        {showSuccess && (
          <div className="mb-6 rounded-lg bg-green-50 p-4 text-sm text-green-700 ring-1 ring-green-600/20">
            Account created! Please log in.
          </div>
        )}

        {/* API error banner */}
        {apiError && (
          <div className="mb-6 rounded-lg bg-red-50 p-4 text-sm text-red-700 ring-1 ring-red-600/20">
            {apiError}
          </div>
        )}

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
          {/* Email */}
          <div>
            <label
              htmlFor="email"
              className="mb-1.5 block text-sm font-medium text-gray-700"
            >
              Email address
            </label>
            <div className="relative">
              <div className="pointer-events-none absolute inset-y-0 left-0 flex items-center pl-3">
                <EnvelopeIcon className="h-5 w-5 text-gray-400" />
              </div>
              <input
                id="email"
                type="email"
                placeholder="you@example.com"
                className={`block w-full rounded-lg border py-2.5 pl-10 pr-3 text-sm 
                text-gray-900 placeholder-gray-400 bg-white dark:bg-gray-800 dark:text-gray-100 dark:placeholder-gray-400
                focus:outline-none focus:ring-2 ${
                    errors.email
                    ? 'border-red-300 focus:ring-red-500'
                    : 'border-gray-300 focus:ring-indigo-500'
                }`}
                {...register('email', {
                  required: 'Email is required',
                  pattern: {
                    value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                    message: 'Please enter a valid email address',
                  },
                })}
              />
            </div>
            {errors.email && (
              <p className="mt-1.5 text-sm text-red-600">
                {errors.email.message}
              </p>
            )}
          </div>

          {/* Password */}
          <div>
            <label
              htmlFor="password"
              className="mb-1.5 block text-sm font-medium text-gray-700"
            >
              Password
            </label>
            <div className="relative">
              <div className="pointer-events-none absolute inset-y-0 left-0 flex items-center pl-3">
                <LockClosedIcon className="h-5 w-5 text-gray-400" />
              </div>
              <input
                id="password"
                type="password"
                autoComplete="current-password"
                placeholder="••••••••"
                className={`block w-full rounded-lg border py-2.5 pl-10 pr-3 text-sm text-gray-900 placeholder-gray-400 focus:outline-none focus:ring-2 ${
                  errors.password
                    ? 'border-red-300 focus:ring-red-500'
                    : 'border-gray-300 focus:ring-indigo-500'
                }`}
                {...register('password', {
                  required: 'Password is required',
                  minLength: {
                    value: 8,
                    message: 'Password must be at least 8 characters',
                  },
                })}
              />
            </div>
            {errors.password && (
              <p className="mt-1.5 text-sm text-red-600">
                {errors.password.message}
              </p>
            )}
          </div>

          {/* Submit */}
          <button
            type="submit"
            disabled={isSubmitting}
            className="flex w-full items-center justify-center rounded-lg bg-indigo-600 px-4 py-2.5 text-sm font-semibold text-white shadow-sm transition-colors hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 disabled:cursor-not-allowed disabled:opacity-50"
          >
            {isSubmitting ? 'Logging in...' : 'Sign in'}
          </button>
        </form>

        {/* Footer link */}
        <p className="mt-6 text-center text-sm text-gray-500">
          Don&apos;t have an account?{' '}
          <Link
            href="/register"
            className="font-semibold text-indigo-600 hover:text-indigo-500"
          >
            Register
          </Link>
        </p>
      </div>
    </div>
  )
}

export default function LoginPage() {
  return (
    <Suspense>
      <LoginForm />
    </Suspense>
  )
}
