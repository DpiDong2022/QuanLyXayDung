<template>
  <div class="flex h-screen">
    <!-- Sidebar -->
    <aside class="w-64 bg-gray-900 text-white p-4">
      <h1 class="text-xl font-bold mb-8">
        <NuxtLink to="/" class="flex items-center gap-2">
          🏗️ Quản lý Xây Dựng
        </NuxtLink>
      </h1>
      <nav class="space-y-2">
        <NuxtLink to="/" class="flex items-center gap-2 p-2 rounded hover:bg-gray-700">
          📊 Dashboard
        </NuxtLink>
        <NuxtLink to="/cong-trinh" class="flex items-center gap-2 p-2 rounded hover:bg-gray-700">
          🏢 Công Trình
        </NuxtLink>
        <NuxtLink to="/nhan-cong" class="flex items-center gap-2 p-2 rounded hover:bg-gray-700">
          👷 Nhân Công
        </NuxtLink>
        <NuxtLink to="/vat-tu" class="flex items-center gap-2 p-2 rounded hover:bg-gray-700">
          🧱 Vật Tư
        </NuxtLink>
        <NuxtLink to="/bao-cao" class="flex items-center gap-2 p-2 rounded hover:bg-gray-700">
          📈 Báo Cáo
        </NuxtLink>
        <UButton
          icon="i-heroicons-arrow-right-on-rectangle"
          color="error"
          variant="ghost"
          @click="logout"
        >
          Đăng xuất
        </UButton>
      </nav>
    </aside>

    <!-- Main content -->
    <main class="flex-1 p-6 overflow-auto bg-gray-100">
      <slot />
    </main>
  </div>
</template>

<script setup lang="ts">
import { useAuth } from '~/composables/useAuth'

const { logout } = useAuth()
</script>
