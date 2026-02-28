// ============================================
// composables/useApi.ts
// Wrapper gọi ASP.NET API với JWT Token
// ============================================

export const useApi = () => {
  const config = useRuntimeConfig()
  const baseURL = config.public.apiUrl // http://localhost:5000/api

  // Lấy token từ cookie
  const getToken = () => useCookie('auth_token').value

  // GET
  const get = async <T>(endpoint: string) => {
    return await $fetch<T>(`${baseURL}${endpoint}`, {
      headers: {
        Authorization: `Bearer ${getToken()}`
      }
    })
  }

  // POST
  const post = async <T>(endpoint: string, body: unknown) => {
    return await $fetch<T>(`${baseURL}${endpoint}`, {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${getToken()}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(body)
    })
  }

  // PUT
  const put = async <T>(endpoint: string, body: unknown) => {
    return await $fetch<T>(`${baseURL}${endpoint}`, {
      method: 'PUT',
      headers: {
        'Authorization': `Bearer ${getToken()}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(body)
    })
  }

  // DELETE
  const del = async <T>(endpoint: string) => {
    return await $fetch<T>(`${baseURL}${endpoint}`, {
      method: 'DELETE',
      headers: {
        Authorization: `Bearer ${getToken()}`
      }
    })
  }

  return { get, post, put, del }
}

// ============================================
// composables/useAuth.ts
// Xử lý login / logout / user hiện tại
// ============================================

interface LoginResponse {
  token: string
  hoTen: string
  email: string
  role: string
  congTyId: string
  tenCongTy: string
  message: string
}

interface UserState {
  hoTen: string
  email: string
  role: string
  congTyId: string
  tenCongTy: string
}

export const useAuth = () => {
  const token = useCookie('auth_token', {
    maxAge: 60 * 60 * 8, // 8 giờ
    secure: true,
    sameSite: 'lax'
  })
  const user = useState<UserState | null>('auth_user', () => null)
  const loading = ref(false)
  const error = ref('')

  const config = useRuntimeConfig()
  const apiUrl = config.public.apiUrl

  // Đăng nhập
  const login = async (email: string, password: string) => {
    loading.value = true
    error.value = ''
    try {
      const res = await $fetch<LoginResponse>(`${apiUrl}/auth/login`, {
        method: 'POST',
        body: { email, password }
      })

      // Lưu token vào cookie
      token.value = res.token

      // Lưu thông tin user vào state
      user.value = {
        hoTen: res.hoTen,
        email: res.email,
        role: res.role,
        congTyId: res.congTyId,
        tenCongTy: res.tenCongTy
      }

      // Điều hướng theo role
      const router = useRouter()
      if (res.role.includes('SuperAdmin')) {
        await router.push('/')
        // await router.push('/super-admin/cong-ty')
      } else {
        await router.push('/')
      }
    } catch (err: any) {
      console.error('Login error:', err)

      error.value = err?.data?.message || err?.statusMessage || 'Đăng nhập thất bại'
    } finally {
      loading.value = false
    }
  }

  // Đăng xuất
  const logout = async () => {
    token.value = null
    user.value = null
    await navigateTo('/login')
  }

  // Kiểm tra quyền
  const isSuperAdmin = computed(() => user.value?.role === 'SuperAdmin')
  const isAdmin = computed(() => ['SuperAdmin', 'admin'].includes(user.value?.role ?? ''))

  // Lấy thông tin user hiện tại từ API
  const fetchMe = async () => {
    if (!token.value) return
    try {
      const config = useRuntimeConfig()
      const res = await $fetch<UserState>(`${config.public.apiUrl}/auth/me`, {
        headers: { Authorization: `Bearer ${token.value}` }
      })
      user.value = res
    } catch {
      token.value = null
      user.value = null
    }
  }

  return {
    token,
    user,
    loading,
    error,
    login,
    logout,
    fetchMe,
    isSuperAdmin,
    isAdmin
  }
}
