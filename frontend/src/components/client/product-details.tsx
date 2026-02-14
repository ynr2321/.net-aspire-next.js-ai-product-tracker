'use client'

import { useEffect, useState } from 'react'
import { 
  Container, 
  Paper, 
  Typography, 
  Grid, 
  Chip,
  Box,
  Button,
  Card,
  CardContent,
  Divider,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
} from '@mui/material'
import ArrowBackIcon from '@mui/icons-material/ArrowBack'
import InventoryIcon from '@mui/icons-material/Inventory'
import AttachMoneyIcon from '@mui/icons-material/AttachMoney'
import DescriptionIcon from '@mui/icons-material/Description'
import { useRouter } from 'next/navigation'
import { Product } from '@/types/order'

interface ProductDetailsProps {
  product: Product
}

export function ProductDetails({ product }: ProductDetailsProps) {
  const [mounted, setMounted] = useState(false)
  const router = useRouter()

  useEffect(() => {
    setMounted(true)
  }, [])

  if (!mounted) {
    return null
  }

  const getStockStatus = (stockLevel: number) => {
    if (stockLevel <= 0) return { label: 'Out of Stock', color: 'error' as const }
    if (stockLevel < 10) return { label: 'Low Stock', color: 'warning' as const }
    return { label: 'In Stock', color: 'success' as const }
  }

  const stockStatus = getStockStatus(product.stockLevel)

  return (
    <Container maxWidth="xl" className="p-4">
      <Button
        startIcon={<ArrowBackIcon />}
        onClick={() => router.back()}
        className="mb-4"
      >
        Back to Products
      </Button>

      <Grid container spacing={3}>
        {/* Product Header */}
        <Grid item xs={12}>
          <Paper className="p-4">
            <Grid container spacing={2} alignItems="center">
              <Grid item xs={12} md={6}>
                <Typography variant="h5" component="h1">
                  {product.name}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Product ID: {product.id}
                </Typography>
              </Grid>
              <Grid item xs={12} md={6} className="flex justify-end">
                <Chip
                  label={stockStatus.label}
                  color={stockStatus.color}
                  size="large"
                />
              </Grid>
            </Grid>
          </Paper>
        </Grid>

        {/* Product Details */}
        <Grid item xs={12} md={8}>
          <Paper className="p-4">
            <Typography variant="h6" className="mb-4">Product Details</Typography>
            <List>
              <ListItem>
                <ListItemIcon>
                  <DescriptionIcon />
                </ListItemIcon>
                <ListItemText 
                  primary="Description" 
                  secondary={product.description}
                  secondaryTypographyProps={{ sx: { whiteSpace: 'pre-wrap' } }}
                />
              </ListItem>
              <Divider />
              <ListItem>
                <ListItemIcon>
                  <AttachMoneyIcon />
                </ListItemIcon>
                <ListItemText 
                  primary="Price" 
                  secondary={`$${product.price.toFixed(2)}`}
                />
              </ListItem>
              <Divider />
              <ListItem>
                <ListItemIcon>
                  <InventoryIcon />
                </ListItemIcon>
                <ListItemText 
                  primary="Stock Level" 
                  secondary={`${product.stockLevel} units available`}
                />
              </ListItem>
            </List>
          </Paper>
        </Grid>

        {/* Product Summary Card */}
        <Grid item xs={12} md={4}>
          <Card>
            <CardContent>
              <Typography variant="h6" className="mb-4">Product Summary</Typography>
              <Box className="space-y-4">
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Current Price
                  </Typography>
                  <Typography variant="h4" color="primary">
                    ${product.price.toFixed(2)}
                  </Typography>
                </Box>
                <Divider />
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Stock Status
                  </Typography>
                  <Typography variant="h6" color={stockStatus.color}>
                    {stockStatus.label}
                  </Typography>
                </Box>
                <Divider />
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Available Units
                  </Typography>
                  <Typography variant="h6">
                    {product.stockLevel}
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Container>
  )
} 