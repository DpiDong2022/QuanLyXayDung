export const formatDate_dMyyyy = (dateString: string) => new Date(dateString).toLocaleDateString('vi-VN')

export const formatDate_ddMMyyyy = (dateString: string) =>
  new Date(dateString).toLocaleDateString('vi-VN', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
  })
