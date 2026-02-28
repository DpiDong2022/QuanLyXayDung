// middleware/auth.ts
// Bảo vệ tất cả route, chuyển hướng về /login nếu chưa đăng nhập

export default defineNuxtRouteMiddleware((to) => {
  const token = useCookie('auth_token')
  const { user } = useAuth()

  console.log('Middleware chạy:', to.path)

  // Các route không cần đăng nhập
  const publicRoutes = ['/login', '/register']
  if (publicRoutes.includes(to.path)) return

  // Chưa có token → về login
  if (!token.value) {
    return navigateTo('/login')
  }

  // Kiểm tra quyền truy cập route super-admin
  if (to.path.startsWith('/super-admin') && user.value?.role !== 'super_admin') {
    return navigateTo('/dashboard')
  }
})
