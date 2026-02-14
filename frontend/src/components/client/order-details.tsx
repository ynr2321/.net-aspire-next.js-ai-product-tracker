'use client'

import { useEffect, useState } from 'react'
import { 
  Container, 
  Paper, 
  Typography, 
  Grid, 
  Chip, 
  Divider,
  Box,
  Button,
  Card,
  CardContent,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from '@mui/material'
import ArrowBackIcon from '@mui/icons-material/ArrowBack'
import LocalShippingIcon from '@mui/icons-material/LocalShipping'
import PaymentIcon from '@mui/icons-material/Payment'
import PersonIcon from '@mui/icons-material/Person'
import { useRouter } from 'next/navigation'
import { Order, Product } from '@/types/order'

interface OrderDetailsProps {
  order: Order
  products: Product[]
}

function getStateColor(state: string) {
  switch (state) {
    case 'Delivered':
      return 'success'
    case 'Shipped':
      return 'primary'
    case 'Processing':
      return 'warning'
    default:
      return 'default'
  }
}

export function OrderDetails({ order, products }: OrderDetailsProps) {
  const [mounted, setMounted] = useState(false)
  const router = useRouter()

  useEffect(() => {
    setMounted(true)
  }, [])

  if (!mounted) {
    return null
  }

  const orderDate = new Date(order.createdAt).toLocaleString()
  const totalItems = order.items.reduce((sum, item) => sum + item.quantity, 0)

  const handleProductClick = (productId: string) => {
    router.push(`/products/${productId}`)
  }

  return (
    <Container maxWidth="xl" className="p-4">
      <Button
        startIcon={<ArrowBackIcon />}
        onClick={() => router.back()}
        className="mb-4"
      >
        Back to Orders
      </Button>

      <Grid container spacing={3}>
        {/* Order Header */}
        <Grid item xs={12}>
          <Paper className="p-4">
            <Grid container spacing={2} alignItems="center">
              <Grid item xs={12} md={6}>
                <Typography variant="h5" component="h1">
                  Order #{order.id}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Placed on {orderDate}
                </Typography>
              </Grid>
              <Grid item xs={12} md={6} className="flex justify-end">
                <Chip
                  label={order.state}
                  color={getStateColor(order.state)}
                />
              </Grid>
            </Grid>
          </Paper>
        </Grid>

        {/* Order Summary Cards */}
        <Grid item xs={12} md={4}>
          <Card>
            <CardContent>
              <Box className="flex items-center gap-2 mb-2">
                <PersonIcon color="primary" />
                <Typography variant="h6">Customer</Typography>
              </Box>
              <Typography variant="body1">ID: {order.userId}</Typography>
              <Typography variant="body2" color="text.secondary">
                Customer {order.userId}
              </Typography>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={4}>
          <Card>
            <CardContent>
              <Box className="flex items-center gap-2 mb-2">
                <LocalShippingIcon color="primary" />
                <Typography variant="h6">Order Summary</Typography>
              </Box>
              <Typography variant="body1">{totalItems} items</Typography>
              <Typography variant="body2" color="text.secondary">
                {order.items.length} different products
              </Typography>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={4}>
          <Card>
            <CardContent>
              <Box className="flex items-center gap-2 mb-2">
                <PaymentIcon color="primary" />
                <Typography variant="h6">Payment</Typography>
              </Box>
              <Typography variant="h5" color="primary">
                ${order.totalAmount.toFixed(2)}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Total Amount
              </Typography>
            </CardContent>
          </Card>
        </Grid>

        {/* Order Items */}
        <Grid item xs={12}>
          <Paper className="p-4">
            <Typography variant="h6" className="mb-4">Order Items</Typography>
            <TableContainer>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Product</TableCell>
                    <TableCell>Price</TableCell>
                    <TableCell align="center">Quantity</TableCell>
                    <TableCell align="right">Total</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {order.items.map((item) => {
                    const product = products.find(p => p.id === item.productId)
                    return (
                      <TableRow 
                        key={item.id}
                        onClick={() => handleProductClick(item.productId)}
                        sx={{ 
                          cursor: 'pointer',
                          '&:hover': { 
                            backgroundColor: 'action.hover',
                          },
                        }}
                      >
                        <TableCell>
                          <Typography variant="body1">{product?.name || 'Unknown Product'}</Typography>
                          <Typography variant="body2" color="text.secondary">
                            {product?.description || 'No description available'}
                          </Typography>
                        </TableCell>
                        <TableCell>${item.unitPrice.toFixed(2)}</TableCell>
                        <TableCell align="center">{item.quantity}</TableCell>
                        <TableCell align="right">
                          ${(item.quantity * item.unitPrice).toFixed(2)}
                        </TableCell>
                      </TableRow>
                    )
                  })}
                </TableBody>
              </Table>
            </TableContainer>
          </Paper>
        </Grid>
      </Grid>
    </Container>
  )
} 