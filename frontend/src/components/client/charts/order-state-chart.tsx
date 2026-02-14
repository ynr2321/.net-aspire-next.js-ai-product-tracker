'use client'

import { Paper, Typography } from '@mui/material'
import { PieChart } from '@mui/x-charts'

interface OrderStateChartProps {
  orders: any[]
}

export function OrderStateChart({ orders }: OrderStateChartProps) {
  const orderStateData = orders.reduce((acc: any[], order: any) => {
    const state = order.state || 'Pending'
    const existing = acc.find(item => item.state === state)
    if (existing) {
      existing.count++
    } else {
      acc.push({ state, count: 1 })
    }
    return acc
  }, [])

  return (
    <Paper className="p-4 h-full">
      <Typography variant="h6" className="mb-4">Order Distribution by State</Typography>
      <div className="h-[300px]">
        <PieChart
          series={[
            {
              data: orderStateData.map(item => ({
                id: item.state,
                value: item.count,
                label: item.state
              })),
              highlightScope: { fade: 'global', highlighted: 'item' },
              fade: { innerRadius: 30, additionalRadius: -30 },
            },
          ]}
          height={300}
        />
      </div>
    </Paper>
  )
} 