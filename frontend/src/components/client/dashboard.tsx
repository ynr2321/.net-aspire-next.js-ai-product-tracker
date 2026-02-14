'use client'

import { Container, Grid, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import { Orders } from './orders'
import { OrderStateChart } from './charts/order-state-chart'
import { OrderVolumeChart } from './charts/order-volume-chart'
import { InventoryChart } from './charts/inventory-chart'

interface DashboardProps {
  orders: any[]
  products: any[]
}

export default function Dashboard({ orders, products }: DashboardProps) {
  const [mounted, setMounted] = useState(false)

  useEffect(() => {
    setMounted(true)
  }, [])

  if (!mounted) {
    return null
  }

  return (
    <Container maxWidth="xl" className="p-4">
      <Typography variant="h4" component="h1" className="mb-6">
        Admin Dashboard
      </Typography>
      
      <Grid container spacing={4}>
        {/* Top Row: Charts */}
        <Grid container item spacing={4}>
          {/* Order State Distribution */}
          <Grid item xs={12} md={4}>
            <OrderStateChart orders={orders} />
          </Grid>

          {/* Order Volume */}
          <Grid item xs={12} md={4}>
            <OrderVolumeChart orders={orders} />
          </Grid>

          {/* Inventory Levels */}
          <Grid item xs={12} md={4}>
            <InventoryChart products={products} />
          </Grid>
        </Grid>

        {/* Bottom Row: Order History */}
        <Grid item xs={12}>
          <Orders orders={orders} products={products} />
        </Grid>
      </Grid>
    </Container>
  )
} 