<template>
  <div>
    <h1 class="text-2xl font-bold">Quản lý công ty</h1>
    <USeparator color="info" size="sm" class="pt-3 pb-5" />
    <div class="flex justify-end mb-4">
      <UButton class="mb-4" color="primary" size="xl" @click="open = true; resetForm()">Thêm công ty</UButton>
    </div>
    <UTable
      :columns="columns"
      :loading="pending"
      :data="congtys"
    >
      <template #empty>
        <div class="text-center py-10">
          <p v-if="!pending && congtys && congtys.length === 0" class="text-gray-500">Không có công ty nào.</p>
          <p v-else-if="pending" class="text-gray-500">Đang tải dữ liệu...</p>
          <p v-else class="text-red-500">Lỗi khi tải dữ liệu.</p>
        </div>
      </template>
    </UTable>

    <UModal v-model:open="open" :title="modalTitle" description="" :ui="{ footer: 'justify-end' }" @update:open="(val: boolean) => { if (!val) { resetForm() } }">
      <template #body>
        <UForm class="space-y-6">
          <!-- Tên -->
          <UFormField label="Tên công ty" name="ten" required :error="loi.ten" class="mb-3">
            <UInput v-model="state.ten" size="xl" class="w-full" />
          </UFormField>

          <!-- Địa chỉ -->
          <UFormField label="Địa chỉ" name="diaChi" required :error="loi.diaChi" class="mb-3">
            <UInput v-model="state.diaChi" size="xl" class="w-full"/>
          </UFormField>

          <!-- SĐT -->
          <UFormField label="Số điện thoại" name="sdt" required :error="loi.sdt"  class="mb-3">
            <UInput v-model="state.sdt" size="xl" class="w-full"/>
          </UFormField>

          <!-- Email -->
          <UFormField label="Email" name="email" required :error="loi.email"  class="mb-3">
            <UInput v-model="state.email" type="email" size="xl" class="w-full"/>
          </UFormField>

          <!-- Mã số thuế -->
          <UFormField label="Mã số thuế" name="maSoThue" required :error="loi.maSoThue" class="mb-3">
            <UInput v-model="state.maSoThue" size="xl" class="w-full" />
          </UFormField>

          <!-- Plan -->
          <UFormField label="Gói dịch vụ" name="plan" :error="loi.plan" class="mb-3" required>
            <USelect
              v-model="state.plan"
              :items="planOptions"
              size="xl"
              class="w-full"
            />
          </UFormField>
        </UForm>
      </template>

      <template #footer="{ close }">
        <UButton label="Cancel" color="neutral" variant="outline" @click="{ close(); open=false }" size="xl">Đóng</UButton>
        <UButton label="Submit" color="primary" size="xl" :loading="loading" @click="onSubmit">Lưu</UButton>
      </template>
    </UModal>
    <UModal v-model:open="openDelete" :ui="{ footer: 'justify-end' }">
      <template #header>
        <h3 class="font-bold">Xác nhận xóa</h3>
      </template>
      <template #body>
        <p>Bạn có chắc muốn xóa công ty này không?</p>
      </template>
      <template #footer>
        <div class="flex justify-end gap-2">
          <UButton color="neutral" variant="outline" size="xl" @click="openDelete = false">Hủy</UButton>
          <UButton color="error" :loading="loadingXoa" size="xl" @click="xoaCongTy()">Xóa</UButton>
        </div>
      </template>
    </UModal>
  </div>
</template>

<script setup lang="ts">
import type { BaseResponse } from '~/types/ApiResponse'
import type { CongTyCreate, CongTy } from '~/types/CongTy'
import { formatDate_ddMMyyyy } from '~/utils/Utils'
import type { FetchError } from 'ofetch'
import { UButton, UBadge } from '#components'

const toast = useToast()
const open = ref(false)
const error = ref('')

const columns = [
  {
    accessorKey: 'ten',
    header: 'Tên Công Ty'
  },
  {
    accessorKey: 'sdt',
    header: 'Số điện thoại'
  },
  {
    accessorKey: 'email',
    header: 'Email'
  },
  {
    accessorKey: 'maSoThue',
    header: 'Mã số thuế'
  },
  {
    accessorKey: 'plan',
    header: 'Gói thuê phần mềm',
    cell: ({ row }) => {
      const plan = row.original.plan
      if (plan === 1)
        return h(UBadge, { color: 'neutral', variant: 'soft' }, 'Thường')

      if (plan === 2)
        return h(UBadge, { color: 'primary', variant: 'soft' }, 'Víp')

      return 'Không xác định'
    }
  },
  {
    accessorKey: 'ngayTao',
    header: 'Ngày tạo',
    cell: ({ row }) => {
      return formatDate_ddMMyyyy(row.original.ngayTao)
    }
  },
  {
    accessorKey: '',
    header: ' ',
    cell: ({ row }) => {
      return h('div', { class: 'flex gap-2' }, [
        h(UButton, {
          color: 'error',
          variant: 'ghost',
          size: 'xs',
          icon: 'i-heroicons-trash',
          onClick: () => {
            deleteId.value = row.original.id
            openDelete.value = true
          }
        }),
        h(UButton, {
          color: 'primary',
          variant: 'ghost',
          size: 'xs',
          icon: 'i-heroicons-pencil-square',
          onClick: () => {
            console.log(row.original)
            resetForm()
            openEditCongTy(row.original)
          }
        })
      ])
    }
  }
]

