export interface OrderItem {
  id: string
  productId: string
  productName: string
  quantity: number
  price: number
}

export interface Product {
  id: string
  name: string
  description: string
  price: number
  stockLevel: number
}

export interface Order {
  id: string
  userId: string
  state: 'Pending' | 'Processing' | 'Shipped' | 'Delivered' | 'Cancelled'
  totalAmount: number
  createdAt: string
  items: OrderItem[]
} 