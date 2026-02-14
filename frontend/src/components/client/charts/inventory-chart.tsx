'use client'

import { Paper, Typography } from '@mui/material'
import { PieChart } from '@mui/x-charts'

interface InventoryChartProps {
  products: any[]
}

export function InventoryChart({ products }: InventoryChartProps) {
  const inventoryData = products.map((product: any) => ({
    product: product.name,
    stock: product.stockLevel
  }))

  return (
    <Paper className="p-4 h-full">
      <Typography variant="h6" className="mb-4">Product Inventory Levels</Typography>
      <div className="h-[300px]">
        <PieChart
          series={[
            {
              data: inventoryData.map(item => ({
                id: item.product,
                value: item.stock,
                label: item.product
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