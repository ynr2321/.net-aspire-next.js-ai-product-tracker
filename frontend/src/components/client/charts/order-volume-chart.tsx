'use client'

import { Paper, Typography } from '@mui/material'
import { LineChart } from '@mui/x-charts'

interface OrderVolumeChartProps {
  orders: any[]
}

export function OrderVolumeChart({ orders }: OrderVolumeChartProps) {
  const orderVolumeData = Array.from({ length: 30 }, (_, i) => {
    const date = new Date()
    date.setDate(date.getDate() - (29 - i))
    const dateStr = date.toISOString().split('T')[0]
    const count = orders.filter((order: any) => 
      new Date(order.createdAt).toISOString().split('T')[0] === dateStr
    ).length
    return { date: dateStr, orders: count }
  })

  return (
    <Paper className="p-4 h-full">
      <Typography variant="h6" className="mb-4">Order Volume (Last 30 Days)</Typography>
      <div className="h-[300px]">
        <LineChart
          xAxis={[{ 
            data: orderVolumeData.map(d => d.date),
            scaleType: 'band',
          }]}
          series={[
            {
              data: orderVolumeData.map(d => d.orders),
              area: true,
            },
          ]}
          height={300}
        />
      </div>
    </Paper>
  )
} 