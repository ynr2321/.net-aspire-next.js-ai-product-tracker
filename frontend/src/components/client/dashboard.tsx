'use client'

interface DashboardProps{
  apiStatus: string
}

export default function Dashboard(props: DashboardProps) {

  return (
    <div>
      <h1>Placeholder</h1>

      <h1>{`API Status: ${props.apiStatus}`}</h1>
    </div>
  )
} 