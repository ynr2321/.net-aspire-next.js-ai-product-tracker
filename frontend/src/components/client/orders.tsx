'use client'

import { Grid, Paper, Typography, IconButton, Collapse, Box } from '@mui/material'
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown'
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp'
import { useState } from 'react'

interface Order {
  id: string
  createdAt: string
  userId: string
  totalAmount: number
  state: string
  items: OrderItem[]
}

interface OrderItem {
  id: string
  productId: string
  quantity: number
  unitPrice: number
}

interface Product {
  id: string
  name: string
}

interface OrdersProps {
  orders: Order[]
  products: Product[]
}

function OrderItem({ item, product }: { item: OrderItem; product?: Product }) {
  return (
    <Grid container spacing={1} className="py-1 border-b border-gray-100">
      <Grid item xs={2} className="text-sm">{item.productId}</Grid>
      <Grid item xs={5} className="text-sm">{product?.name || 'Unknown Product'}</Grid>
      <Grid item xs={1} className="text-sm text-center">{item.quantity}</Grid>
      <Grid item xs={2} className="text-sm text-right">${item.unitPrice.toFixed(2)}</Grid>
      <Grid item xs={2} className="text-sm text-right">${(item.quantity * item.unitPrice).toFixed(2)}</Grid>
    </Grid>
  )
}

function OrderRow({ order, products }: { order: Order; products: Product[] }) {
  const [open, setOpen] = useState(false)

  return (
    <Paper className="mb-1 shadow-sm">
      <Grid 
        container 
        spacing={1} 
        className="p-2 cursor-pointer hover:bg-gray-50"
        onClick={() => window.location.href = `/orders/${order.id}`}
      >
        <Grid item xs={1}>
          <IconButton
            size="small"
            onClick={(e) => {
              e.stopPropagation()
              setOpen(!open)
            }}
          >
            {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
          </IconButton>
        </Grid>
        <Grid item xs={2} className="text-sm">{order.id}</Grid>
        <Grid item xs={2} className="text-sm">{new Date(order.createdAt).toLocaleString()}</Grid>
        <Grid item xs={2} className="text-sm">{order.userId}</Grid>
        <Grid item xs={2} className="text-sm">{`${order.userId} - Customer ${order.userId}`}</Grid>
        <Grid item xs={2} className="text-sm text-right">${order.totalAmount.toFixed(2)}</Grid>
        <Grid item xs={1}>
          <span className={`px-1.5 py-0.5 rounded-full text-xs ${
            order.state === 'Delivered' ? 'bg-green-100 text-green-800' :
            order.state === 'Shipped' ? 'bg-blue-100 text-blue-800' :
            order.state === 'Processing' ? 'bg-yellow-100 text-yellow-800' :
            'bg-gray-100 text-gray-800'
          }`}>
            {order.state}
          </span>
        </Grid>
      </Grid>
      
      <Collapse in={open} timeout="auto" unmountOnExit>
        <Box className="p-2 bg-gray-50">
          <Typography variant="subtitle2" className="mb-1.5 font-medium">Products in Order</Typography>
          <Grid container spacing={1} className="mb-1 text-sm font-medium text-gray-600">
            <Grid item xs={2}>Product ID</Grid>
            <Grid item xs={5}>Product Name</Grid>
            <Grid item xs={1} className="text-center">Qty</Grid>
            <Grid item xs={2} className="text-right">Price</Grid>
            <Grid item xs={2} className="text-right">Total</Grid>
          </Grid>
          {order.items?.map((item) => {
            const product = products.find(p => p.id === item.productId)
            return <OrderItem key={item.id} item={item} product={product} />
          })}
        </Box>
      </Collapse>
    </Paper>
  )
}

export function Orders({ orders, products }: OrdersProps) {
  const [page, setPage] = useState(0)
  const [rowsPerPage] = useState(10)

  const handleChangePage = (event: unknown, newPage: number) => {
    setPage(newPage)
  }

  const paginatedOrders = orders
    .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
    .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)

  return (
    <Paper className="p-2">
      <Typography variant="h6" className="mb-2">Recent Orders</Typography>
      <div className="space-y-1" style={{ maxHeight: '400px', overflowY: 'auto' }}>
        <Grid container spacing={1} className="mb-1 text-sm font-medium text-gray-600">
          <Grid item xs={1}></Grid>
          <Grid item xs={2}>Order ID</Grid>
          <Grid item xs={2}>Date & Time</Grid>
          <Grid item xs={2}>Customer ID</Grid>
          <Grid item xs={2}>Customer Name</Grid>
          <Grid item xs={2} className="text-right">Total</Grid>
          <Grid item xs={1}>State</Grid>
        </Grid>
        
        {paginatedOrders.map((order) => (
          <OrderRow key={order.id} order={order} products={products} />
        ))}
      </div>
      
      <div className="mt-2 flex justify-end">
        <div className="flex items-center gap-1.5">
          <button
            onClick={() => handleChangePage(null, page - 1)}
            disabled={page === 0}
            className="px-2 py-0.5 text-sm border rounded hover:bg-gray-50 disabled:opacity-50 disabled:hover:bg-white"
          >
            Previous
          </button>
          <span className="px-2 text-sm">
            Page {page + 1} of {Math.ceil(orders.length / rowsPerPage)}
          </span>
          <button
            onClick={() => handleChangePage(null, page + 1)}
            disabled={page >= Math.ceil(orders.length / rowsPerPage) - 1}
            className="px-2 py-0.5 text-sm border rounded hover:bg-gray-50 disabled:opacity-50 disabled:hover:bg-white"
          >
            Next
          </button>
        </div>
      </div>
    </Paper>
  )
} 