const { get, post, put, del } = useApi()
const { data: congtys, pending, refresh } = useAsyncData('congtys', () => get<CongTy[]>('/cong-ty'))
// Force refresh khi mount
onMounted(() => refresh())

const state = reactive<CongTyCreate>({
  ten: '',
  diaChi: '',
  sdt: '',
  email: '',
  maSoThue: '',
  plan: 1
})

const loi = ref({
  id: '',
  ten: '',
  diaChi: '',
  sdt: '',
  email: '',
  maSoThue: '',
  plan: ''
})

const planOptions = [
  { label: 'Thường', value: 1 },
  { label: 'Víp', value: 2 }
]

const loading = ref(false)
const loadingXoa = ref(false)
function validate() {
  loi.value = {
    id: '',
    ten: '',
    diaChi: '',
    sdt: '',
    email: '',
    maSoThue: '',
    plan: ''
  }
  if (!state.ten) loi.value.ten = 'Tên công ty không được để trống'
  if (!state.email) loi.value.email = 'Email không được để trống'
  if (!state.sdt) loi.value.sdt = 'Số điện thoại không được để trống'
  if (!state.maSoThue) loi.value.maSoThue = 'Mã số thuế không được để trống'
  if (!state.plan) loi.value.plan = 'Vui lòng chọn gói dịch vụ'
  return !Object.values(loi.value).some(v => v !== '')
}

async function onSubmit() {
  if (!validate()) {
    return
  }
  loading.value = true
  const isEdit = editId.value != ''
  try {
    if (!isEdit) {
      const res = await post<string>('/cong-ty', state)
      console.log('Tạo công ty thành công:', res)
    } else {
      await put('/cong-ty', {
        ...state,
        id: editId.value
      })
      console.log('Sửa công ty thành công')
    }
    open.value = false
    await refresh()
    toast.add({
      title: 'Thành công',
      description: `Đã ${isEdit ? 'sửa' : 'tạo'} công ty`,
      color: 'primary'
    })
  } catch (err: unknown) {
    console.error(`Lỗi khi ${isEdit ? 'sửa' : 'tạo'} công ty:`, err)
    const error = (err as FetchError<BaseResponse>)?.data?.message || `Có lỗi xảy ra khi ${isEdit ? 'sửa' : 'tạo'} công ty`
    toast.add({
      title: 'Lỗi',
      description: error,
      color: 'error'
    })
    loading.value = false
  }
}
function resetForm() {
  console.log('start reset')
  state.ten = ''
  state.diaChi = ''
  state.sdt = ''
  state.email = ''
  state.maSoThue = ''
  state.plan = 1
  loi.value = {
    id: '',
    ten: '',
    diaChi: '',
    sdt: '',
    email: '',
    maSoThue: '',
    plan: ''
  }
  modalTitle.value = 'Tạo mới thông tin công ty'
  deleteId.value = ''
  editId.value = ''
  loading.value = false
  error.value = ''
  console.log('end reset')
}
const deleteId = ref('')
const openDelete = ref(false)
function xoaCongTy() {
  loadingXoa.value = true
  del(`/cong-ty/${deleteId.value}`).then(() => {
    if (congtys.value) {
      congtys.value = congtys.value.filter(c => c.id !== deleteId.value)
    }
    toast.add({
      title: 'Thành công',
      description: 'Đã xóa công ty',
      color: 'primary'
    })
  }).finally(() => {
    loadingXoa.value = false
    openDelete.value = false
  })
}
const modalTitle = ref('Tạo mới thông tin công ty')
const editId = ref('' as string | undefined)
function openEditCongTy(value: CongTy) {
  open.value = true
  modalTitle.value = 'Chỉnh sửa thông tin công ty'
  editId.value = value.id
  state.diaChi = value.diaChi
  state.email = value.email
  state.maSoThue = value.maSoThue
  state.plan = value.plan
  state.ten = value.ten
  state.sdt = value.sdt
}
</script>

<style scoped>
</style>
