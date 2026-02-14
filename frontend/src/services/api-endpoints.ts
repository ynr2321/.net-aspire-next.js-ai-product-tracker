export const endpoints = {
  orders: {
    list: '/orders',
    get: (id: string) => `/orders/${id}`,
    create: '/orders',
    update: (id: string) => `/orders/${id}`,
    delete: (id: string) => `/orders/${id}`,
  },
  products: {
    list: '/products',
    get: (id: string) => `/products/${id}`,
    create: '/products',
    update: (id: string) => `/products/${id}`,
    delete: (id: string) => `/products/${id}`,
  },
  users: {
    list: '/users',
    get: (id: string) => `/users/${id}`,
    create: '/users',
    update: (id: string) => `/users/${id}`,
    delete: (id: string) => `/users/${id}`,
  },
} as const 