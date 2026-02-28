<template>
  <div class="login-page">
    <div class="login-card">
      <!-- Logo -->
      <div class="login-header">
        <div class="login-logo">🏗️</div>
        <h1 class="login-title">Quản Lý Công Trình</h1>
        <p class="login-sub">Đăng nhập vào hệ thống</p>
      </div>

      <!-- Form -->
      <UForm class="login-form" @submit.prevent="handleLogin">
        <UFormField label="Email" :error="loi.email">
          <UInput
            v-model="email"
            type="email"
            placeholder="you@congty.vn"
            icon="i-heroicons-envelope"
            size="lg"
            :disabled="loading"
          />
        </UFormField>

        <UFormField label="Mật khẩu" :error="loi.password">
          <UInput
            v-model="password"
            :type="hienMatKhau ? 'text' : 'password'"
            :ui="{ trailing: 'pe-1' }"
          >
            <template #trailing>
              <UButton
                color="neutral"
                variant="link"
                size="sm"
                :icon="hienMatKhau ? 'i-heroicons-eye-slash' : 'i-heroicons-eye'"
                :aria-label="hienMatKhau ? 'Hide password' : 'Show password'"
                :aria-pressed="hienMatKhau"
                aria-controls="password"
                @click="hienMatKhau = !hienMatKhau"
              >
              </UButton>
            </template>
          </UInput>
        </UFormField>

        <UAlert v-if="error" :description="error" color="error" icon="i-heroicons-exclamation-circle"/>

        <UButton
          type="submit"
          color="primary"
          size="lg"
          block
          :loading="loading"
          icon="i-heroicons-arrow-right-end-on-rectangle"
        >
          Đăng nhập
        </UButton>
      </UForm>

      <!-- Tài khoản mẫu (xóa khi production) -->
      <div class="demo-accounts">
        <p class="demo-title">Tài khoản thử nghiệm:</p>
        <div class="demo-list">
          <button class="demo-item" @click="dienMau('admin@gmail.com', '123')">
            <NuxtBadge color="blue" size="xs">Admin</NuxtBadge>
            <span>admin@gmail.com</span>
          </button>
          <button class="demo-item" @click="dienMau('staff@xdmn.vn', 'admin123')">
            <NuxtBadge color="gray" size="xs">Staff</NuxtBadge>
            <span>staff@xdmn.vn</span>
          </button>
        </div>
      </div>
    </div>

    <!-- Background decoration -->
    <div class="bg-decoration">
      <div class="bg-circle bg-circle-1" />
      <div class="bg-circle bg-circle-2" />
      <div class="bg-circle bg-circle-3" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAuth } from '~/composables/useAuth'

definePageMeta({ layout: 'login' }) // Không dùng layout mặc định

const { login, loading, error } = useAuth()

const email = ref('')
const password = ref('')
const hienMatKhau = ref(false)
const loi = ref({ email: '', password: '' })

function validate() {
  loi.value = { email: '', password: '' }
  if (!email.value) loi.value.email = 'Vui lòng nhập email'
  if (!password.value) loi.value.password = 'Vui lòng nhập mật khẩu'
  return !loi.value.email && !loi.value.password
}

async function handleLogin() {
  if (!validate()) return
  await login(email.value, password.value)
}

function dienMau(e: string, p: string) {
  email.value = e
  password.value = p
}
</script>

<style scoped>
.login-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #0f172a 0%, #1e293b 50%, #0f172a 100%);
  position: relative;
  overflow: hidden;
}

.login-card {
  background: white;
  border-radius: 1.25rem;
  padding: 2.5rem;
  width: 100%;
  max-width: 420px;
  box-shadow: 0 25px 50px rgba(0, 0, 0, 0.4);
  position: relative;
  z-index: 10;
}

.login-header {
  text-align: center;
  margin-bottom: 2rem;
}

.login-logo {
  font-size: 3rem;
  margin-bottom: 0.75rem;
}

.login-title {
  font-size: 1.5rem;
  font-weight: 800;
  color: #0f172a;
  margin-bottom: 0.25rem;
}

.login-sub {
  color: #64748b;
  font-size: 0.875rem;
}

.login-form {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

/* Demo accounts */
.demo-accounts {
  margin-top: 1.5rem;
  padding-top: 1.25rem;
  border-top: 1px dashed #e2e8f0;
}

.demo-title {
  font-size: 0.75rem;
  color: #94a3b8;
  margin-bottom: 0.5rem;
  text-align: center;
}

.demo-list {
  display: flex;
  flex-direction: column;
  gap: 0.375rem;
}

.demo-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.375rem 0.625rem;
  border-radius: 0.5rem;
  font-size: 0.75rem;
  color: #475569;
  cursor: pointer;
  transition: background 0.15s;
  text-align: left;
  border: none;
  background: transparent;
  width: 100%;
}

.demo-item:hover {
  background: #f8fafc;
}

/* Background decoration */
.bg-decoration {
  position: absolute;
  inset: 0;
  overflow: hidden;
  pointer-events: none;
}

.bg-circle {
  position: absolute;
  border-radius: 50%;
  opacity: 0.07;
}

.bg-circle-1 {
  width: 600px;
  height: 600px;
  background: #3b82f6;
  top: -200px;
  right: -200px;
}

.bg-circle-2 {
  width: 400px;
  height: 400px;
  background: #8b5cf6;
  bottom: -100px;
  left: -100px;
}

.bg-circle-3 {
  width: 250px;
  height: 250px;
  background: #06b6d4;
  bottom: 100px;
  right: 100px;
}
</style>
