import { Navbar } from "@/components/ui/Navbar"
import { Footer } from "@/components/ui/Footer"
import { PopupWidget } from "@/components/ui/PopupWidget"

export default function MainLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <>
      <Navbar />
      <div>{children}</div>
      <Footer />
      <PopupWidget />
    </>
  )
}
