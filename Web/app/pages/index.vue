<template>
  <div>
    <h2 class="text-2xl font-bold mb-6">
      Thông tin Tổng Quan
    </h2>

    <!-- Thống kê -->
    <div class="grid grid-cols-4 gap-4 mb-8">
      <UCard>
        <div class="text-center">
          <p class="text-3xl font-bold text-blue-600">12</p>
          <p class="text-gray-500">Công Trình Đang Thi Công</p>
        </div>
      </UCard>
      <UCard>
        <div class="text-center">
          <p class="text-3xl font-bold text-green-600">85</p>
          <p class="text-gray-500">Nhân Công Đang Làm Việc</p>
        </div>
      </UCard>
      <UCard>
        <div class="text-center">
          <p class="text-3xl font-bold text-yellow-600">5</p>
          <p class="text-gray-500">Công Trình Sắp Hoàn Thành</p>
        </div>
      </UCard>
      <UCard>
        <div class="text-center">
          <p class="text-3xl font-bold text-red-600">2</p>
          <p class="text-gray-500">Công Trình Trễ Tiến Độ</p>
        </div>
      </UCard>
    </div>

    <!-- Bảng công trình gần đây -->
    <UCard>
      <template #header>
        <h3 class="font-bold">Công Trình Gần Đây</h3>
      </template>
      <UTable :data="congTrinhs" :columns="columns" />
    </UCard>
  </div>
</template>

<script setup>
const columns = [
  {
    accessorKey: 'ten',
    header: 'Tên Công Trình'
  },
  {
    accessorKey: 'diaDiem',
    header: 'Địa Điểm'
  },
  {
    accessorKey: 'tienDo',
    header: 'Tiến Độ'
  },
  {
    accessorKey: 'trangThai',
    header: 'Trạng Thái'
  }
]

const congTrinhs = [
  { ten: 'Chung cư Sunrise', diaDiem: 'TP.HCM', tienDo: '75%', trangThai: 'Đang thi công' },
  { ten: 'Cầu Bình Lợi', diaDiem: 'Bình Dương', tienDo: '40%', trangThai: 'Đang thi công' },
  { ten: 'Nhà máy ABC', diaDiem: 'Đồng Nai', tienDo: '95%', trangThai: 'Sắp hoàn thành' }
]
</script>
